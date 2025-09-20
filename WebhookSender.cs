using Rocket.Core.Logging;
using System;
using System.Net;
using Newtonsoft.Json;

namespace VIPTestPlugin
{
    public static class WebhookSender
    {
        public static void SendVIPGrant(string name, string steamID, DateTime start, DateTime end)
        {
            string msg = $"📢 **✅ VIP Verildi**\n👤 Oyuncu: {name}\n🆔 SteamID: {steamID}\n📥 Başlangıç: {start:dd.MM.yyyy HH:mm:ss}\n📤 Bitiş: {end:dd.MM.yyyy HH:mm:ss}";
            SendSimpleMessage(msg);
        }

        public static void SendSimpleMessage(string content)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("Content-Type", "application/json");
                    var payload = new { content = content };
                    wc.UploadString(VIPTestPlugin.Instance.Configuration.Instance.DiscordWebhookURL, "POST", JsonConvert.SerializeObject(payload));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Webhook gönderim hatası: " + ex.Message);
            }
        }
    }
}
