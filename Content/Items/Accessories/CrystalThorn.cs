using HarvesterClassMod.Common.ModPlayers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HarvesterClassMod.Content.Core;
using HarvesterClassMod.Content.Items.Ingredients;

namespace HarvesterClassMod.Content.Items.Accessories
{
    public class CrystalThorn : LifeStealItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 7, 50);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CrystalThornEffect>().hasCrystalThornEquipped = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LifeCrystal, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ModContent.ItemType<SoulOfBlood>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
