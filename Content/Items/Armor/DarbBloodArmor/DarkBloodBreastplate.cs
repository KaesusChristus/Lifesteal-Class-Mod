using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Placeable;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Armor.DarbBloodArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class DarkBloodBreastplate : LifeStealItem
    {

        public static readonly int CritStrikeChance = 3;
        public static readonly float damageBoost = 0.1f;


        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritStrikeChance, damageBoost);
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 3);
            Item.rare = ItemRarityID.Green;

            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.GetDamage(ModContent.GetInstance<LifestealDamage>()) += damageBoost;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 20);
            recipe1.AddIngredient(ItemID.ShadowScale, 20);
            recipe1.AddTile(TileID.DemonAltar);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<LifeBar>(), 20);
            recipe2.AddIngredient(ItemID.TissueSample, 20);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}
