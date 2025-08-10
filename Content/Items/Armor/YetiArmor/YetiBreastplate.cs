using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace LifeStealClass.Content.Items.Armor.YetiArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class YetiBreastplate : LifeStealItem
    {
        public static readonly int CritStrikeChance = 5;
        public static readonly float damageBoost = 0.12f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritStrikeChance, damageBoost);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID.Orange;

            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.GetDamage(ModContent.GetInstance<LifestealDamage>()) += damageBoost;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<Flur>(), 18);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }
    }
}
