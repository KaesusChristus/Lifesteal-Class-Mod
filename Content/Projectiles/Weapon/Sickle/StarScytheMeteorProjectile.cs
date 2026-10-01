using LifeStealClass.Content.Core;
using LifeStealClass.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public sealed class StarScytheMeteorProjectile : ModProjectile, INoLifestealProjectile
    {
        private int TargetIndex => (int)Projectile.ai[0];

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Meteor1;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 58 && TargetIndex >= 0 && TargetIndex < Main.maxNPCs)
            {
                NPC target = Main.npc[TargetIndex];
                if (target.active && !target.friendly)
                {
                    Vector2 desiredVelocity = (target.Center - Projectile.Center)
                        .SafeNormalize(Vector2.UnitY)
                        * 13f;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.09f);
                }
            }

            Projectile.rotation += 0.24f * (Projectile.velocity.X >= 0f ? 1f : -1f);
            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.35f, 0.08f));

            Dust dust = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.MeteorHead,
                -Projectile.velocity.X * 0.12f,
                -Projectile.velocity.Y * 0.12f,
                80,
                default,
                1.25f);
            dust.noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[ProjectileID.Meteor1].Value;
            Vector2 origin = texture.Size() * 0.5f;

            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float strength = 1f - i / (float)Projectile.oldPos.Length;
                Vector2 drawPosition = Projectile.oldPos[i]
                    + Projectile.Size * 0.5f
                    - Main.screenPosition;
                Color trailColor = new Color(255, 90, 25, 0) * (0.45f * strength);

                Main.EntitySpriteDraw(
                    texture,
                    drawPosition,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None);
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 18; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.MeteorHead,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    80,
                    default,
                    1.5f);
                dust.noGravity = true;
            }
        }
    }
}
