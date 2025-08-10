using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Common.ModPlayers
{
    public class DarkManaSoulEffect : ModPlayer
    {
        public bool hasDarkManaSoulEquipped = false;

        public override void ResetEffects()
        {
            hasDarkManaSoulEquipped = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasDarkManaSoulEquipped)
            {
                Player.statLifeMax2 += Player.statManaMax2;
            }
        }
    }
}
