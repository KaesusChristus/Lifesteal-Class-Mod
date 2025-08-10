using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Armor.ZomboidArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class ZomboidBreastplate : LifeStealItem
    {
        public static readonly int MaxLifeBoost = 40;
        public static readonly float damageBoost = 0.12f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxLifeBoost, damageBoost);
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 90);
            Item.rare = ItemRarityID.Green;

            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += MaxLifeBoost;
            player.GetDamage(ModContent.GetInstance<LifestealDamage>()) += damageBoost;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodCells>(), 50);
            recipe.AddIngredient(ModContent.ItemType<BloodyVeins>(), 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
