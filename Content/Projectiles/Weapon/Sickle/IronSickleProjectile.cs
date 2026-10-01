
using Microsoft.Xna.Framework;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class IronSickleProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/IronSickle";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 42;
            Projectile.height = 34;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
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
