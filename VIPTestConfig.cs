using Rocket.API;

namespace VIPTestPlugin
{
    public class VIPTestConfig : IRocketPluginConfiguration
    {
        public int Süre_Gün;
        public int Süre_Saat;
        public int Süre_Dakika;
        public string VIP_YetkiAdı;
        public string DiscordWebhookURL;

        public void LoadDefaults()
        {
            Süre_Gün = 3;
            Süre_Saat = 0;
            Süre_Dakika = 0;
            VIP_YetkiAdı = "VIP";
            DiscordWebhookURL = "https://discord.com/api/webhooks/SENIN_WEBHOOK";
        }
    }
}
