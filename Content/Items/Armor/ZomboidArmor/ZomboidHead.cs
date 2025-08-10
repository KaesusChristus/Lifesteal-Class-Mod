using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Armor.ZomboidArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class ZomboidHead : LifeStealItem
    {
        public static readonly int CritStrikeChance = 5;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritStrikeChance);
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true; // Draw head
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 80);
            Item.rare = ItemRarityID.Green;

            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool bodyMatch = body.type == ModContent.ItemType<ZomboidBreastplate>();
            bool legsMatch = legs.type == ModContent.ItemType<ZomboidLegs>();
            return bodyMatch && legsMatch;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.LifeStealClass.ItemSetBonus.ZomboidSet");
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodCells>(), 35);
            recipe.AddIngredient(ModContent.ItemType<BloodyVeins>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
