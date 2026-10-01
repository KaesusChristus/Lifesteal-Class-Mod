using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Ingredients;
using Terraria.GameContent.ItemDropRules;


namespace LifeStealClass.Common.GlobalNpcs.ItemDropRules
{
    public class LifeShardDropChance : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new BloodMoonCondition(), ModContent.ItemType<LifeShard>(),
                chanceDenominator: 1, minimumDropped: 1, maximumDropped: 3));
        }
    }

    public class BloodMoonCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return Main.bloodMoon && Main.rand.NextFloat() < 0.1f; // 10% Chance
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "Drops during a Blood Moon";
        }
    }
}
