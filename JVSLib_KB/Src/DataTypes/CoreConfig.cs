using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JVSLib_KB.Src.DataTypes
{
    class CoreConfig
    {
        public bool connectOnStart;
        public bool noReset;
        public bool doNotPrioritiseFormEvents;
        public int pollingRate = 100;

        FileStream file;

        public CoreConfig(string filename)
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
            file.Position = 0;
            var br = new BinaryReader(file);
            connectOnStart = br.ReadBoolean();
            noReset = br.ReadBoolean();
            doNotPrioritiseFormEvents = br.ReadBoolean();
            pollingRate = br.ReadInt32();
        }

        public void Save()
        {
            file.Position = 0;
            var bw = new BinaryWriter(file);
            bw.Write(connectOnStart);
            bw.Write(noReset);
            bw.Write(doNotPrioritiseFormEvents);
            bw.Write(pollingRate);
        }

        public void Close()
        {
            file.Close();
        }

        ~CoreConfig()
        {
            Close();
        }
    }
}
