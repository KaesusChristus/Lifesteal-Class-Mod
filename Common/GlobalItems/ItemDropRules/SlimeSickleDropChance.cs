using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Weapons.Sickle;

namespace LifeStealClass.Common.GlobalItems.ItemDropRules
{
    public class SlimeSickleDropChance : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.KingSlimeBossBag)
            {
                // 1 von 3 entspricht einer 33%igen Chance
                itemLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SlimeSickle>(), 3));
            }
        }
    }
}