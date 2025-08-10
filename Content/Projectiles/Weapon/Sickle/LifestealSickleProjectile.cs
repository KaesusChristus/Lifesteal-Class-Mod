using LifeStealClass.Content.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public abstract class LifestealSickleProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<LifestealDamage>();
            Projectile.damage = 20;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.localAI[0] == 0)
            {
                // Sichtlinienprüfung einmalig beim Spawnen
                bool canHit = Collision.CanHitLine(
                    player.Center, 1, 1,
                    Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 40f,
                    1, 1
                );

                Projectile.localAI[1] = canHit ? 1f : 0f;
                Projectile.ai[1] = player.direction;
                Projectile.localAI[0] = 1f;
            }

            Projectile.rotation += 0.3f * Projectile.ai[1];

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = Main.rand.NextVector2Circular(Projectile.width / 2f, Projectile.height / 2f);
                Dust dust = Dust.NewDustDirect(Projectile.Center + offset, 0, 0, DustID.BlueTorch, 0f, 0f, 150, GetGlowColor(), 0.8f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.localAI[1] == 0f)
            {
                modifiers.FinalDamage *= 0f;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Setze eine eigene Flag, dass dieses Projectile KEINE Heilung auslösen soll
            Projectile.localAI[1] = 1f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.ai[1] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color glowColor = GetGlowColor();

            // Glow-Schicht
            Main.spriteBatch.Draw(texture, drawPos, null, glowColor * 0.3f, Projectile.rotation, drawOrigin, Projectile.scale * 1.2f, effects, 0f);

            // Normales Projektil
            Main.spriteBatch.Draw(texture, drawPos, null, glowColor, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);

            return false;
        }

        public virtual Color GetGlowColor()
        {
            return Color.White;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.friendly)
                return false;

            return Projectile.localAI[1] == 1f;
        }
    }
}
