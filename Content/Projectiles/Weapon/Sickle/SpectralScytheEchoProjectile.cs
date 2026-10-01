using LifeStealClass.Content.Core;
using LifeStealClass.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public sealed class SpectralScytheEchoProjectile : ModProjectile, INoLifestealProjectile
    {
        private int ItemType => (int)Projectile.ai[0];
        private bool IsPlatinum => Projectile.ai[1] == 1f;
        private bool IsHeavy => Projectile.ai[2] == 1f;

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.WoodenArrowFriendly;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 30;
            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = IsHeavy ? 1.25f : 1f;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation()
                + (Projectile.spriteDirection > 0
                    ? MathHelper.PiOver4
                    : MathHelper.Pi * 1.75f);
            Projectile.velocity *= 0.985f;
            Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 12f, 0f, 1f);

            Color glowColor = GetGlowColor();
            Lighting.AddLight(Projectile.Center, glowColor.ToVector3() * 0.65f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    IsPlatinum ? DustID.Enchanted_Pink : DustID.Enchanted_Gold,
                    Projectile.velocity.X * 0.05f,
                    Projectile.velocity.Y * 0.05f,
                    120,
                    glowColor,
                    IsHeavy ? 1.2f : 0.9f);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ItemType <= ItemID.None || ItemType >= TextureAssets.Item.Length)
                return false;

            Texture2D texture = TextureAssets.Item[ItemType].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Color glowColor = GetGlowColor();
            SpriteEffects effects = Projectile.spriteDirection > 0
                ? SpriteEffects.None
                : SpriteEffects.FlipVertically;

            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float trailStrength = 1f - i / (float)Projectile.oldPos.Length;
                Vector2 drawPosition = Projectile.oldPos[i]
                    + Projectile.Size * 0.5f
                    - Main.screenPosition;
                Color trailColor = new Color(glowColor.R, glowColor.G, glowColor.B, 0)
                    * (0.35f * trailStrength * Projectile.Opacity);

                Main.EntitySpriteDraw(
                    texture,
                    drawPosition,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale,
                    effects);
            }

            Color coreColor = new Color(255, 255, 255, 0) * Projectile.Opacity;
            Color auraColor = new Color(glowColor.R, glowColor.G, glowColor.B, 0)
                * (0.75f * Projectile.Opacity);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                auraColor,
                Projectile.rotation,
                origin,
                Projectile.scale * 1.12f,
                effects);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                coreColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects);

            return false;
        }

        private Color GetGlowColor()
        {
            return IsPlatinum
                ? new Color(170, 225, 255)
                : new Color(255, 205, 75);
        }
    }
}
