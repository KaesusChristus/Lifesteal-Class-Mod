using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Placeable;

namespace LifeStealClass.Content.Items.Accessories
{
    public class HealinfoDisplayItem : ModItem
    {
		public override void SetStaticDefaults()
        {
            // We want the information benefits of this accessory to work while in the void bag in order to keep
            // it in line with the vanilla accessories; This is the default behavior.
            // If you DON'T want your info accessory to work in the void bag, then add: ItemID.Sets.WorksInVoidBag[Type] = false;
        }

        public override void SetDefaults()
        {
            // We don't need to add anything particularly unique for the stats of this item; so let's just clone the Radar.
            Item.CloneDefaults(ItemID.DPSMeter);
        }

        // This is the main hook that allows for our info display to actually work with this accessory. 
        public override void UpdateInfoAccessory(Player player)
        {
            player.GetModPlayer<HealinfoDisplayPlayer>().showHealInformation = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 10);
            recipe1.AddIngredient(ModContent.ItemType<LifeShard>(), 6);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }

    }
}
