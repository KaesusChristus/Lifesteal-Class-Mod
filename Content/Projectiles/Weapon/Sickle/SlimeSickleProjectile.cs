using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class SlimeSickleProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/SlimeSickle";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 64;
            Projectile.height = 64;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                PrepTime = 16f,
                ExecTime = 12f,
                HideTime = 14f,

                scale = 1.1f,
                hitboxWidth = 18f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        public Color GetGlowColor()
        {
            return new Color(100, 180, 255); // hellblau, geisterhaft
        }
    }
}
