using LifeStealClass.Content.Core;
using Terraria;
using Terraria.ID;

namespace LifeStealClass.Content.Items.Ingredients
{
    public class Flur : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.value = Item.sellPrice(0, 0, 70);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
        }
    }
}
