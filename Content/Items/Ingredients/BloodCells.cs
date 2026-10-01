using LifeStealClass.Content.Core;
using Terraria;
using Terraria.ID;

namespace LifeStealClass.Content.Items.Ingredients
{
    public class BloodCells : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.value = Item.sellPrice(0, 0, 0, 30);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }
    }
}
