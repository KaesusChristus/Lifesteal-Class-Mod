using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Projectiles.Weapon.Spears
{
    public class DarkHalberdProjectile : BaseSpearProjectile
    {
        public override float HoldoutRangeMin => 40f; // Spitze
        public override float HoldoutRangeMax => 160f; // Ende

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.aiStyle = 19; // Spear aiStyle
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<LifestealDamage>();
        }

        public override void SpawnDust()
        {
            // Dust Effect
            if (Main.rand.NextBool(3))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);
            }

            if (Main.rand.NextBool(4))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Alpha: 128, Scale: 0.3f);
            }
        }


        // Heal the player, if the target dies
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0 && target.lifeMax > 5)
            {
                Player localPlayer = Main.LocalPlayer;

                localPlayer.GetModPlayer<LifestealEffectsPlayer>().SetHealAmount(4);
            }
        }
    }
}
