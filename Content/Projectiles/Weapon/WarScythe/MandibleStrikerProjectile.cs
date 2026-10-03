using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.ModPlayers;
using Terraria.ID;

namespace LifeStealClass.Content.Projectiles.Weapon.WarScythe
{
    public class MandibleStrikerProjectile : BaseSpearProjectile
    {
        public override WarScytheStats GetStats()
        {
            WarScytheStats stats = base.GetStats();
            stats.ThrustStartGrip = 42f;
            stats.ThrustEndGrip = 20f;
            stats.SwingGrip = 32f;
            stats.RecoverGrip = 42f;
            stats.DashGrip = 32f;
            stats.WeaponLength = 88f;
            stats.HitboxWidth = 12f;
            stats.Scale = 1.2f;
            return stats;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 32;
            Projectile.height = 32;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            if (player.GetModPlayer<WarScytheDash>().IsDashing)
            {
                target.AddBuff(BuffID.Poisoned, 240);
            }
        }
    }
}
