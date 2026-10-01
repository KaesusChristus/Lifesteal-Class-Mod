using LifeStealClass.Content.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace LifeStealClass.Content.Items.Ingredients
{
    public class LifeShard : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.value = Item.sellPrice(0, 0, 2);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe(10);
            recipe1.AddIngredient(ItemID.LifeCrystal);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe(2);
            recipe2.AddIngredient(ModContent.ItemType<BloodCells>(), 5);
            recipe2.Register();
        }
    }
}
