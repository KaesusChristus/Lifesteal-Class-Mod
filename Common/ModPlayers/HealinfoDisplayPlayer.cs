using LifeStealClass.Content;
using LifeStealClass.Content.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Common.ModPlayers
{
    public class HealinfoDisplayPlayer : ModPlayer
    {
        // Flag checking when information display should be activated
        public bool showHealInformation;

        // Make sure to use the right Reset hook. This one is unique, as it will still be
        // called when the game is paused; this allows for info accessories to keep updating properly.
        public override void ResetInfoAccessories()
        {
            showHealInformation = false;
        }

        // If we have another nearby player on our team, we want to get their info accessories working on us,
        // just like in vanilla. This is what this hook is for.
        public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
        {
            if (otherPlayer.GetModPlayer<HealinfoDisplayPlayer>().showHealInformation)
            {
                showHealInformation = true;
            }
        }
    }
}