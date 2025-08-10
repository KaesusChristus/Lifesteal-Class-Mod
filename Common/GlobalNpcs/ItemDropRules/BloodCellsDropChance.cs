using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Common.GlobalNPCs
{
    public class BloodCellsDropChance : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            // Prüfe, ob der NPC ein Zombie ist
            return entity.type == NPCID.Zombie ||
                   entity.type == NPCID.ZombieEskimo ||
                   entity.type == NPCID.ArmedZombie ||
                   entity.type == NPCID.BaldZombie ||
                   entity.type == NPCID.PincushionZombie ||
                   entity.type == NPCID.SlimedZombie ||
                   entity.type == NPCID.ZombieDoctor ||
                   entity.type == NPCID.ZombieSuperman ||
                   entity.type == NPCID.DemonEye ||
                   entity.type == NPCID.CataractEye ||
                   entity.type == NPCID.DialatedEye ||
                   entity.type == NPCID.GreenEye ||
                   entity.type == NPCID.PurpleEye ||
                   entity.type == NPCID.SleepyEye ||
                   entity.type == NPCID.DemonEyeOwl;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (AppliesToEntity(npc, false))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodCells>(), 2, 2, 5)); // 50% Chance
            }
        }
    }
}
