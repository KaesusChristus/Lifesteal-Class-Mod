using LifeStealClass.Content.Items.Weapons.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Common.Systems.ChestSystems
{
    public class AquaScytheSpawnSystem : ModSystem
    {
        public override void PostWorldGen()
        {
            float spawnChance = 0.15f;
            int moddedItemType = ModContent.ItemType<AquaScythe>();

            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && IsDungeonChest(chest))
                {
                    TryReplaceItem(chest, ItemID.Muramasa, moddedItemType, spawnChance);
                    TryReplaceItem(chest, ItemID.AquaScepter, moddedItemType, spawnChance);
                    TryReplaceItem(chest, ItemID.MagicMissile, moddedItemType, spawnChance);
                    TryReplaceItem(chest, ItemID.CobaltShield, moddedItemType, spawnChance);
                    TryReplaceItem(chest, ItemID.BlueMoon, moddedItemType, spawnChance);
                    TryReplaceItem(chest, ItemID.Handgun, moddedItemType, spawnChance);
                    TryReplaceItem(chest, ItemID.Valor, moddedItemType, spawnChance);
                }
            }
        }

        private static void TryReplaceItem(Chest chest, int vanillaItemID, int moddedItemID, float chance)
        {
            if (chest.item[0].type == vanillaItemID && Main.rand.NextFloat() < chance)
            {
                chest.item[0].SetDefaults(moddedItemID);
            }
        }

        private static bool IsDungeonChest(Chest chest)
        {
            int tileType = Main.tile[chest.x, chest.y].TileFrameX / 36;
            return tileType == 0 || tileType == 2 || tileType == 4 || tileType == 6 || tileType == 8;
        }
    }
}
