using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using LifeStealClass.Common.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Common.GlobalItems.Other
{
    public class HealthCost : GlobalItem
    {
        public int healthCost = 0;

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            HealthCost clone = (HealthCost)base.Clone(item, itemClone);
            clone.healthCost = healthCost;
            return clone;
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (item.shoot > ProjectileID.None && item.noMelee)
            {
                if (healthCost > 0)
                {
                    int reduction = player.GetModPlayer<LifestealEffectsPlayer>().reduceLifecostFlat;
                    int adjustedCost = Math.Max(0, healthCost - reduction);

                    if (player.statLife > adjustedCost)
                    {
                        player.statLife -= adjustedCost;

                        if (Main.netMode != NetmodeID.Server)
                            CombatText.NewText(player.getRect(), Color.Red, $"-{adjustedCost}");
                    }
                    else
                    {
                        return false; // Nicht genug Leben
                    }
                }
            }

            return base.UseItem(item, player);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (healthCost > 0)
            {
                int reduction = player.GetModPlayer<LifestealEffectsPlayer>().reduceLifecostFlat;
                int adjustedCost = Math.Max(0, healthCost - reduction);

                if (player.statLife > adjustedCost)
                {
                    player.statLife -= adjustedCost;

                    if (Main.netMode != NetmodeID.Server)
                    {
                        CombatText.NewText(player.getRect(), Color.Red, $"-{adjustedCost}");
                    }
                }
            }
        }


        public override bool CanUseItem(Item item, Player player)
        {
            int reduction = 0;
            if (player.TryGetModPlayer(out LifestealEffectsPlayer modPlayer))
                reduction = modPlayer.reduceLifecostFlat;

            int adjustedCost = Math.Max(0, healthCost - reduction);

            if (adjustedCost > 0 && player.statLife <= adjustedCost)
            {
                return false; // Nicht genug Leben
            }

            return base.CanUseItem(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var global = item.GetGlobalItem<HealthCost>();
            if (global.healthCost > 0)
            {
                int reduction = 0;
                if (Main.LocalPlayer != null && Main.LocalPlayer.TryGetModPlayer(out LifestealEffectsPlayer modPlayer))
                    reduction = modPlayer.reduceLifecostFlat;

                int displayCost = Math.Max(0, global.healthCost - reduction);

                var line = new TooltipLine(Mod, "HealthCost", $"Uses {displayCost} health")
                {
                    OverrideColor = new Color(180, 0, 0)
                };

                tooltips.Add(line);
            }
        }
    }
}