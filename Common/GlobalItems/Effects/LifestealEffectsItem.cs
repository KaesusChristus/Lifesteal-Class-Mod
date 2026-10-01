using Terraria;
using Terraria.ModLoader;
using HarvesterClassMod.Common.ModPlayers;
using Terraria.ID;
using HarvesterClassMod.Common.Utils;
using HarvesterClassMod.Content.Core;

namespace HarvesterClassMod.Common.GlobalItems.Effects
{
    public class LifestealEffectsItem : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.ModItem is LifeStealItem)
            {
                bool crit = hit.Crit;

                player.GetModPlayer<LifestealEffectsPlayer>().AddDamage(damageDone);
                player.GetModPlayer<LifestealEffectsPlayer>().IsCrit(crit);

                if (crit)
                {
                    LifestealHelper.MakeDust(target.position, target.width, target.height, DustID.LifeDrain);
                }
            }
        }
    }
}