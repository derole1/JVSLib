using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JVSLib_BG4FFB.Src;

namespace JVSLib_BG4FFB_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ffb = new FFBConnection();
            Console.Write("Enter a polling rate (Default: 10ms): ");
            var pollingRate = Console.ReadLine().ToLower().Replace("ms", "");
            if (pollingRate.Length > 0 && int.TryParse(pollingRate, out _))
            {
                ffb.wait = int.Parse(pollingRate);
                Console.Write("Polling rate updated to {0}ms", ffb.wait);
            }
            ffb.Connect();
            while (ffb.connected)
            {
                Console.Write("Enter an FFB command: ");
                var cmd = StringToByteArray(Console.ReadLine());
                if (cmd.Length != 2)
                {
                    Console.WriteLine("Input is incorrect!");
                    continue;
                }
                Console.WriteLine("            Response: {0}", BitConverter.ToString(ffb.CustomCommand(cmd[0], cmd[1])).Replace("-", ""));
            }
            Console.WriteLine("Connection to FFB board was lost");
            Console.ReadLine();
        }

        public static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("-", "").Replace(":", "").Replace("0x", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
