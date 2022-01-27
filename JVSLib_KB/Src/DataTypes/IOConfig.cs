using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace JVSLib_KB.Src.DataTypes
{
    class IOConfig
    {
        public class JVSSwitchInput
        {
            public byte player;
            public byte switchInp;
        }

        public Dictionary<Tuple<byte,byte>, Keys> switchBindings = new Dictionary<Tuple<byte, byte>, Keys>();

        FileStream file;

        public IOConfig(string filename)
        {
            if (Path.GetDirectoryName(filename).Length > 0)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }
            }
            try
            {
                file = File.Open(filename, FileMode.Open);
                Load();
            }
            catch
            {
                if (file != null) { file.Close(); }
                file = File.Open(filename, FileMode.Create);
                Save();
            }
        }

        public void Load()
        {
            switchBindings.Clear();
            file.Position = 0;
            var br = new BinaryReader(file);
            int count = br.ReadInt32();
            for (int i=0; i<count; i++)
            {
                switchBindings.Add(new Tuple<byte, byte>(br.ReadByte(), br.ReadByte()), (Keys)br.ReadInt32());
            }
        }

        public void Save()
        {
            file.Position = 0;
            var bw = new BinaryWriter(file);
            bw.Write(switchBindings.Count);
            foreach (var binding in switchBindings)
            {
                bw.Write(binding.Key.Item1);
                bw.Write(binding.Key.Item2);
                bw.Write((int)binding.Value);
            }
        }

        public void Close()
        {
            file.Close();
        }

        ~IOConfig()
        {
            Close();
        }
    }
}
