
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
            Projectile.width = 46;
            Projectile.height = 44;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
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
