using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Armor.ZomboidArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ZomboidLegs : LifeStealItem
    {
        public static readonly float jumpBoni = 2f;
        public static readonly float movementSpeed = 0.08f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(jumpBoni, movementSpeed);
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 85);
            Item.rare = ItemRarityID.Green;

            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += jumpBoni;
            player.moveSpeed += movementSpeed;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodCells>(), 40);
            recipe.AddIngredient(ModContent.ItemType<BloodyVeins>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
