using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class AquaScytheProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/AquaScythe";

        private int timer;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.WaterCandle, 0f, 0f, 100, new Color(140, 27, 130), 1.8f);
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            base.AI();

            int count = 2;
            float speed = 12f;

            timer++;

            if (timer % 30 == 0)
            {
                float baseRotation = Projectile.velocity.ToRotation();
                float spread = MathHelper.ToRadians(180f);
                float startAngle = baseRotation - spread / 2f;
                float angleStep = spread / (count - 1);

                for (int i = 0; i < count; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item21, Projectile.position);

                    float angle = startAngle + angleStep * i;
                    Vector2 direction = angle.ToRotationVector2();
                    Vector2 spawnPos = Projectile.Center + direction;
                    Vector2 velocity = direction * speed * 0.75f;


                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPos, velocity, ProjectileID.WaterBolt, Projectile.damage / 4, Projectile.knockBack, Projectile.owner, 1f);
                    proj.penetrate = 1;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);
            Vector2 direction = randomAngle.ToRotationVector2();

            float speed = 5f;
            float offset = -50f;

            Vector2 velocity = direction * speed;
            Vector2 spawnPos = target.Center + direction * offset;

            Projectile proj = Projectile.NewProjectileDirect(target.GetSource_FromThis(), spawnPos, velocity, ProjectileID.Muramasa, Projectile.damage / 4, 3f, Main.myPlayer);
            proj.penetrate = -1;



            Player localPlayer = Main.LocalPlayer;
            localPlayer.GetModPlayer<LifestealEffectsPlayer>().allowHeal = false;
        }

        public override Color GetGlowColor()
        {
            return new Color(40, 90, 205);
        }
    }
}
