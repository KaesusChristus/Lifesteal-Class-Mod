using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Projectiles.Weapon.Spears
{
    public class SpearFromTheDevilProjectile : BaseSpearProjectile
    {
        public override float HoldoutRangeMin => 50f; // Spitze
        public override float HoldoutRangeMax => 220; // Ende

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
            if (Main.rand.NextBool(2))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 128, Scale: 1.2f);
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Alpha: 128, Scale: 0.3f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);

            if (target.life <= 0 && target.lifeMax > 5)
            {
                Player localPlayer = Main.LocalPlayer;

                localPlayer.GetModPlayer<LifestealEffectsPlayer>().SetHealAmount(8);
            }
        }
    }
}
