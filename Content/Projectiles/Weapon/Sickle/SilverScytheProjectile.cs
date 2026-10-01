
using Microsoft.Xna.Framework;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class SilverScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/SilverScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();
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

                PrepTime = 16f,
                ExecTime = 12f,
                HideTime = 14f,

                scale = 1.35f,
                hitboxWidth = 18f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }
    }
}
