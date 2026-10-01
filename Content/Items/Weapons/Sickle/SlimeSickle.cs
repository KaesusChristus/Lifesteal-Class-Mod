using HarvesterClassMod.Common.GlobalItems.Other;
using HarvesterClassMod.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ModLoader;

namespace HarvesterClassMod.Content.Items.Weapons.Sickle
{
    public class SlimeSickle : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.sellPrice(0, 0, 70);

            Item.damage = 14;

            Item.useTime = 35;
            Item.useAnimation = 35;

            Item.shoot = ModContent.ProjectileType<SlimeSickleProjectile>();
            Item.shootSpeed = 7f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 2;
        }
    }
}
