using LifeStealClass.Common.Interfaces;
using LifeStealClass.Content.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public sealed class CrimsonScytheWaveProjectile : ModProjectile, INoLifestealProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BloodShot;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 7;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 90;
            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity *= 0.99f;
            Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 10f, 0f, 1f);
            Lighting.AddLight(Projectile.Center, new Vector3(0.75f, 0.04f, 0.08f));

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Blood,
                    Projectile.velocity.X * 0.08f,
                    Projectile.velocity.Y * 0.08f,
                    80,
                    new Color(235, 25, 40),
                    1.15f);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() * 0.5f;

            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float strength = 1f - i / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition,
                    null,
                    new Color(255, 30, 45, 0) * (0.45f * strength * Projectile.Opacity),
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale * (0.8f + strength * 0.2f),
                    SpriteEffects.None);
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                new Color(255, 170, 170, 0) * Projectile.Opacity,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None);
            return false;
        }
    }
}
