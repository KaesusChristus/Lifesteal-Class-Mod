using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Accessories
{
    public class GreaterLifeCrystal : LifeStealItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 80);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<HealthIncreaseOverrideRule>().doOverride == false)
            {
                player.statLifeMax2 += 100;
            }

            player.GetModPlayer<LifestealEffectsPlayer>().bonusHealOnHit += 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LifeCrystal, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
