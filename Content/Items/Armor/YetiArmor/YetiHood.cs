using LifeStealClass.Content.Items.Ingredients;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using LifeStealClass.Common.ModPlayers.SetBoni;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Armor.YetiArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class YetiHood : LifeStealItem
    {
        public static readonly int CritStrikeChance = 2;
        public static readonly float damageBoost = 0.12f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CritStrikeChance, damageBoost);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 3);
            Item.rare = ItemRarityID.Orange;

            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += CritStrikeChance;
            player.GetDamage(ModContent.GetInstance<LifestealDamage>()) += damageBoost;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool bodyMatch = body.type == ModContent.ItemType<YetiBreastplate>();
            bool legsMatch = legs.type == ModContent.ItemType<YetiLeggins>();
            return bodyMatch && legsMatch;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.LifeStealClass.ItemSetBonus.YetiSet");
            player.GetModPlayer<YetiArmorSetBonus>().setBonusActive = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<Flur>(), 14);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }
    }
}
