using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace LifeStealClass.Content.Core
{
    public abstract class LifeStealItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var damageTooltip = tooltips.Find(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (damageTooltip != null)
            {
                damageTooltip.OverrideColor = new Color(180, 0, 0);
            }
        }
    }

    public class LifestealDamage : DamageClass
    {
        public override void SetDefaultStats(Player player)
        {
            player.GetCritChance<LifestealDamage>() = 2;
            player.GetArmorPenetration<LifestealDamage>() += 10;
        }
    }
}
