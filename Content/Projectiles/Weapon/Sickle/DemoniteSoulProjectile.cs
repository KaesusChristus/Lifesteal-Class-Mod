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
    public sealed class DemoniteSoulProjectile : ModProjectile, INoLifestealProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DemonScythe;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 9;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 54;
            Projectile.scale = 0.7f;
            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
        }

        public override void AI()
        {
            Projectile.localAI[0]++;
            Projectile.rotation += 0.22f * (Projectile.velocity.X >= 0f ? 1f : -1f);
            Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 12f, 0f, 1f);

            if (Projectile.localAI[0] > 6f)
            {
                NPC target = FindClosestTarget(420f);
                if (target != null)
                {
                    const float speed = 7.5f;
                    Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * speed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.07f);
                }
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.38f, 0.05f, 0.48f));
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Shadowflame,
                    Projectile.velocity.X * 0.04f,
                    Projectile.velocity.Y * 0.04f,
                    100,
                    new Color(150, 55, 190),
                    1.05f);
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
                    new Color(145, 45, 205, 0) * (0.4f * strength * Projectile.Opacity),
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale,
                    SpriteEffects.None);
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                new Color(225, 160, 255, 0) * Projectile.Opacity,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None);
            return false;
        }

        private NPC FindClosestTarget(float maxDistance)
        {
            NPC closest = null;
            float closestDistance = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy(Projectile))
                    continue;

                float distance = Projectile.Distance(npc.Center);
                if (distance < closestDistance)
                {
                    closest = npc;
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }
}
