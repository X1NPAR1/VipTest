using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace VIPTestPlugin
{
    public class VIPDataManager
    {
        public Dictionary<string, DateTime> ActiveVIPs = new Dictionary<string, DateTime>();

        private string FilePath =>
            Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "VIPTestPlugin", "vip_data.json");

        public void Load()
        {
            if (File.Exists(FilePath))
                ActiveVIPs = JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(ActiveVIPs, Formatting.Indented));
        }
    }
}
