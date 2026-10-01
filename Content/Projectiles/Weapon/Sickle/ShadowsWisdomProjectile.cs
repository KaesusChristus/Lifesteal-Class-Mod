using System;
using LifeStealClass.Content.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class ShadowsWisdomProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/ShadowsWisdom";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 54;
            Projectile.height = 46;
            Projectile.scale = 1.2f;
            Projectile.timeLeft = 60;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                PrepTime = 12f,
                ExecTime = 8f,
                HideTime = 12f,
                scale = 1.35f,
                hitboxWidth = 15f,
                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        protected override void OnAttackStarted()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 direction = GetAttackDirection();
            float damageMultiplier = CurrentAttack == SickleAttackType.HeavySlash ? 0.42f : 0.3f;

            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 velocity = direction.RotatedBy(MathHelper.ToRadians(12f * i)) * 7f;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Owner.MountedCenter + direction * 24f,
                    velocity,
                    ModContent.ProjectileType<DemoniteSoulProjectile>(),
                    Math.Max(1, (int)(Projectile.damage * damageMultiplier)),
                    Projectile.knockBack * 0.45f,
                    Projectile.owner);
            }
        }
    }
}
