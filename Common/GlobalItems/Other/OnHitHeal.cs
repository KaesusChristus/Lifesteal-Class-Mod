using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Prefixes;

namespace LifeStealClass.Common.GlobalItems.Other
{
    public class OnHitHeal : GlobalItem
    {
        public int baseHealOnHit = 0;
        public int bonusHealOnHit = 0;

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            var clone = (OnHitHeal)base.Clone(item, itemClone);
            clone.baseHealOnHit = baseHealOnHit;
            clone.bonusHealOnHit = bonusHealOnHit;
            return clone;
        }

        public int GetTotalHeal(Item item)
        {
            return baseHealOnHit
                + bonusHealOnHit
                + ScythePrefixPool.GetHealModifier(item);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (baseHealOnHit != 0 && item.DamageType == ModContent.GetInstance<HarvesterDamage>())
            {
                var modPlayer = player.GetModPlayer<LifestealEffectsPlayer>();
                int totalHeal = GetTotalHeal(item);

                if (totalHeal != 0)
                {
                    modPlayer.SetHealAmount(totalHeal);
                }
            }
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (baseHealOnHit != 0)
            {
                int total = GetTotalHeal(item);

                var line = new TooltipLine(Mod, "OnHitHeal", $"Heal: {total}")
                {
                    OverrideColor = total > 0
                        ? new Color(0, 200, 0)
                        : total < 0
                            ? new Color(220, 70, 70)
                            : Color.Gray
                };

                tooltips.Add(line);
            }
        }

    }
}
