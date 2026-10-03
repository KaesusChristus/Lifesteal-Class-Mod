using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Weapons.WarScythe;
using Terraria.Utilities;
using LifeStealClass.Content.Prefixes;

namespace LifeStealClass.Common.GlobalItems.Other
{
    public class LifestealPrefixHandler : GlobalItem
    {
        public override bool AllowPrefix(Item item, int pre)
        {
            if (item.ModItem is LifestealWarScytheWeapon)
            {
                return WarScythePrefixPool.Contains(pre);
            }
            return base.AllowPrefix(item, pre);
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if (item.ModItem is LifestealWarScytheWeapon)
            {
                return WarScythePrefixPool.Choose(rand);
            }

            return base.ChoosePrefix(item, rand);
        }
    }
}
