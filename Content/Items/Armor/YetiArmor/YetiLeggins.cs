using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Armor.YetiArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class YetiLeggins : LifeStealItem
    {
        public static readonly int CritStrikeChance = 2;
        public static readonly float movementSpeed = 0.1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritStrikeChance, movementSpeed);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 3, 50);
            Item.rare = ItemRarityID.Orange;

            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.moveSpeed += movementSpeed;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<Flur>(), 12);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }
    }
}
