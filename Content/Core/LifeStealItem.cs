using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Core
{
    public abstract class LifeStealItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<HarvesterDamage>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine damageTooltip = tooltips.Find(line =>
                line.Name == "Damage" && line.Mod == "Terraria");

            if (damageTooltip != null)
            {
                damageTooltip.OverrideColor = new Color(180, 0, 0);
            }
        }

        public override bool ReforgePrice(ref int reforgePrice, ref bool canApplyDiscount)
        {
            // Most Harvester items declare a sell value. Terraria stores that as five
            // times the displayed amount, which otherwise makes reforging excessive.
            reforgePrice = System.Math.Max(1, reforgePrice / 5);
            return true;
        }
    }
}
