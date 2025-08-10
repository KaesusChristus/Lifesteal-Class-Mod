using LifeStealClass.Content.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Common.GlobalItems.VanillaItemModification
{
    public class AlternateRecipe : GlobalItem
    {
        public override void AddRecipes()
        {
            Recipe LifeCrystal1 = Recipe.Create(ItemID.LifeCrystal);
            LifeCrystal1.AddIngredient(ModContent.ItemType<LifeShard>(), 10);
            LifeCrystal1.AddIngredient(ModContent.ItemType<SoulOfBlood>(), 3);
            LifeCrystal1.AddTile(TileID.DemonAltar);
            LifeCrystal1.Register();
        }
    }
}
