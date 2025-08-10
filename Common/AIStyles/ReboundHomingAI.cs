using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Common.AIStyles
{
    public class ReboundHomingAI : GlobalProjectile
    {
        private const float HomingRange = 500f;
        private const float HomingSpeed = 10f;
        private const float HomingLerp = 0.25f;

        // Geschwindigkeit, mit der sich die neuen Projektile kurz vom Ziel entfernen
        private const float SpawnPushSpeed = 9f;
        // Zeit (Ticks), wie lange sich die neuen Projektile vom Ziel entfernen bevor sie homing aktivieren
        private const int SpawnPushDuration = 20;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.aiStyle == CustomAiStyleID.ReboundHoming;
        }

        public override void SetDefaults(Projectile entity)
        {
            entity.penetrate = -1;
        }

        public override void AI(Projectile projectile)
        {
            Player owner = Main.player[projectile.owner];

            int state = (int)projectile.ai[0];
            int timer = (int)projectile.ai[1];

            NPC target = FindClosestNPC(projectile.Center, HomingRange);

            switch (state)
            {
                case 0: // Initialflug geradeaus
                    if (target != null)
                    {
                        projectile.ai[0] = 1; // Homing aktivieren
                    }
                    break;

                case 1: // Homing zum ersten Treffer
                    if (target != null && target.active)
                    {
                        Vector2 direction = (target.Center - projectile.Center).SafeNormalize(Vector2.UnitX);
                        projectile.velocity = Vector2.Lerp(projectile.velocity, direction * HomingSpeed, HomingLerp);
                    }
                    break;

                case 2: // Weiterflug nach dem ersten Treffer (Originalprojektil)
                    timer++;
                    projectile.ai[1] = timer;
                    projectile.tileCollide = false;

                    if (timer >= 15) // nach 15 Ticks erneutes Homing aktivieren
                    {
                        projectile.ai[0] = 3;
                        projectile.ai[1] = 0;
                    }
                    break;

                case 3: // Zweites Homing (Originalprojektil und gespawnte Projektile)
                    if (target != null && target.active)
                    {
                        Vector2 direction = (target.Center - projectile.Center).SafeNormalize(Vector2.UnitX);
                        projectile.velocity = Vector2.Lerp(projectile.velocity, direction * HomingSpeed, HomingLerp);
                    }
                    else
                    {
                        // Kein Ziel gefunden im zweiten Homing -> Projektil zerstören
                        projectile.Kill();
                    }
                    break;

                case 4: // Zustand für die gespawnten Projektile: entfernen sich kurz vom Ziel
                    projectile.tileCollide = false;
                    timer++;
                    projectile.ai[1] = timer;

                    if (timer >= SpawnPushDuration)
                    {
                        projectile.ai[0] = 3; // dann starten sie das Homing (wie state 3)
                        projectile.ai[1] = 0;
                    }
                    break;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.aiStyle != CustomAiStyleID.ReboundHoming)
                return;

            if (projectile.ai[0] == 1) // Erster Treffer des Originalprojektils
            {
                // 1. Wechsel in Nach-dem-Treffer-Modus für das Originalprojektil
                projectile.ai[0] = 2;
                projectile.ai[1] = 0;
                projectile.penetrate = 2;

                // 2. Spawn neuer Projektile rund ums getroffene Ziel
                SpawnProjectilesAroundTarget(projectile, target);
            }
            else if (projectile.ai[0] == 3)
            {
                // Zweiter Treffer: Projektile sterben
                projectile.Kill();
            }
        }

        private void SpawnProjectilesAroundTarget(Projectile parentProjectile, NPC target)
        {
            Player owner = Main.player[parentProjectile.owner];

            int spawnCount = (int)parentProjectile.localAI[1];
            for (int i = 0; i < spawnCount; i++)
            {
                // Zufälliger Winkel für das neue Projektil
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                // Position leicht um das Ziel herum
                Vector2 spawnPos = target.Center + new Vector2(20f, 0).RotatedBy(angle);

                // Neues Projektil spawnen (gleiches Type wie das Original)
                int proj = Projectile.NewProjectile(
                    parentProjectile.GetSource_FromThis(),
                    spawnPos,
                    Vector2.Zero,
                    parentProjectile.type,
                    parentProjectile.damage,
                    parentProjectile.knockBack,
                    parentProjectile.owner);

                Projectile spawnedProj = Main.projectile[proj];

                // Zustand 4: "kurz entfernen vom Ziel"
                spawnedProj.ai[0] = 4;
                spawnedProj.ai[1] = 0;

                // Zufällige Richtung weg vom Ziel für Anfangsgeschwindigkeit
                Vector2 awayDir = (spawnedProj.Center - target.Center).SafeNormalize(Vector2.UnitX);
                float randomAngle = Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);
                awayDir = awayDir.RotatedBy(randomAngle);

                spawnedProj.velocity = awayDir * SpawnPushSpeed;

                // Penetrate auf 1 setzen, damit sie nur einmal treffen (optional, je nach Wunsch)
                spawnedProj.penetrate = 1;
            }
        }

        private NPC FindClosestNPC(Vector2 position, float range)
        {
            NPC closest = null;
            float minDist = range;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && !npc.friendly && !npc.dontTakeDamage)
                {
                    float dist = Vector2.Distance(position, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = npc;
                    }
                }
            }

            return closest;
        }

        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            int state = (int)projectile.ai[0];
            if (state == 1 || state == 3)
                return true;
            return false;
        }

    }
}
