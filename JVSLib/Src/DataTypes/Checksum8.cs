using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JVSLib.Src.DataTypes
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
}
