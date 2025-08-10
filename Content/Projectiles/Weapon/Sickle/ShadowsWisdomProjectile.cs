using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Core;
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

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, new Color(140, 27, 130), 1.8f);
                dust.noGravity = true;
            }

            int count = 5;
            float speed = 7f;
            float offsetDistance = 180f;

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.ToRadians(360f / count * i);
                Vector2 direction = angle.ToRotationVector2();
                Vector2 spawnPos = Projectile.Center + direction * offsetDistance;
                Vector2 velocity = (direction * speed) * -0.75f;

                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPos, velocity, ProjectileID.LightsBane, Projectile.damage / 2, Projectile.knockBack, Projectile.owner, 1f);
                proj.DamageType = ModContent.GetInstance<LifestealDamage>();
            }
        }

        public override Color GetGlowColor()
        {
            return new Color(140, 27, 130);
        }
    }
}
