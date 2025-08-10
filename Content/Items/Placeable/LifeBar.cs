using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using LifeStealClass;
using LifeStealClass.Content;
using LifeStealClass.Content.Tiles;
using LifeStealClass.Content.Items.Ingredients;
using Terraria.DataStructures;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Placeable
{
    public class LifeBar : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            // Registriere die Animation für dieses Item
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(20, 2));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            // ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LifeBarTile>());
            Item.width = 30;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 0, 60);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe(2);
            recipe1.AddIngredient(ItemID.DemoniteBar, 1);
            recipe1.AddIngredient(ModContent.ItemType<LifeShard>(), 2);
            recipe1.AddTile(TileID.Furnaces);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe(2);
            recipe2.AddIngredient(ItemID.CrimtaneBar, 1);
            recipe2.AddIngredient(ModContent.ItemType<LifeShard>(), 2);
            recipe2.AddTile(TileID.Furnaces);
            recipe2.Register();
        }
    }
}
