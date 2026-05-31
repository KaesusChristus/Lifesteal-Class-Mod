using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class TungstenScythe : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 42;
            Item.height = 32;
            Item.scale = 1.4f;

            Item.value = Item.sellPrice(0, 0, 90);
            Item.damage = 15;

            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<TungstenScytheProjectile>();
        }
    }
}