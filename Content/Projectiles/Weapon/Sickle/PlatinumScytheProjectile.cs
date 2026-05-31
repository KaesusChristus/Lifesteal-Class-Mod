
using Microsoft.Xna.Framework;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class PlatinumScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/PlatinumScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.timeLeft = 60;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                SWINGRANGE = 2.0f * MathF.PI,
                SPINRANGE = 3.0f * MathF.PI,

                WINDUP = 0.3f,
                UNWIND = 0.55f,
                SPINTIME = 2.0f,

                PrepTime = 13f,
                ExecTime = 10f,
                HideTime = 13f,

                scale = 1.35f,
                hitboxWidth = 18f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }
    }
}
