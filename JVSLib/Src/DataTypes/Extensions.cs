using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace JVSLib.Src.DataTypes
{
    public static class Extensions
    {
        public static byte[] ToArray(this Stream input)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                input.CopyTo(ms);
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        public static byte SwapBits(this byte b)
        {
            byte result = 0x00;
            for (byte mask = 0x80; Convert.ToInt32(mask) > 0; mask >>= 1)
            {
                result = (byte)(result >> 1);
                var tempbyte = (byte)(b & mask);
                if (tempbyte != 0x00)
                {
                    result = (byte)(result | 0x80);
                }
            }
            return result;
        }

        public static short SwapBits(this short value)
        {
            return (short)((value << 8) + ((ushort)value >> 8));
        }

        public static short LeftShift(this short b, short amount)
        {
            return (short)(b << amount);
        }

        public static short AndMask(this short b, short mask)
        {
            return (short)(b & mask);
        }
        
        public static byte EscapedReadByte(this SerialPort sPort)
        {
            byte b = (byte)sPort.ReadByte();
            if (b == JVSCodes.JVSMark)
            {
                b = (byte)(sPort.ReadByte() + 1);
            }
            return b;
        }

        //public static int EscapedRead(this SerialPort sPort, ref byte[] buffer, int offset, int count)
        //{
        //    int read = sPort.Read(buffer, offset, count);
        //    var data = buffer.ToList();
        //    for (int i = 1; i < data.Count; i++)
        //    {
        //        if (data[i] == JVSCodes.JVSMark)
        //        {
        //            Console.WriteLine("Found a MARK!");
        //            data[i + 1] += 1;
        //            data.RemoveAt(i);
        //        }
        //    }
        //    buffer = data.ToArray();
        //    return read;
        //}

        public static int EscapedRead(this SerialPort sPort, ref byte[] buffer, int offset, int count)
        {
            var lst = new List<byte>();
            for (int i=0; i<count; i++)
            {
                lst.Add(sPort.EscapedReadByte());
            }
            buffer = lst.ToArray();
            return count;
        }

        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static int ConvertRange(this int value, int originalStart, int originalEnd,  int newStart, int newEnd)
        {
            double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            return (int)(newStart + ((value - originalStart) * scale));
        }

        public static ushort ConvertRange(this ushort value, int originalStart, int originalEnd, int newStart, int newEnd)
        {
            double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            return (ushort)(newStart + ((value - originalStart) * scale));
        }
    }
}
