using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Common.AIStyles
{
    public class HomingDelayedAI : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.aiStyle == CustomAiStyleID.HomingDelayed;
        }

        public override void AI(Projectile projectile)
        {
            projectile.rotation = projectile.velocity.ToRotation();

            if (projectile.ai[0] == 1)
            {
                projectile.tileCollide = false;
                projectile.ai[1]++;

                if (projectile.ai[1] < 30f)
                {
                    projectile.velocity *= 1.02f;
                    return;
                }

                float homingSpeed = projectile.localAI[0];

                // Ziel finden
                NPC target = null;
                float detectRange = 1000f;
                float shortest = detectRange;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile))
                    {
                        float dist = Vector2.Distance(projectile.Center, npc.Center);
                        if (dist < shortest)
                        {
                            shortest = dist;
                            target = npc;
                        }
                    }
                }

                if (target != null)
                {
                    projectile.tileCollide = true;
                    Vector2 move = (target.Center - projectile.Center).SafeNormalize(Vector2.Zero);
                    projectile.velocity = Vector2.Lerp(projectile.velocity, move * homingSpeed, 0.1f);
                }
                else if (projectile.ai[1] >= 60f)
                {
                    projectile.tileCollide = true;
                    projectile.Kill();
                }
            }
        }



        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (projectile.aiStyle == CustomAiStyleID.HomingDelayed &&
                projectile.ai[0] == 1 && projectile.ai[1] < 30f)
            {
                return false;
            }

            return base.CanHitNPC(projectile, target);
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.aiStyle != CustomAiStyleID.HomingDelayed)
                return;

            if (projectile.ai[0] == 0 || projectile.localAI[1] > 0) // Nur Ursprung oder wenn Splits erlaubt sind
            {
                int splits = (int)projectile.localAI[1];
                float homingSpeed = projectile.localAI[0];
                var source = projectile.GetSource_FromThis();

                for (int i = 0; i < splits; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                    int newProj = Projectile.NewProjectile(source, projectile.Center, velocity, projectile.type, projectile.damage / 2, 1f, projectile.owner, 1f, 0f);

                    Main.projectile[newProj].localAI[0] = homingSpeed;
                    Main.projectile[newProj].localAI[1] = 0f; // Diese dürfen sich **nicht mehr splitten**
                }
            }
        }

    }
}
