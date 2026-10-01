using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using HarvesterClassMod.Content.Items.Accessories;

namespace HarvesterClassMod.Common.GlobalItems.ItemDropRules
{
    public class ToothOfCthulhuDropChance : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.EyeOfCthulhuBossBag)
            {
                // 1 von 3 entspricht einer 33%igen Chance
                itemLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<ToothOfCthulhu>(), 3));
            }
        }
    }
}
