
using Microsoft.Xna.Framework;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class LeadSickleProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/LeadSickle";

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

                WINDUP = 0.25f,
                UNWIND = 0.5f,
                SPINTIME = 2.0f,

                PrepTime = 13f,
                ExecTime = 13f,
                HideTime = 13f,

                scale = 1.25f,
                hitboxWidth = 18f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }
    }
}
