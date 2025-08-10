using Terraria;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Ingredients;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace LifeStealClass.Common.GlobalNpcs.ItemDropRules
{
    public class BloodSoulGlobalNPC : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            // Drop während Blutmond bei allen Gegnern (25% Chance)
            globalLoot.Add(ItemDropRule.ByCondition(new SoulOfBloodBloodMoonCondition(),
                ModContent.ItemType<SoulOfBlood>(), chanceDenominator: 1)); // 25% Chance (Siehe SoulOfBloodBloodMoonCondition)

            // Drop bei Zombies jederzeit (12,5% Chance)
            globalLoot.Add(ItemDropRule.ByCondition(new SoulOfBloodZombieCondition(),
                ModContent.ItemType<SoulOfBlood>(), chanceDenominator: 8)); // 12,5% Chance

            globalLoot.Add(ItemDropRule.ByCondition(new SoulOfBloodEyeCondition(),
                ModContent.ItemType<SoulOfBlood>(), chanceDenominator: 8)); // 12,5% Chance

            globalLoot.Add(ItemDropRule.ByCondition(new SoulOfBloodWerewolfCondition(),
                ModContent.ItemType<SoulOfBlood>(), chanceDenominator: 1)); // 100% Chance


        }
    }

    // Condition für Blutmond
    public class SoulOfBloodBloodMoonCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return Main.hardMode && Main.bloodMoon && Main.rand.NextFloat() < 0.25f; // 25% Chance wird hier schon definiert
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops from any enemy during a Blood Moon";
    }

    // Condition für Zombies
    public class SoulOfBloodZombieCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!Main.hardMode) return false;

            NPC npc = info.npc;

            return npc.type == NPCID.Zombie ||
                   npc.type == NPCID.ZombieEskimo ||
                   npc.type == NPCID.ArmedZombie ||
                   npc.type == NPCID.BaldZombie ||
                   npc.type == NPCID.PincushionZombie ||
                   npc.type == NPCID.SlimedZombie ||
                   npc.type == NPCID.ZombieDoctor ||
                   npc.type == NPCID.ZombieSuperman;
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops from zombies";
    }

    public class SoulOfBloodEyeCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!Main.hardMode) return false;

            int type = info.npc.type;

            return type == NPCID.DemonEye ||
                   type == NPCID.CataractEye ||
                   type == NPCID.DialatedEye ||
                   type == NPCID.GreenEye ||
                   type == NPCID.PurpleEye ||
                   type == NPCID.SleepyEye ||
                   type == NPCID.DemonEyeOwl;
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops from flying eyes";
    }

    public class SoulOfBloodWerewolfCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            int type = info.npc.type;

            // Liste der Werwölfe
            return type == NPCID.Werewolf;
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Guaranteed drop from Werewolves";
    }


}
