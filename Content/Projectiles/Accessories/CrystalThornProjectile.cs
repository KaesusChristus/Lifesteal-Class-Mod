using LifeStealClass.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Accessories
{
    public class CrystalThornProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 3;
            Projectile.height = 3;
            Projectile.scale = 1.2f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1f)
                return;

            float homingRange = 500f;
            float lerpSpeed = 0.1f;
            float desiredSpeed = 8f;

            NPC closestTarget = null;
            float closestDistance = homingRange;

            // Ziel suchen
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = npc;
                    }
                }
            }

            if (closestTarget == null)
            {
                Projectile.Kill();
                return;
            }

            Vector2 direction = closestTarget.Center - Projectile.Center;
            direction.Normalize();
            direction *= desiredSpeed;

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction, lerpSpeed);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

    }
}
