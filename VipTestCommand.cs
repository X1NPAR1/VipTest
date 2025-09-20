using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace VIPTestPlugin
{
    public class VipTestCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "viptest";
        public string Help => "Bir kezlik VIP verir.";
        public string Syntax => "/viptest";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "viptest.kullan" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string steamID = player.CSteamID.ToString();

            if (VIPTestPlugin.Instance.UsageManager.HasUsed(steamID))
            {
                UnturnedChat.Say(player, "❌ Bu komutu yalnızca 1 kez kullanabilirsin.");
                return;
            }


            var config = VIPTestPlugin.Instance.Configuration.Instance;
            DateTime expiration = DateTime.Now
                .AddDays(config.Süre_Gün)
                .AddHours(config.Süre_Saat)
                .AddMinutes(config.Süre_Dakika);

            // Süreyi kaydet
            VIPTestPlugin.Instance.DataManager.ActiveVIPs[steamID] = expiration;

            // VIP yetkisini ver
            Rocket.Core.R.Commands.Execute(null, $"p add {player.CharacterName} {config.VIP_YetkiAdı}");

            // Discord’a gönder
            WebhookSender.SendVIPGrant(player.CharacterName, steamID, DateTime.Now, expiration);

            // Kullanım kaydı oluştur
            VIPTestPlugin.Instance.UsageManager.MarkUsed(steamID);

            // Bildirim gönder
            UnturnedChat.Say(player, $"🎁 {config.VIP_YetkiAdı} yetkisi {expiration} tarihine kadar verildi!");

            // Veriyi kaydet
            VIPTestPlugin.Instance.DataManager.Save();

        }
    }
}
