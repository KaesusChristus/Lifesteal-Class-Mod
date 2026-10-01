using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Common.ModPlayers
{
    public class DarkBloodCrystalEffect : ModPlayer
    {
        public bool hasDarkBloodCrystalEquipped = false;

        public override void ResetEffects()
        {
            hasDarkBloodCrystalEquipped = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasDarkBloodCrystalEquipped)
            {
                Player.GetModPlayer<LifestealEffectsPlayer>().BonusHealAmountAdd(2);
            }
        }
    }
}

