using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Placeable;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Armor.DarbBloodArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DarkBloodLeggins : LifeStealItem
    {

        public static readonly int CritStrikeChance = 2;
        public static readonly float movementSpeed = 0.08f;


        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritStrikeChance, movementSpeed);
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 50);
            Item.rare = ItemRarityID.Green;

            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.moveSpeed += movementSpeed;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 16);
            recipe1.AddIngredient(ItemID.ShadowScale, 15);
            recipe1.AddTile(TileID.DemonAltar);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<LifeBar>(), 16);
            recipe2.AddIngredient(ItemID.TissueSample, 15);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}
