using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Projectiles.Weapon.WarScythe
{
    public class SpearFromTheDevilProjectile : BaseSpearProjectile
    {
        public override WarScytheStats GetStats()
        {
            WarScytheStats stats = base.GetStats();
            stats.ThrustTime = 6f;
            stats.SwingTime = 9f;
            stats.RecoverTime = 5f;
            stats.ThrustStartGrip = 100f;
            stats.ThrustEndGrip = 50f;
            stats.SwingGrip = 75f;
            stats.RecoverGrip = 100f;
            stats.DashGrip = 75f;
            stats.WeaponLength = 218f;
            stats.HitboxWidth = 22f;
            return stats;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 64;
            Projectile.height = 64;
        }

        public override void SpawnDust()
        {
            // Dust Effect
            if (Main.rand.NextBool(2))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, AttackDirection.X * 2f, AttackDirection.Y * 2f, Alpha: 128, Scale: 1.2f);
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
