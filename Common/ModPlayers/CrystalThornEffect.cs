using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Projectiles.Accessories;

namespace LifeStealClass.Common.ModPlayers
{
    public class CrystalThornEffect : ModPlayer
    {
        public bool hasCrystalThornEquipped = false;
        private int setOverHealDamage;

        public override void ResetEffects()
        {
            hasCrystalThornEquipped = false;
            setOverHealDamage = 0;
        }

        public override void PostUpdateEquips()
        {
            if (hasCrystalThornEquipped)
            {
                int overHeal = Player.GetModPlayer<LifestealEffectsPlayer>().getOverHeal;
                if (overHeal > 0)
                {
                    setOverHealDamage = overHeal;
                    var source = Player.GetSource_FromThis();

                    Projectile.NewProjectile(source, position: Player.Center, velocity: new Vector2(0, 0), ModContent.ProjectileType<CrystalThornProjectile>(), setOverHealDamage, 0f, Player.whoAmI);

                    Player.GetModPlayer<LifestealEffectsPlayer>().getOverHeal = 0;
                }
            }
        }
    }
}
