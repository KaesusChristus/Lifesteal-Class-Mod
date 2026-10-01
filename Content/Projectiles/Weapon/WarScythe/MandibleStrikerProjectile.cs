using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.ModPlayers;
using Terraria.ID;

namespace LifeStealClass.Content.Projectiles.Weapon.WarScythe
{
    public class MandibleStrikerProjectile : BaseSpearProjectile
    {
        public override float HoldoutRangeMin => 40f; // Spitze
        public override float HoldoutRangeMax => 100f; // Ende

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
