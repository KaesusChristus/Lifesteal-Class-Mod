using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using System;
using LifeStealClass.Common.ModPlayers;

namespace LifeStealClass.Content.Projectiles.Accessories
{
    public class ToothOfCthulhuProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.CritChance = 5; // 5%
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                // Initialisierung nach dem Spawnen
                Projectile.ai[0] = 1;
                Projectile.velocity = Vector2.Zero; // Projektil bewegt sich nicht zu Beginn

                // Create dust effect on spawn
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 1.5f);
                }

                // Rotate the projectile to face the target direction initially
                NPC target = Main.npc[(int)Projectile.ai[1]];
                Vector2 targetDirection = target.Center - Projectile.Center;
                targetDirection.Normalize();
                Projectile.rotation = targetDirection.ToRotation() + MathHelper.Pi; // Rotate by 180 degrees
            }
            else if (Projectile.ai[0] == 1)
            {
                // Warten, bevor der Dash beginnt
                if (Projectile.timeLeft <= 570)
                {
                    SoundEngine.PlaySound(SoundID.ForceRoarPitched, Projectile.position);
                    Projectile.ai[0] = 2; // Zustand ändern: Dash beginnt

                    // Gegnerposition erhalten
                    NPC target = Main.npc[(int)Projectile.ai[1]];
                    Vector2 targetPosition = target.Center; // Gegnerzentrum verwenden

                    // Richtung und Geschwindigkeit des Dashes setzen
                    Vector2 targetDirection = targetPosition - Projectile.Center;
                    targetDirection.Normalize();
                    Projectile.velocity = targetDirection * 20f; // Beispielhafte Geschwindigkeit

                    // Rotate the projectile to face the target
                    Projectile.rotation = targetDirection.ToRotation() + MathHelper.Pi; // Rotate by 180 degrees
                }
            }

            if (Projectile.ai[0] == 2)
            {
                // After-image effect
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }

            Projectile.tileCollide = false;

            if (Projectile.timeLeft <= 530)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha >= 255)
                {
                    // Create dust effect on despawn
                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 1.5f);
                    }
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            // Heal the player
            if (hit.Crit)
            {
                Player player = Main.player[Projectile.owner];

                player.GetModPlayer<LifestealEffectsPlayer>().SetHealAmount(damageDone / 4);
            }
            else
            {
                Player player = Main.player[Projectile.owner];

                player.GetModPlayer<LifestealEffectsPlayer>().SetHealAmount(3);
            }


        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}
