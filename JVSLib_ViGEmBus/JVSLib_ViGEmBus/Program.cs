using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using JVSLib.Src;
using JVSLib.Src.DataTypes;

using JVSLib_BG4FFB.Src;
using JVSLib_BG4FFB.Src.DataTypes;

using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Nefarius.ViGEm.Client.Exceptions;

namespace JVSLib_ViGEmBus
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Keys vKeys);

        const byte nodeNum = 1;
        const int pollingRate = 100;

        static FFBConnection ffb = new FFBConnection();

        static JVSConnection jvs = new JVSConnection();
        static ViGEmClient vigem = new ViGEmClient();
        static List<IXbox360Controller> x360 = new List<IXbox360Controller>();

        static bool ffbOk = false;

        static bool jvsOk = false;
        static bool jvsPoll = true;

        static byte players = 1;
        static byte switchBits = 19;
        static bool analog = false;
        static byte analogChannels = 0;
        static byte analogBits = 0;

        static Dictionary<byte, ushort> calibrationValuesMin = new Dictionary<byte, ushort>();
        static Dictionary<byte, ushort> calibrationValuesMax = new Dictionary<byte, ushort>();

        static void Main(string[] args)
        {
            Console.WriteLine("JVSLib_ViGEmBus v1.0");
            jvs.Connect();
            if (jvs.connected)
            {
                jvs.Reset();
                jvs.SetAddress(nodeNum);
                var identify = jvs.IOIdentify(nodeNum);
                if (identify != null && identify.status == JVSStatus.Normal && identify.report == JVSReport.Normal)
                {
                    Console.WriteLine("JVS I/O is \"{0}\"", identify.identifier);
                }
                var features = jvs.FeatureCheck(nodeNum);
                if (features != null && features.status == JVSStatus.Normal && features.report == JVSReport.Normal)
                {
                    if (features.features.ContainsKey(JVSFeatCheck.FeatType.Switch))
                    {
                        players = features.features[JVSFeatCheck.FeatType.Switch].param1;
                        for (int i = 0; i < players; i++)
                        {
                            var con = vigem.CreateXbox360Controller();
                            con.Connect();
                            x360.Add(con);
                        }
                        switchBits = features.features[JVSFeatCheck.FeatType.Switch].param2;
                        while (switchBits > x360.Count * 10)
                        {
                            var con = vigem.CreateXbox360Controller();
                            con.Connect();
                            x360.Add(con);
                        }
                        analog = features.features.ContainsKey(JVSFeatCheck.FeatType.Analog);
                        if (analog)
                        {
                            analogChannels = features.features[JVSFeatCheck.FeatType.Analog].param1;
                            analogBits = features.features[JVSFeatCheck.FeatType.Analog].param2;
                            for (byte i=0; i<analogChannels; i++)
                            {
                                calibrationValuesMin.Add(i, 0);
                                calibrationValuesMax.Add(i, 1023);
                            }
                            CalibrateMinJVS();
                            //CalibrateMaxJVS();
                            while (analogChannels + 1 > x360.Count * 4)
                            {
                                var con = vigem.CreateXbox360Controller();
                                con.Connect();
                                x360.Add(con);
                            }
                            Console.WriteLine("JVS I/O Analog OK!");
                        }
                        jvsOk = true;
                        Console.WriteLine("JVS I/O OK!");
                        InitFFB();
                        var mainThread = new Thread((ThreadStart)delegate
                        {
                            while (jvsOk)
                            {
                                MainLoop();
                                //Thread.Sleep(1000 / pollingRate);
                            }

                        });
                        var ffbThread = new Thread((ThreadStart)delegate
                        {
                            while (ffbOk)
                            {
                                FFBLoop();
                                //Thread.Sleep(1000 / pollingRate);
                            }

                        });
                        mainThread.Start();
                        ffbThread.Start();
                        while (true)
                        {
                            var cmd = Console.ReadLine().Split(' ');
                            switch (cmd[0])
                            {
                                case "exit":
                                    jvsOk = false;
                                    ffbOk = false;
                                    foreach (var con in x360)
                                    {
                                        con.Disconnect();
                                    }
                                    return;
                                case "calibrate":
                                    if (analog)
                                    {
                                        jvsPoll = false;
                                        Console.WriteLine("Please release all analog devices, and press any key...");
                                        Console.ReadKey();
                                        if (CalibrateMinJVS())
                                        {
                                            Console.WriteLine("Please press down all analog devices, and press any key...");
                                            Console.ReadKey();
                                            if (CalibrateMaxJVS())
                                            {
                                                Console.WriteLine("Calibration completed successfully!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("JVS I/O is not responding!");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("JVS I/O is not responding!");
                                        }
                                        jvsPoll = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The connected JVS I/O does not support analog!");
                                    }
                                    break;
                                case "ffb_stop":
                                    if (ffbOk)
                                    {
                                        ffb.MotorStop();
                                    }
                                    else
                                    {
                                        Console.WriteLine("FFB is not initialised!");
                                    }
                                    break;
                                case "ffb_reset":
                                    if (ffbOk)
                                    {
                                        ffb.Reset();
                                        ffbOk = false;
                                        Console.WriteLine("WARNING: Steering board will no longer work until re initialisation!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("FFB is not initialised!");
                                    }
                                    break;
                                case "ffb_spring":
                                    if (ffbOk)
                                    {
                                        ffb.usePolling = false;
                                        ffb.SetMotorDeadzone(0x22);
                                        ffb.UnknownOperation_08(0x0F);
                                        ffb.SetGlobalStrength(0x7F);
                                        //ffb.SetCenterForce(0x19);
                                        ffb.SetCenterForce((byte)(cmd.Length > 1 ? byte.Parse(cmd[1]) : 0x19));
                                        ffb.SetResistanceForce(0x00);
                                        ffb.SetWeakerResistanceForce(0x00);
                                        ffb.SetSpringCenter(0x80);
                                        ffb.SetRumbleFrequency(0x00);
                                        ffb.SetRumbleStrength(0x00);
                                        ffb.SetSteeringRadius(0x18);
                                        ffb.UnknownOperation_0E(0x03);
                                        ffb.SetSpringCenter_NoFling(0x80);
                                        ffb.UnknownOperation_15(0x10);
                                        ffb.UnknownOperation_16(0x10);
                                        ffb.UnknownOperation_1A(0x05);
                                        ffb.usePolling = true;
                                        Console.WriteLine("Set steering center spring to {0}", (byte)(cmd.Length > 1 ? byte.Parse(cmd[1]) : 0x19));
                                    }
                                    else
                                    {
                                        Console.WriteLine("FFB is not initialised!");
                                    }
                                    break;
                                case "ffb_gamespring":
                                    if (ffbOk)
                                    {
                                        ffb.usePolling = false;
                                        ffb.SetMotorDeadzone(0x1F);
                                        ffb.UnknownOperation_08(0x0F);
                                        ffb.SetGlobalStrength(0x7F);
                                        ffb.SetCenterForce(0x19);
                                        ffb.SetResistanceForce(0x19);
                                        ffb.SetWeakerResistanceForce(0x00);
                                        ffb.SetSpringCenter(0x80);
                                        ffb.SetRumbleFrequency(0x00);
                                        ffb.SetRumbleStrength(0x00);
                                        ffb.SetSteeringRadius(0x18);
                                        ffb.UnknownOperation_0E(0x03);
                                        ffb.SetSpringCenter_NoFling(0x80);
                                        ffb.UnknownOperation_15(0x10);
                                        ffb.UnknownOperation_16(0x10);
                                        ffb.UnknownOperation_1A(0x05);
                                        ffb.usePolling = true;
                                        Console.WriteLine("WARNING: This is packet capture directly from game, spring force is non adjustable for now");
                                    }
                                    else
                                    {
                                        Console.WriteLine("FFB is not initialised!");
                                    }
                                    break;
                                case "ffb_deadzone":
                                    if (ffbOk)
                                    {
                                        ffb.usePolling = false;
                                        ffb.SetMotorDeadzone((byte)(cmd.Length > 1 ? byte.Parse(cmd[1]) : 0x1F));
                                        ffb.usePolling = true;
                                        Console.WriteLine("Set effect deadzone to {0}", (byte)(cmd.Length > 1 ? byte.Parse(cmd[1]) : 0x1F));
                                    }
                                    else
                                    {
                                        Console.WriteLine("FFB is not initialised!");
                                    }
                                    break;
                                case "ffb_recalibrate":
                                    if (ffbOk)
                                    {
                                        ffb.Close();
                                        ffb = new FFBConnection();
                                        InitFFB();
                                    }
                                    else
                                    {
                                        Console.WriteLine("FFB is not initialised!");
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("JVS I/O does not support switches!");
                    }
                }
                else
                {
                    Console.WriteLine("JVS I/O is not responding!");
                }
            }
            else
            {
                Console.WriteLine("Error connecting to JVS I/O!");
            }
            //InitFFB();
            //if (ffbOk)
            //{
            //    if (x360.Count < 1)
            //    {
            //        var con = vigem.CreateXbox360Controller();
            //        con.Connect();
            //        x360.Add(con);
            //    }
            //    while (true)
            //    {
            //        MainLoop();
            //        //Thread.Sleep(1000 / pollingRate);
            //    }
            //}
            Console.ReadLine();
        }

        static void Test()
        {
            var conA = vigem.CreateXbox360Controller();
            var conB = vigem.CreateXbox360Controller();
            conA.Connect();
            conB.Connect();
            conB.SetButtonState(Xbox360Button.A, true);
            for (short i = short.MinValue; i < short.MaxValue; i++)
            {
                conA.SetAxisValue(0, i);
                Thread.Sleep(10);
            }
            conA.Disconnect();
            conB.Disconnect();
        }

        static bool CalibrateMinJVS()
        {
            var analog = jvs.AnalogInputs(nodeNum, analogChannels);
            if (analog != null)
            {
                for (byte i = 0; i < analog.channels.Count; i++)
                {
                    calibrationValuesMin[i] = analog.channels[i];
                }
                return true;
            }
            return false;
        }

        static bool CalibrateMaxJVS()
        {
            var analog = jvs.AnalogInputs(nodeNum, analogChannels);
            if (analog != null)
            {
                for (byte i = 0; i < analog.channels.Count; i++)
                {
                    calibrationValuesMax[i] = analog.channels[i];
                }
                return true;
            }
            return false;
        }

        static void InitFFB()
        {
            ffb.Connect();
            if (ffb.connected)
            {
                ffb.Reset();
                for (int i = 0; i < 5; i++)
                    ffb.MotorStop();
                ffb.EnableMotorPower();
                ffb.UnknownOperation_18();
                ffb.BeginCalibration();
                Console.WriteLine("Steering board is preparing to calibrate...");
                while (!ffb.GetCalibrationStatus()) { }
                Console.Write("Steering board calibrating... ");
                int initialCursorPos = Console.CursorLeft;
                var startTime = DateTime.UtcNow;
                while ((DateTime.UtcNow - startTime).TotalSeconds < 30)
                {
                    Console.CursorLeft = initialCursorPos;
                    Console.Write(30 - (DateTime.UtcNow - startTime).TotalSeconds);
                    ffb.GetCalibrationStatus();
                    Console.CursorLeft = initialCursorPos;
                    Console.Write("                    ");
                }
                Console.WriteLine();
                Console.WriteLine("Calibration complete!");
                //Set up FFB to output steering position (This also applies centering spring, captured directly from game)
                ffb.SetSteeringRadius(0x18);
                ffb.UnknownOperation_13();
                ffb.ReportSteeringPos();      //Steering pot output starts here
                ffb.MotorStop();
                //If we dont want ffb, we could possibly stop here
                //ffb.UnknownOperation_06(0x1F);
                //ffb.UnknownOperation_05(0x0F);
                //ffb.UnknownOperation_04(0x7F);
                //ffb.SetCenterForce(0x19);
                //ffb.UnknownOperation_00(0x19);
                //ffb.UnknownOperation_01(0x00);
                //ffb.SetWheelPosition(0x80);
                //ffb.UnknownOperation_02();
                //ffb.UnknownOperation_03();
                //ffb.SetSteeringRadius(0x18);
                //ffb.UnknownOperation_08(0x03);
                //ffb.UnknownOperation_07(0x80);
                //ffb.UnknownOperation_12(0x10);
                //ffb.UnknownOperation_13(0x10);
                //ffb.UnknownOperation_15(0x05);
                ffb.usePolling = true;
                ffbOk = true;
                Console.WriteLine("FFB OK!");
            }
            else
            {
                Console.WriteLine("Error connecting to FFB!");
                ffb.Close();
            }
        }

        static void MainLoop()
        {
            if (jvsOk && jvsPoll)
            {
                var sw = jvs.SwitchInputs(nodeNum, players, switchBits);
                if (sw != null)
                {
                    ProcessSw(sw);
                }
                if (analog)
                {
                    var analog = jvs.AnalogInputs(nodeNum, analogChannels);
                    if (analog != null)
                    {
                        ProcessAnalog(analog);
                    }
                }
            }
        }

        static void FFBLoop()
        {
            if (ffbOk)
            {
                ProcessAnalogFFB(ffb.Poll());
            }
        }

        static void ProcessSw(JVSSwInp sw)
        {
            for (byte s = 0; s < 8; s++)
            {
                x360[0].SetButtonState(s, sw.systemInputs[s]);
            }
            for (byte p = 0; p < players; p++)
            {
                for (byte s = 0; s < switchBits; s++)
                {
                    //Console.WriteLine("controllerIndex={0} Button={1} CurrentBit={2}", p + (s / 10), s % 10, s);
                    x360[p + (s / 10)].SetButtonState(s % 10, sw.inputs[p, s]);
                }
            }
        }

        static void ProcessSwDEBUG()
        {
            var keys = new Keys[] { Keys.Up, Keys.Down, Keys.Left, Keys.Right,Keys.D1,Keys.D2,Keys.D3,Keys.D4,Keys.V,Keys.Q,(Keys)0xFF,Keys.W,Keys.A,Keys.S };
            int count = 0;
            foreach (var key in keys)
            {
                x360[0].SetButtonState(count, GetAsyncKeyState(keys[count]) != 0);
                count++;
            }
        }

        static void ProcessAnalog(JVSAnalogInp analog)
        {
            var accelMin = calibrationValuesMin[3];
            var brakeMin = calibrationValuesMin[4];
            var accelMax = calibrationValuesMax[3];
            var brakeMax = calibrationValuesMax[4];
            var accel = analog.channels[3].ConvertRange(accelMin, 1023, 0, accelMax - accelMin);
            var brake = analog.channels[4].ConvertRange(brakeMin, 1023, 0, brakeMax - brakeMin) * -1;
            //x360[0].SetAxisValue(Xbox360Axis.RightThumbX, (short)((accel - calibrationValueAccel).Clamp(0, 1024) * (short.MaxValue / calibrationValueAccel)));
            //x360[0].SetAxisValue(Xbox360Axis.RightThumbY, (short)(((brake - calibrationValueBrake).Clamp(0, 1024) * (short.MaxValue / calibrationValueBrake)) * -1));
            x360[0].SetAxisValue(Xbox360Axis.RightThumbX, (short)Math.Max((short)(accel * (short.MaxValue / (accelMax - accelMin))), (short)0));
            x360[0].SetAxisValue(Xbox360Axis.RightThumbY, (short)Math.Max((short)(brake * (short.MaxValue / (brakeMax - brakeMin))), (short)0));
        }

        //static void ProcessAnalog(JVSAnalogInp analog)
        //{
        //    for (int i=1; i<analogChannels-1; i++)
        //    {
        //        var value = analog.channels[i - 1];
        //        var calibrationValue = calibrationValues[(byte)(i - 1)];
        //        //if (i == 1)
        //        //{
        //        //    Console.WriteLine("UC: {0} C: {1} CV: {2} M: {3}", value, (short)((value - calibrationValue) * (short.MaxValue / calibrationValue)), calibrationValue, short.MaxValue / calibrationValue);
        //        //}
        //        if (i > 3) //is a hack to bypass useless channels on bg4 IO
        //        {
        //            x360[i / 4].SetAxisValue(i % 4, (short)((value - calibrationValue) * (short.MaxValue / calibrationValue)));
        //        }
        //    }
        //    //Console.WriteLine();
        //}

        static byte wrapCount = 0;
        static short prevValue = 0;

        static void ProcessAnalogFFB(short steeringPos)
        {
            if (prevValue - steeringPos > 32000)
                wrapCount++;
            if (prevValue - steeringPos < -32000)
                wrapCount--;
            var value = ((ushort.MaxValue * wrapCount) + steeringPos);
            x360[0].SetAxisValue(0, (short)(value / 4));
            prevValue = steeringPos;
        }
    }
}
