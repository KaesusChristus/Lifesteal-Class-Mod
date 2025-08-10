using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public class LavaSickleProjectile : LifestealSickleProjectile
    {
        public override string Texture => "LifeStealClass/Content/Items/Weapons/Sickle/LavaSickle";

        private int timer;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item170, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, new Color(140, 27, 130), 1.8f);
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            base.AI();

            timer++;

            if (timer % 10 == 0)
            {
                float randomAngle = MathHelper.ToRadians(Main.rand.NextFloat(0f, 360f));
                float speed = Main.rand.NextFloat(4f, 7f);

                Vector2 velocity = randomAngle.ToRotationVector2() * speed;
                Vector2 spawnPos = Projectile.Center;

                Projectile molotov = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPos, velocity, ProjectileID.MolotovFire, Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                molotov.penetrate = 1;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(target.GetSource_FromThis(), target.position, Vector2.Zero, ProjectileID.Volcano, Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);

            Player localPlayer = Main.LocalPlayer;
            localPlayer.GetModPlayer<LifestealEffectsPlayer>().allowHeal = false;
        }

        public override Color GetGlowColor()
        {
            return new Color(252, 71, 20);
        }
    }
}
