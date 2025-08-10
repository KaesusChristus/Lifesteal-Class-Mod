using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Common.AIStyles
{
    public class XStrikeAI : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.aiStyle == CustomAiStyleID.XStrike;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.aiStyle != CustomAiStyleID.XStrike)
                return;

            if (projectile.ai[0] == 1f)
                return;

            var source = projectile.GetSource_FromThis();
            Vector2 spawnOffset = new Vector2(110f, -130f);

            for (int i = -1; i <= 1; i += 2)
            {
                float projSpeed = 20f;

                Vector2 spawnPos = target.Center + new Vector2(spawnOffset.X * i, spawnOffset.Y);
                Vector2 velocity = (target.Center - spawnPos).SafeNormalize(Vector2.UnitY) * projSpeed;

                int newProj = Projectile.NewProjectile(source, spawnPos, velocity, projectile.type, projectile.damage / 3, 1f, projectile.owner);
                Main.projectile[newProj].ai[0] = 1f;
                Main.projectile[newProj].timeLeft = 20;
                Main.projectile[newProj].penetrate = -1;
                Main.projectile[newProj].tileCollide = false;
            }
        }

    }
}
