using LifeStealClass.Content.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Ingredients
{
    public class BloodyStinger : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Stinger, 1);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 2);
            recipe.Register();
        }
    }
}
