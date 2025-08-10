using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;

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
            return clone;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (baseHealOnHit > 0 && item.DamageType == ModContent.GetInstance<LifestealDamage>())
            {
                var modPlayer = player.GetModPlayer<LifestealEffectsPlayer>();
                int totalHeal = baseHealOnHit + bonusHealOnHit;

                modPlayer.SetHealAmount(totalHeal);
            }
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (baseHealOnHit > 0)
            {
                int total = baseHealOnHit + bonusHealOnHit;

                var line = new TooltipLine(Mod, "OnHitHeal", $"Baseheal: {total}")
                {
                    OverrideColor = new Color(0, 200, 0)
                };

                tooltips.Add(line);
            }
        }

    }
}
