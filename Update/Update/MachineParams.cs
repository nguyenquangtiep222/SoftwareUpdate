using Newtonsoft.Json;
using System;
using System.IO;

namespace Update
{
    public class MachineParams
    {
        private string _filePath = @$"{AppDomain.CurrentDomain.BaseDirectory}Update.json";
        private bool _autoRun = true;
        private string _appName = "Store.exe";
        private string _versionFile = "version.txt";
        private string _updateFile = "update.7z";

        public bool AutoRun
        {
            get => _autoRun;
            set => _autoRun = value;
        }

        public string AppName
        {
            get => _appName;
            set => _appName = value;
        }

        public string VersionFile
        {
            get => _versionFile;
            set => _versionFile = value;
        }

        public string UpdateFile
        {
            get => _updateFile;
            set => _updateFile = value;
        }

        public static MachineParams Current => __current;
        private static MachineParams __current = new MachineParams();
        private MachineParams() { }
        static MachineParams() { }

        public static void Reload()
        {
            MachineParams machineParams = new MachineParams();
            MachineParams loaded = machineParams.LoadFromFile();
            if (loaded != null)
            {
                __current = loaded;
            }
            else
            {
                machineParams.SaveToFile();
            }
        }

        public MachineParams LoadFromFile()
        {
            MachineParams machineParams = null;
            if (File.Exists(_filePath))
            {
                string contents = File.ReadAllText(_filePath);
                machineParams = JsonConvert.DeserializeObject<MachineParams>(contents);
            }
            return machineParams;
        }

        public void SaveToFile()
        {
            string contents = JsonConvert.SerializeObject(__current, Formatting.Indented);
            File.WriteAllText(_filePath, contents);
        }
    }
}
