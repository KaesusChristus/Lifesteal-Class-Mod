using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.ID;
using LifeStealClass.Common.Utils;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Projectiles.Weapon.FractaliteStaffProjectiles
{
    public class VampiricLeafFractaliteStaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.scale = 4f;
            Projectile.alpha = 100;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = ModContent.GetInstance<LifestealDamage>();
            Projectile.timeLeft = 300;
            Projectile.velocity *= 30f;
            Projectile.ArmorPenetration = 10;

            Projectile.aiStyle = CustomAiStyleID.HomingDelayed;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Alphawert setzen
            Color drawColor = lightColor * ((255 - Projectile.alpha) / 255f);

            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            float rotation = Projectile.rotation;
            SpriteEffects effects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;


            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, drawColor, rotation, origin, Projectile.scale, effects, 0);

            return false; // wir haben selbst gezeichnet
        }

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass);
                Main.dust[dustIndex].noGravity = false;
                Main.dust[dustIndex].scale = 0.8f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
            Vector2 direction = Projectile.velocity * -1;
            direction.Normalize();

            float speed = 2f;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * speed, ProjectileID.BladeOfGrass, Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
        }

    }
}
