using Rocket.API;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Timers;


namespace VIPTestPlugin
{
    public class VIPTestPlugin : RocketPlugin<VIPTestConfig>
    {
        public static VIPTestPlugin Instance;
        public VIPUsageManager UsageManager;
        public VIPDataManager DataManager;
        private Timer webhookTimer;

        protected override void Load()
        {
            Instance = this;
            Logger.Log("✅ VIPTestPlugin yüklendi.");
            DataManager = new VIPDataManager();
            DataManager.Load();

            UsageManager = new VIPUsageManager();
            UsageManager.Load();

            webhookTimer = new Timer(1800000);
            webhookTimer.Elapsed += WebhookUpdate;
            webhookTimer.Start();

            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        protected override void Unload()
        {
            webhookTimer.Stop();
            webhookTimer.Dispose();
            DataManager.Save();
            UsageManager.Save();
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            DataManager.Save();
        }

        private void WebhookUpdate(object sender, ElapsedEventArgs e)
        {
            foreach (var kvp in new Dictionary<string, DateTime>(DataManager.ActiveVIPs))
            {
                string steamID = kvp.Key;
                DateTime expiration = kvp.Value;

                if (DateTime.Now >= expiration)
                {
                    SteamPlayer player = PlayerTool.getSteamPlayer(ulong.Parse(steamID));
                    string name = player?.playerID.characterName ?? steamID;

                    R.Commands.Execute(null, $"p remove \"{name}\" {Configuration.Instance.VIP_YetkiAdı}");
                    WebhookSender.SendSimpleMessage($"❌ VIP süresi sona erdi: {name} ({steamID})");
                    DataManager.ActiveVIPs.Remove(steamID);
                    DataManager.Save();
                }
                else
                {
                    TimeSpan kalan = expiration - DateTime.Now;
                    string kalanSure = $"{kalan.Days}g {kalan.Hours}s {kalan.Minutes}d {kalan.Seconds}s";
                    WebhookSender.SendSimpleMessage($"⏳ Kalan VIP süresi > SteamID: {steamID} | Süre: {kalanSure}");
                }
            }
        }
    }
}
