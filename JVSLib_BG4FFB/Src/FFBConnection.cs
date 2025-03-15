using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JVSLib_BG4FFB.Src.Protocols;
using JVSLib_BG4FFB.Src.DataTypes;

namespace JVSLib_BG4FFB.Src
{
    //    var res = ffb.Recieve();
    //            if (res != null)
    //            {
    //                var value = BitConverter.ToInt16(res, 0);
    //                if (value != 0x00A0)
    //                {
    //                    for (int i=0; i<6; i++)
    //                    {
    //                        ffb.Send(0x1F, 0x00);
    //                        ffb.Recieve();
    //                    }
    //                    Reset();
    //                }
    //            }

    public class FFBConnection
    {
        FFB ffb;

        public bool connected { get { return ffb.connected; } }
        public int wait { get { return ffb.wait; } set { ffb.wait = value; } }

        public bool initialised { get; private set; }
        public bool usePolling { get; set; }

        Queue<Tuple<byte, byte>> commandQueue = new Queue<Tuple<byte, byte>>();

        public FFBConnection(string port = "COM1")
        {
            ffb = new FFB(port);
        }

        public void Connect()
        {
            ffb.Connect();
        }

        public void Calibrate() //This may be depricated soon?
        {
            Reset();
            for (int i=0; i<6; i++)
                MotorStop();
            if (Reset() != 0x00A0)  //Likely not ready to calibrate?
                return;
            EnableMotorPower();   //Turns on motor power circuit (high pitched whine from motor board)
            UnknownOperation_18();   //Nothing?
            if (BeginCalibration())
            {
                //TODO: Do we put a wait here, there is no way to tell if its done, game just uses a 30 second timer
                initialised = true;
            }
        }

        public void SetNoOperation()
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x00, 0x00));
            }
            else { ffb.Send(0x00, 0x00); }
        }

        public void SetRollForce(byte position)   //TODO: This is super tempermental if SetSteeringRadius is not set!!!
                                                     //NOTE: Game always sets this to halfway (0x80)
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x01, position));
            }
            else { ffb.Send(0x01, position); }
        }

        public void SetFrictionForce(byte force)  //Used by game, also used during init procedure
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x02, force));
            }
            else { ffb.Send(0x02, force); }
        }

        public void SetSpringForce(byte force)  //TODO: This can have a different effect depending on commands before it? Investigate before use!
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x03, force));
            }
            else { ffb.Send(0x03, force); }
        }

        public void SetFrictionForce2(byte force)  //Used by game, also used during init procedure, Why is it weaker??? Does it actually do this? (Something makes it weaker unsure what, global strength applies to both)
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x04, force));
            }
            else { ffb.Send(0x04, force); }
        }

        public void SetRumbleFrequency(byte frequency)  //Used by game, Param seems to be always 0, but double check this, also used during init procedure
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x05, frequency));
            }
            else { ffb.Send(0x05, frequency); }
        }

        public void SetRumbleStrength(byte strength)  //Used by game, Param seems to be always 0, but double check this, also used during init procedure
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x06, strength));
            }
            else { ffb.Send(0x06, strength); }
        }

        public void SetGlobalStrength(byte strength)  //Used after calibration during init, param is normally 7F, pretty sure this is global strength, could be something else, but it affects all commands APART FROM RUMBLE, SIGNED BYTE
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x07, strength));
            }
            else { ffb.Send(0x07, strength); }
        }

        public void UnknownOperation_08(byte param)  //Used after calibration during init, param is normally F
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x08, param));
            }
            else { ffb.Send(0x08, param); }
        }

        public void EnableMotorPower()  //???, used in first step after reset and motor stop in calibration procedure
        {
            ffb.Send(0x0A, 0x5A);
        }

        public void SetMotorDeadzone(byte param)  //Used by game, also used during init procedure
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x0B, param));
            }
            else { ffb.Send(0x0B, param); }
        }

        public bool BeginCalibration()
        {
            var res = ffb.Send(0x0C, 0xA0);
            if (res != null)
            {
                if (BitConverter.ToUInt16(res, 0) == 0xA08C)
                {
                    initialised = true;
                    return true;
                }
            }
            return false;
        }

        public void SetSpringCenter(byte param)  //Used by game, also used during init procedure, SOMETHING SETS THE SPEED FOR THIS! Its a smooth transition to the new target pos
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x0D, param));
            }
            else { ffb.Send(0x0D, param); }
        }

        public void SetSpringCenterRadius(byte param)  //Used after calibration during init, param is normally 3, unsure 100% but BE CAREFUL WITH THIS COMMAND! It can cause values from setCenter to be out of range and motor will keep ramping up power even when hitting the stopper, dont burn out your motor/board :P
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x0E, param));
            }
            else { ffb.Send(0x0E, param); }
        }

        public void ReportSteeringPos()  //Used after calibration during init, param is normally 3, makes board start reporting steering position but ONLY if you follow game packet order?
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x11, 0x03));
            }
            else { ffb.Send(0x11, 0x03); }
        }

        public void SetSteeringRadius(byte radius)  //Used by game, usually set to 0x18 for SD, 0x0E for HD. lower = higher steering angle?
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x12, radius));
            }
            else { ffb.Send(0x12, radius); }
        }

        public void UnknownOperation_13()  //Used after calibration during init, param is normally 1
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x13, 0x01));
            }
            else { ffb.Send(0x13, 0x01); }
        }

        public bool GetCalibrationStatus()  //Used after calibration during init, param is normally 10
        {
            var res = ffb.Send(0x14, 0x1A);
            if (res != null)
            {
                return res[0] < 0x80;
            }
            return false;
        }

        public void UnknownOperation_15(byte param)  //Used after calibration during init, param is normally 1, also used later on with param of 10
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x15, param));
            }
            else { ffb.Send(0x15, param); }
        }

        public void UnknownOperation_16(byte param)  //Used after calibration during init, param is normally 10
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x16, param));
            }
            else { ffb.Send(0x16, param); }
        }

        public void UnknownOperation_18()  //Used during calibration procedure
        {
            ffb.Send(0x18, 0x81);
        }

        public void UnknownOperation_1A(byte param)  //Sometimes sets centering spring? Changing parameter doesnt do anything but its normally 5, used after calibration during init
        {
            if (usePolling)
            {
                commandQueue.Enqueue(new Tuple<byte, byte>(0x1A, param));
            }
            else { ffb.Send(0x1A, param); }
            //TODO: Does this need an extra read? Game reads 4 bytes for this, but its definetly 2 lots of int16s rather than an int32
        }

        public void MotorStop()
        {
            ffb.Send(0x1F, 0x00);
        }

        public short Reset()
        {
            var res = ffb.Send(0x20, 0x00);
            if (res != null)
            {
                initialised = false;
                return BitConverter.ToInt16(res, 0);
            }
            return short.MinValue;
        }

        public short Poll()
        {
            if (initialised && usePolling)
            {
                byte currentCommand = 0x00;
                byte currentParam = 0x00;
                if (commandQueue.Count > 0)
                {
                    var cmd = commandQueue.Dequeue();
                    currentCommand = cmd.Item1;
                    currentParam = cmd.Item2;
                }
                var res = ffb.Send(currentCommand, currentParam);
                if (res != null)
                {
                    currentCommand = 0x00;
                    currentParam = 0x00;
                    return BitConverter.ToInt16(res, 0);    //Steering pos (unless uninitialised)
                }
            }
            return short.MinValue;
        }

        public byte[] CustomCommand(byte command, byte param)
        {
            var res = ffb.Send(command, param);
            if (res != null)
            {
                return res;
            }
            return null;
        }

        public void Close()
        {
            ffb.Close();
        }

        ~FFBConnection()
        {
            Close();
        }
    }
}
