using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Core;
using Terraria.ModLoader;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class TheSlaughtersScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/TheSlaughtersScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 48;
            Projectile.height = 38;
            Projectile.scale = 1.2f;

            Projectile.timeLeft = 50;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                SWINGRANGE = 1.67f * MathF.PI,
                SPINRANGE = 3.5f * MathF.PI,

                WINDUP = 0.15f,
                UNWIND = 0.4f,
                SPINTIME = 2.5f,

                PrepTime = 12f,
                ExecTime = 8f,
                HideTime = 12f,

                scale = 1.35f,
                hitboxWidth = 15f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        /*

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, new Color(140, 27, 130), 1.8f);
                dust.noGravity = true;
            }

            int count = 5;
            float speed = 4f;

            float startAngle = MathHelper.ToRadians(110f);
            float endAngle = MathHelper.ToRadians(70f);
            float angleStep = (endAngle - startAngle) / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 direction = angle.ToRotationVector2();
                Vector2 spawnPos = Projectile.Center + direction;
                Vector2 velocity = (direction * speed) * 0.75f;

                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPos, velocity, ProjectileID.BloodShot, Projectile.damage, Projectile.knockBack, Projectile.owner, 1f);
                proj.DamageType = ModContent.GetInstance<HarvesterDamage>();
                proj.friendly = true;
                proj.hostile = false;
                proj.scale = 0.8f;
            }
        }

        */

        public Color GetGlowColor()
        {
            return new Color(255, 40, 40);
        }
    }
}
