using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Items.Placeable;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Accessories
{
    public class DarkBloodCrystal : LifeStealItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 50);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DarkBloodCrystalEffect>().hasDarkBloodCrystalEquipped = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(ModContent.ItemType<LifeBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<BloodyVeins>(), 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
