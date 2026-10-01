
using Microsoft.Xna.Framework;
using System;
using LifeStealClass.Content.Items.Weapons.Sickle;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class PlatinumScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/PlatinumScythe";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.timeLeft = 60;
            Projectile.width = 48;
            Projectile.height = 46;
        }

        public override SickleStats GetStats()
        {
            return new SickleStats
            {
                PrepTime = 13f,
                ExecTime = 10f,
                HideTime = 13f,

                scale = 1.35f,
                hitboxWidth = 18f,

                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        protected override void OnAttackStarted()
        {
            SpawnSpectralEcho(ModContent.ItemType<PlatinumScythe>(), 1);
        }
    }
}
