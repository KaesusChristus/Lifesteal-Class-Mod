using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Projectiles.Weapon.Spears
{
    public abstract class BaseSpearProjectile : ModProjectile
    {
        public virtual float HoldoutRangeMin => 60f;
        public virtual float HoldoutRangeMax => 150f;

        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;

            Projectile.aiStyle = 19;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<LifestealDamage>();
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;
            player.heldProj = Projectile.whoAmI;

            if (Projectile.timeLeft > duration)
                Projectile.timeLeft = duration;

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            float halfDuration = duration * 0.5f;
            float progress = Projectile.timeLeft < halfDuration
                ? Projectile.timeLeft / halfDuration
                : (duration - Projectile.timeLeft) / halfDuration;

            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(
                Projectile.velocity * HoldoutRangeMin,
                Projectile.velocity * HoldoutRangeMax,
                progress);

            Projectile.rotation += MathHelper.ToRadians(Projectile.spriteDirection == -1 ? 45f : 135f);

            SpawnDust();

            return false;
        }

        public virtual void SpawnDust() { }
    }
}
