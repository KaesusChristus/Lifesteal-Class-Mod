using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Common.Utils;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Common.GlobalProjectiles
{
    public class LifestealEffectsProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            if (player.HeldItem.ModItem is LifeStealItem)
            {
                bool crit = hit.Crit;
                player.GetModPlayer<LifestealEffectsPlayer>().AddDamage(damageDone);
                player.GetModPlayer<LifestealEffectsPlayer>().IsCrit(crit);
                LifestealHelper.MakeDust(target.position, target.width, target.height, DustID.LifeDrain);
            }
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            if (player.HeldItem.ModItem is LifeStealItem && Main.rand.NextBool(5))
            {
                LifestealHelper.MakeDust(projectile.position, projectile.width, projectile.height, DustID.Blood);
            }
        }
    }
}
