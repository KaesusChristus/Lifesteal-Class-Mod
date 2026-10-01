using Microsoft.Xna.Framework;
using System;

namespace HarvesterClassMod.Content.Projectiles.Weapon.Sickle
{
    public class TungstenScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "HarvesterClassMod/Content/Items/Weapons/Sickle/TungstenScythe";

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