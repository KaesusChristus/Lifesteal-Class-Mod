using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using LifeStealClass.Common.Interfaces;
using LifeStealClass.Content.Prefixes;
using LifeStealClass.Common.ModPlayers;

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
            int displayDamageBonus = WarScythePrefixPool.GetDashDamageBonus(item, dashDamageBonus);
            int displayCritBonus = WarScythePrefixPool.GetDashCritBonus(item, dashCritBonus);

            if (item.ModItem is IDashWeapon dashItem)
            {
                if (displayDamageBonus > 0 || displayCritBonus > 0)
                {
                    var impactLine = new TooltipLine(
                        Mod,
                        "ChargeImpact",
                        $"Charge impact: +{displayDamageBonus} damage, +{displayCritBonus}% crit")
                    {
                        OverrideColor = new Color(255, 150, 50)
                    };
                    tooltips.Add(impactLine);
                }

                int cooldown = WarScythePrefixPool.GetDashCooldown(item, dashItem.DashCooldown);
                float seconds = cooldown / 60f;
                int healthCost = WarScythePrefixPool.GetDashHealthCost(
                    item,
                    item.GetGlobalItem<HealthCost>().dashHealthCost);
                int reduction = 0;
                if (Main.LocalPlayer != null
                    && Main.LocalPlayer.TryGetModPlayer(out LifestealEffectsPlayer modPlayer))
                {
                    reduction = modPlayer.reduceLifecostFlat;
                }

                int displayHealthCost = System.Math.Max(0, healthCost - reduction);
                var utilityLine = new TooltipLine(
                    Mod,
                    "ChargeUtility",
                    $"Charge utility: {seconds:0.##}s cooldown, {displayHealthCost} health")
                {
                    OverrideColor = new Color(50, 200, 50)
                };
                tooltips.Add(utilityLine);
            }
        }
    }
}
