using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public abstract class LifestealSickle : LifeStealItem
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;

            Item.rare = ItemRarityID.Blue;
            // Item.value = Item.sellPrice(0, 0, 70);

            // Item.damage = 30;
            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.knockBack = 4f;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 1;
        }

        public override bool MeleePrefix()
        {
            return true; // Makes the Sickle receive melee modifiers
        }
    }
}
