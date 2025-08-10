using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Common.ModPlayers;
using Terraria.ID;
using LifeStealClass.Common.Utils;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Common.GlobalItems.Effects
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

                LifestealHelper.MakeDust(target.position, target.width, target.height, DustID.LifeDrain);
            }
        }
    }
}