using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Common.ModPlayers.SetBoni;
using LifeStealClass.Content.Items.Placeable;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Armor.DarbBloodArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class DarkBloodHelmet : LifeStealItem
    {

        public static readonly int CritStrikeChance = 10;
        public static readonly float damageBoost = 0.05f;

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true; // Draw head
        }


        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 2);
            Item.rare = ItemRarityID.Green;

            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.GetDamage(ModContent.GetInstance<LifestealDamage>()) += damageBoost;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool bodyMatch = body.type == ModContent.ItemType<DarkBloodBreastplate>();
            bool legsMatch = legs.type == ModContent.ItemType<DarkBloodLeggins>();
            return bodyMatch && legsMatch;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.LifeStealClass.ItemSetBonus.DarkBloodSet");
            player.GetModPlayer<DarkBloodArmorSetBonus>().setBonusActive = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 14);
            recipe1.AddIngredient(ItemID.ShadowScale, 10);
            recipe1.AddTile(TileID.DemonAltar);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<LifeBar>(), 14);
            recipe2.AddIngredient(ItemID.TissueSample, 10);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}