using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class LeadSickle : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 42;
            Item.height = 32;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 0, 50);

            Item.damage = 10;

            Item.useTime = 40;
            Item.useAnimation = 40;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LeadBar, 8);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe = CreateRecipe();
        }
    }
}
