using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace VIPTestPlugin
{
    public class VIPUsageManager
    {
        public HashSet<string> UsedVIPs = new HashSet<string>();

        private string FilePath =>
            Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "VIPTestPlugin", "vip_used.json");

        public void Load()
        {
            if (File.Exists(FilePath))
            {
                var list = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(FilePath));
                UsedVIPs = new HashSet<string>(list);
            }
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(UsedVIPs, Formatting.Indented));
        }

        public bool HasUsed(string steamID)
        {
            return UsedVIPs.Contains(steamID);
        }

        public void MarkUsed(string steamID)
        {
            UsedVIPs.Add(steamID);
            Save();
        }
    }
}
