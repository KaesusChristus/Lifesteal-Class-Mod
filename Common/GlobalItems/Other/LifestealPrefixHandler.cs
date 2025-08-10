using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Weapons.Spears;
using Terraria.Utilities;

namespace LifeStealClass.Common.GlobalItems.Other
{
    public class LifestealPrefixHandler : GlobalItem
    {
        public override bool AllowPrefix(Item item, int pre)
        {
            if (item.ModItem is LifestealSpearWeapon)
            {
                return true;
            }
            return base.AllowPrefix(item, pre);
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if (item.ModItem is LifestealSpearWeapon)
            {
                int[] possiblePrefixes = new int[]
                {
                    PrefixID.Legendary,
                    PrefixID.Godly,
                    PrefixID.Demonic,
                    PrefixID.Forceful,
                    PrefixID.Hurtful,
                    PrefixID.Strong,
                    PrefixID.Unpleasant,
                    PrefixID.Weak,
                    PrefixID.Ruthless,
                    PrefixID.Broken,
                    PrefixID.Damaged,
                    PrefixID.Shoddy,
                    PrefixID.Massive,
                    PrefixID.Savage,
                    PrefixID.Slow,
                    PrefixID.Sluggish,
                    PrefixID.Lazy,
                    PrefixID.Annoying,
                    PrefixID.Nasty
                };

                return possiblePrefixes[rand.Next(possiblePrefixes.Length)];
            }

            return base.ChoosePrefix(item, rand);
        }
    }
}
