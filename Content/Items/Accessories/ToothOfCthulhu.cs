using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Accessories
{
    public class ToothOfCthulhu : LifeStealItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 3);

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(ModContent.GetInstance<LifestealDamage>()) += 4; // 4%
            player.GetModPlayer<ToothOfCthulhuEffect>().hasToothOfCthulhuEquipped = true;
        }
    }
}
