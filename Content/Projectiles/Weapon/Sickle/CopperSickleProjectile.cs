
using Microsoft.Xna.Framework;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class CopperSickleProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/CopperSickle";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 42;
            Projectile.height = 42;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                SWINGRANGE = 2.0f * MathF.PI,
                SPINRANGE = 3.5f * MathF.PI,

                WINDUP = 0.3f,
                UNWIND = 0.55f,
                SPINTIME = 2.0f,

                PrepTime = 16f,
                ExecTime = 17f,
                HideTime = 14f,

                scale = 1.1f,
                hitboxWidth = 18f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }
    }
}
