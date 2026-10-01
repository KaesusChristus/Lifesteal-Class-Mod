using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using HarvesterClassMod.Content.Items.Ingredients;
using HarvesterClassMod.Content.Projectiles.Weapon.Sickle;

namespace HarvesterClassMod.Content.Items.Weapons.Sickle
{
    public class TinSickle : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 42;
            Item.height = 32;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 0, 20);

            Item.damage = 7;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.shoot = ModContent.ProjectileType<TinSickleProjectile>();
            Item.shootSpeed = 7f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TinBar, 8);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
