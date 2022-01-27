using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace JVSLib_BG4FFB.Src.Protocols
{
    class FFB
    {
        string port;
        SerialPort sPort;

        public bool connected { get; private set; }
        public int wait = 10;

        public FFB(string port, int readTimeout = 2000, int writeTimeout = 2000)
        {
            this.port = port;
            sPort = new SerialPort(port, 38400, Parity.None, 8, StopBits.One);
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

        public byte[] Send(byte cmd, byte param)    //Integrated recieve as every command seems to always return
        {
            var bw = new BinaryWriter(new MemoryStream());
            bw.Write(cmd);
            bw.Write(param);
            sPort.Write(((MemoryStream)bw.BaseStream).ToArray(), 0, (int)((MemoryStream)bw.BaseStream).Length);
            Thread.Sleep(wait);
            try
            {
                return new byte[] { (byte)sPort.ReadByte(), (byte)sPort.ReadByte() };
            }
            catch
            {
                return null;
            }
        }

        //public byte[] Recieve()
        //{
        //    try
        //    {
        //        return new byte[] { (byte)sPort.ReadByte(), (byte)sPort.ReadByte() };
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public void Close()
        {
            sPort.Close();
            connected = sPort.IsOpen;
        }
    }
}
