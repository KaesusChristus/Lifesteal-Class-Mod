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
    }
}
