using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Common.ModPlayers
{
    public class HealthIncreaseOverrideRule : ModPlayer
    {
        public bool doOverride = false;

        public override void PostUpdateEquips()
        {
            if (Player.GetModPlayer<DarkManaSoulEffect>().hasDarkManaSoulEquipped == true)
            {
                doOverride = true;
            }

            // Hier mehr hinzufügen

            else
            {
                doOverride = false;
            }
        }
    }
}
