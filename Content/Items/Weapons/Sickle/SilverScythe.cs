using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using LifeStealClass.Common.GlobalItems.Other;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class SilverScythe : LifestealSickle
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
            Item.shoot = ModContent.ProjectileType<SilverScytheProjectile>();
            Item.shootSpeed = 7f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SilverBar, 10);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe = CreateRecipe();
        }
    }
}
