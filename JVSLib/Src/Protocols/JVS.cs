using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

using JVSLib.Src.DataTypes;

namespace JVSLib.Src.Protocols
{
    class JVS
    {
        string port;
        SerialPort sPort;

        public bool connected { get; private set; }
        public int wait = 10;

        public JVS(string port, int readTimeout = 2000, int writeTimeout = 2000)
        {
            this.port = port;
            sPort = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
            sPort.ReadTimeout = readTimeout;
            sPort.WriteTimeout = writeTimeout;
        }

        public void Connect()
        {
            try
            {
                sPort.Open();
                connected = sPort.IsOpen;
            }
            catch
            {
                connected = false;
            }
        }

        public void Send(JVSReq req)
        {
            var bw = new BinaryWriter(new MemoryStream());
            bw.Write(JVSCodes.JVSSync);
            bw.Write(req.nodeNum);
            bw.Write((byte)(req.data.Length + 1));
            bw.Write(req.data);
            bw.Write(Checksum8.Calculate(((MemoryStream)bw.BaseStream).ToArray(), 1));
            sPort.Write(((MemoryStream)bw.BaseStream).ToArray(), 0, (int)((MemoryStream)bw.BaseStream).Length);
            Thread.Sleep(wait);
        }

        public JVSRes Recieve()
        {
            try
            {

                if (sPort.ReadByte() == JVSCodes.JVSSync)
                {
                    var res = new JVSRes();
                    res.nodeNum = sPort.EscapedReadByte();
                    byte dataLen = sPort.EscapedReadByte();
                    res.status = (JVSStatus)sPort.EscapedReadByte();
                    var buf = new byte[dataLen - 2];
                    sPort.EscapedRead(ref buf, 0, buf.Length);
                    res.data = buf;
                    byte givenSum = sPort.EscapedReadByte();
                    byte calcSum = (byte)(Checksum8.Calculate(res) + 1);
                    if (givenSum == calcSum)
                    {
                        return res;
                    }
                    else
                    {
                        var req = new JVSReq();
                        req.nodeNum = 1;
                        req.data = new byte[] { 0x2F };
                        Send(req);
                        Recieve();
                    }
                    return null;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void Close()
        {
            sPort.Close();
            connected = sPort.IsOpen;
        }
    }
}
