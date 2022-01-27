using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JVSLib.Src.Protocols;
using JVSLib.Src.DataTypes;

namespace JVSLib.Src
{
    public class JVSConnection
    {
        JVS jvs;

        public bool connected { get { return jvs.connected; } }
        public int wait { get { return jvs.wait; } set { jvs.wait = value; } }

        public JVSConnection(string port = "COM2")
        {
            jvs = new JVS(port);
        }

        public void Connect()
        {
            jvs.Connect();
        }

        public void Reset()
        {
            var req = new JVSReq();
            req.nodeNum = 0xFF;
            req.data = new byte[] { 0xF0, 0xD9 };
            jvs.Send(req);
        }

        public JVSSetAddress SetAddress(byte address)
        {
            var req = new JVSReq();
            req.nodeNum = 0xFF;
            req.data = new byte[] { 0xF1, address };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSSetAddress();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                }
                return data;
            }
            return null;
        }

        public void ChangeCommMethod(byte methodCode)
        {
            var req = new JVSReq();
            req.nodeNum = 0xFF;
            req.data = new byte[] { 0xF2, methodCode };
            jvs.Send(req);
        }

        public JVSIOIdentify IOIdentify(byte nodeNum)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x10 };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSIOIdentify();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    data.identifier = Encoding.ASCII.GetString(res.data, 1, res.data.Length - 1).Trim('\0');
                }
                return data;
            }
            return null;
        }

        public JVSCmdRev CommandRevision(byte nodeNum)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x11 };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSCmdRev();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    data.rev = res.data[1];
                }
                return data;
            }
            return null;
        }

        public JVSRev JVSRevision(byte nodeNum)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x12 };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSRev();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    data.rev = res.data[1];
                }
                return data;
            }
            return null;
        }

        public JVSCommVer CommunicationVersion(byte nodeNum)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x13 };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSCommVer();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    data.ver = res.data[1];
                }
                return data;
            }
            return null;
        }

        public JVSFeatCheck FeatureCheck(byte nodeNum)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x14 };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSFeatCheck();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    if (data.report == JVSReport.Normal)
                    {
                        int i = 1;
                        while (i < res.data.Length - 4)
                        {
                            byte featType = res.data[i];
                            if (featType == 0) { break; }
                            var feature = new JVSFeatCheck.FeatureParam();
                            feature.param1 = res.data[i + 1];
                            feature.param2 = res.data[i + 2];
                            feature.param3 = res.data[i + 3];
                            data.features.Add((JVSFeatCheck.FeatType)featType, feature);
                            i += 4;
                        }
                    }
                }
                return data;
            }
            return null;
        }

        public JVSMainID MainID(byte nodeNum, string pcbID)
        {
            if (pcbID.Length > 100)
            {
                return null;
            }
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = Encoding.ASCII.GetBytes("\x15" + pcbID + "\0");
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSMainID();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                }
                return data;
            }
            return null;
        }

        public JVSSwInp SwitchInputs(byte nodeNum, byte numPlayers, byte numSwitches)
        {
            byte dataBytes = (byte)Math.Ceiling((decimal)numSwitches / 8);
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x20, numPlayers, dataBytes };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSSwInp();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    if (data.report == JVSReport.Normal)
                    {
                        data.systemInputs = new bool[8];
                        for (byte s = 0; s < data.systemInputs.Length; s++)
                        {
                            data.systemInputs[s] = res.data[1].GetBit(s);
                        }
                        data.inputs = new bool[numPlayers, numSwitches];
                        for (byte p = 0; p < numPlayers; p++)
                        {
                            for (byte s = 0; s < numSwitches; s++)
                            {
                                int currByte = (p * dataBytes) + (s / 8);
                                int currBit = s % 8;
                                //Swap endianness
                                currBit = 7 - currBit;
                                data.inputs[p, s] = res.data[2 + currByte].GetBit(currBit);
                            }
                        }
                    }
                }
                return data;
            }
            return null;
        }
        
        public JVSCoinInp CoinInputs(byte nodeNum, byte slotCount)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x21, slotCount };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSCoinInp();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    for (int i=1; i<1+slotCount*2; i+=2)
                    {
                        //Swap endianness
                        //res.data[i] = res.data[i].SwapBits();
                        //res.data[i + 1] = res.data[i + 1].SwapBits();
                        short slotData = BitConverter.ToInt16(res.data, i).SwapBits();
                        var slot = new JVSCoinInp.CoinSlot();
                        slot.condition = (JVSCoinInp.SlotCondition)(slotData & 0x3);
                        slot.coinCount = (short)(slotData >> 2);
                        data.slots.Add(slot);
                    }
                }
                return data;
            }
            return null;
        }

        public JVSAnalogInp AnalogInputs(byte nodeNum, byte channels)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x22, channels };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSAnalogInp();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    if (data.report == JVSReport.Normal)
                    {
                        for (int i = 1; i < 1 + channels * 2; i += 2)
                        {
                            byte[] value = new byte[] { res.data[i + 1], res.data[i] };
                            data.channels.Add((ushort)(BitConverter.ToUInt16(value, 0) >> 6));
                        }
                    }
                }
                return data;
            }
            return null;
        }

        public JVSRotaryInp RotaryInputs(byte nodeNum, byte channels)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x23, channels };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSRotaryInp();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    if (data.report == JVSReport.Normal)
                    {
                        for (int i = 1; i < 1 + channels * 2; i += 2)
                        {
                            data.channels.Add(BitConverter.ToInt16(res.data, i));
                        }
                    }
                }
                return data;
            }
            return null;
        }

        public JVSKeyInp KeyInputs(byte nodeNum)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x24 };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSKeyInp();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    data.keyCode = res.data[1];
                }
                return data;
            }
            return null;
        }

        public JVSScrPosInp ScreenPositionInputs(byte nodeNum, byte channelIndex)
        {
            var req = new JVSReq();
            req.nodeNum = nodeNum;
            req.data = new byte[] { 0x25, channelIndex };
            jvs.Send(req);
            var res = jvs.Recieve();
            if (res != null)
            {
                var data = new JVSScrPosInp();
                data.status = res.status;
                if (res.status == JVSStatus.Normal)
                {
                    data.report = (JVSReport)res.data[0];
                    data.X = BitConverter.ToUInt16(res.data, 1);
                    data.Y = BitConverter.ToUInt16(res.data, 3);
                }
                return data;
            }
            return null;
        }

        public void Close()
        {
            jvs.Close();
        }

        ~JVSConnection()
        {
            Close();
        }
    }
}
