using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Common.GlobalProjectiles.VanillaProjectileModification
{
    public class BoneHelmProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            // Überprüfe, ob das Projektil vom Bone Helm ist
            if (projectile.type == ProjectileID.InsanityShadowFriendly)
            {
                projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
                projectile.damage = +20;
                projectile.CritChance = 100;
            }
        }
    }
}
