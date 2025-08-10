using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class SlimeSickleProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/SlimeSickle";

        //private Vector2 targetPos; // Wo die Maus beim Abschuss war
        //private bool returning = false;
        //private const float maxDistance = 400f;
        //private const float speed = 20f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 60;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.t_Slime, 0f, 0f, 100, new Color(70, 70 , 255), 1.4f);
                dust.noGravity = true;
            }
        }
        
        public override Color GetGlowColor()
        {
            return new Color(100, 180, 255); // hellblau, geisterhaft
        }



        /*
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.ai[0] == 0)
            {
                targetPos = Main.MouseWorld;
                Vector2 toTarget = targetPos - Projectile.Center;
                if (toTarget.Length() > maxDistance)
                {
                    toTarget = Vector2.Normalize(toTarget) * maxDistance;
                    targetPos = Projectile.Center + toTarget;
                }
                Projectile.velocity = Vector2.Normalize(toTarget) * speed;

                // Spieler Richtung speichern (1 = rechts, -1 = links)
                Projectile.ai[1] = player.direction;

                Projectile.ai[0] = 1;
            }

            if (!returning)
            {
                if (Vector2.Distance(Projectile.Center, targetPos) < speed)
                {
                    returning = true;
                }
            }

            if (returning)
            {
                Vector2 toPlayer = player.Center - Projectile.Center;
                Projectile.velocity = Vector2.Normalize(toPlayer) * speed;

                if (toPlayer.Length() < speed)
                {
                    Projectile.Kill();
                }
            }

            // Grundrotation = Sprite zeigt nach oben rechts → -45° nötig
            float rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver4;

            // Wenn der Spieler beim Abschuss nach links schaute, -90° Rotation korrigieren
            if (Projectile.ai[1] == -1)
            {
                rotation -= MathHelper.PiOver2;
            }

            Projectile.rotation = rotation;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;

            // Spiegelung nach der gespeicherten Richtung beim Abschuss
            if (Projectile.ai[1] == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.Draw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                effects,
                0f
            );

            return false; // Wir zeichnen selbst, kein Vanilla-Zeichnen mehr
        }

        */

    }
}
