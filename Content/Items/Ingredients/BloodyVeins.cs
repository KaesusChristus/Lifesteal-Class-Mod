using LifeStealClass.Content.Core;
using Terraria;
using Terraria.ID;

namespace LifeStealClass.Content.Items.Ingredients
{
    public class BloodyVeins : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.value = Item.sellPrice(0, 0, 7);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }
    }
}