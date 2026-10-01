using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Ingredients;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using System.Collections.Generic;

namespace LifeStealClass.Common.GlobalNpcs.ItemDropRules
{
    public class BloodyVeinsDropChance : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new ZombieOrEyeBloodMoonCondition(), ModContent.ItemType<BloodyVeins>(), 3, 1, 3)); // 1 in 3 Chance bei Blood Moon
            globalLoot.Add(ItemDropRule.ByCondition(new ZombieOrEyeNormalNightCondition(), ModContent.ItemType<BloodyVeins>(), 8, 1, 3)); // 1 in 8 Chance bei normaler Nacht
        }
    }

    public class ZombieOrEyeBloodMoonCondition : IItemDropRuleCondition
    {
        private static readonly HashSet<int> ValidNpcIds = new()
        {
            NPCID.Zombie,
            NPCID.ZombieEskimo,
            NPCID.ArmedZombie,
            NPCID.BaldZombie,
            NPCID.PincushionZombie,
            NPCID.SlimedZombie,
            NPCID.ZombieDoctor,
            NPCID.ZombieSuperman,
            NPCID.DemonEye,
            NPCID.CataractEye,
            NPCID.DialatedEye,
            NPCID.GreenEye,
            NPCID.PurpleEye,
            NPCID.SleepyEye,
            NPCID.DemonEyeOwl,
            NPCID.BloodZombie,
            NPCID.Drippler
        };

        public bool CanDrop(DropAttemptInfo info)
        {
            return Main.bloodMoon && ValidNpcIds.Contains(info.npc.type);
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops from Zombies, Demon Eyes, Blood Zombies, or Dripplers during a Blood Moon";
    }

    public class ZombieOrEyeNormalNightCondition : IItemDropRuleCondition
    {
        private static readonly HashSet<int> ValidNpcIds = new()
        {
            NPCID.Zombie,
            NPCID.ZombieEskimo,
            NPCID.ArmedZombie,
            NPCID.BaldZombie,
            NPCID.PincushionZombie,
            NPCID.SlimedZombie,
            NPCID.ZombieDoctor,
            NPCID.ZombieSuperman,
            NPCID.DemonEye,
            NPCID.CataractEye,
            NPCID.DialatedEye,
            NPCID.GreenEye,
            NPCID.PurpleEye,
            NPCID.SleepyEye,
            NPCID.DemonEyeOwl
        };

        public bool CanDrop(DropAttemptInfo info)
        {
            return !Main.dayTime && !Main.bloodMoon && ValidNpcIds.Contains(info.npc.type);
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops from Zombies or Demon Eyes at night";
    }
}
