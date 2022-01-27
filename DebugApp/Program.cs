using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugApp
{
    static class Program
    {
        static int numPlayers = 2;
        static int numSwitches = 13;

        static int dataBytes;
        static byte[] data;

        static void Main(string[] args)
        {
            data = new byte[numPlayers * numSwitches];
            data[1] = 0x1;
            dataBytes = (byte)Math.Ceiling((decimal)numSwitches / 8);
            for (byte p = 0; p < numPlayers; p++)
            {
                for (byte s = 0; s < numSwitches; s++)
                {
                    int currByte = (p * dataBytes) + (s / 8);
                    int currBit  = s % 8;
                    currBit = 7 - currBit;
                    Console.WriteLine("       p: {0}", p);
                    Console.WriteLine("       s: {0}", s);
                    Console.WriteLine("currByte: {0}", currByte);
                    Console.WriteLine(" currBit: {0}", currBit);
                    Console.WriteLine("   state: {0}", data[currByte].GetBit(currBit));
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }
    }
}
