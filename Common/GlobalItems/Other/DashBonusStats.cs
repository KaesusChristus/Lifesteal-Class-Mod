using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LifeStealClass.Common.GlobalItems.Other
{
    public class DashBonusStats : GlobalItem
    {
        public int dashDamageBonus = 0;
        public int dashCritBonus = 0;

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            var clone = (DashBonusStats)base.Clone(item, itemClone);
            clone.dashDamageBonus = dashDamageBonus;
            clone.dashCritBonus = dashCritBonus;
            return clone;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (dashDamageBonus > 0)
            {
                var line = new TooltipLine(Mod, "DashDamageBonus", $"Dash bonus damage: +{dashDamageBonus}")
                {
                    OverrideColor = new Color(255, 150, 50)
                };
                tooltips.Add(line);
            }

            if (dashCritBonus > 0)
            {
                var line = new TooltipLine(Mod, "DashCritBonus", $"Dash bonus crit: +{dashCritBonus}%")
                {
                    OverrideColor = new Color(255, 150, 50)
                };
                tooltips.Add(line);
            }

            // Basis Dash Cooldown
            if (item.ModItem is IDashWeapon dashItem)
            {
                int seconds = dashItem.DashCooldown / 60;
                var line = new TooltipLine(Mod, "DashCooldown", $"Dash cooldown: {seconds} seconds")
                {
                    OverrideColor = new Color(50, 200, 50)
                };
                tooltips.Add(line);
            }
        }
    }
}
