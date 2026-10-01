using LifeStealClass.Common.ModPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Content
{
    public class HealinfoDisplayEffect : InfoDisplay
    {
        
        public static Color RedInfoTextColor => new(255, 19, 19, Main.mouseTextColor);

        public override string HoverTexture => Texture + "_Hover";

        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<HealinfoDisplayPlayer>().showHealInformation;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            LifestealEffectsPlayer lifestealPlayer = Main.LocalPlayer.GetModPlayer<LifestealEffectsPlayer>();

            float healingPerSecond = lifestealPlayer.GetHealAmountPerSecond();

            bool noHealing = healingPerSecond == 0.0f;
            if (noHealing)
            {
                displayColor = InactiveInfoTextColor;
            }
            else if (healingPerSecond < 10.0f)
            {
                displayColor = RedInfoTextColor;
            }

            return !noHealing ? $"{healingPerSecond:F1} Heal per Second" : "No Healing";
        }
    }
}
