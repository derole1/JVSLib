using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DebugApp2
{
    class Program
    {
        class Checksum8
        {
            public static byte Calculate(byte[] data, int startOffset = 0, int endOffset = 0)
            {
                try
                {
                    int total = 0;
                    for (int i = startOffset; i < data.Length - endOffset; i++)
                    {
                        total += data[i];
                    }
                    return (byte)(total % 256);
                }
                catch
                {
                    return 0;
                }
            }

            public static byte Calculate(JVSRes res)
            {
                try
                {
                    var bw = new BinaryWriter(new MemoryStream());
                    bw.Write(res.nodeNum);
                    bw.Write((byte)(res.data.Length + 1));
                    bw.Write((byte)res.status);
                    bw.Write(res.data);
                    var data = ((MemoryStream)bw.BaseStream).ToArray();
                    int total = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        total += data[i];
                    }
                    return (byte)(total % 256);
                }
                catch
                {
                    return 0;
                }
            }
        }

        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;

        const uint MAPVK_VK_TO_VSC = 0x00;
        const uint MAPVK_VSC_TO_VK = 0x01;
        const uint MAPVK_VK_TO_CHAR = 0x02;
        const uint MAPVK_VSC_TO_VK_EX = 0x03;
        const uint MAPVK_VK_TO_VSC_EX = 0x04;

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, ushort bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern ushort MapVirtualKeyEx(byte uCode, uint uMapType, IntPtr dwhkl);

        static void Main(string[] args)
        {
            var data = new byte[] { 0xE0, 0x01, 0x03, 0x01, 0x00, 0x05 };
            var sPort = new System.IO.BinaryReader(new System.IO.MemoryStream(data));
            if (sPort.ReadByte() == JVSCodes.JVSSync)
            {
                var res = new JVSRes();
                res.nodeNum = (byte)sPort.ReadByte();
                byte dataLen = (byte)sPort.ReadByte();
                res.status = (JVSStatus)sPort.ReadByte();
                var buf = new byte[dataLen - 2];
                sPort.Read(buf, 0, buf.Length);
                res.data = buf;
                byte givenSum = (byte)sPort.ReadByte();
                byte calcSum = Checksum8.Calculate(res);
                if (givenSum == calcSum)
                {
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.WriteLine("INCORRECT {0} {1}", givenSum, calcSum);
                }
            }
            Console.ReadLine();
        }
    }

    public class JVSRes
    {
        public byte nodeNum;
        public JVSStatus status;
        public byte[] data;
    }

    public class JVSCodes
    {
        public const byte JVSSync = 0xE0;
        public const byte JVSMark = 0xD0;
    }

    public enum JVSStatus
    {
        Normal = 1,
        UnknownCommand = 2,
        ChecksumError = 3,
        AckOverflow = 4
    }

    public enum JVSReport
    {
        Normal = 1,
        IncorrectParams = 2,
        InvalidParams = 3,
        Busy = 4
    }
}
