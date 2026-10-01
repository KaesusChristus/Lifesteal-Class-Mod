using HarvesterClassMod.Common.GlobalItems.Other;
using HarvesterClassMod.Content.Items.Ingredients;
using HarvesterClassMod.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarvesterClassMod.Content.Items.Weapons.Sickle
{
    public class ShadowsWisdom : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 54;
            Item.height = 46;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 3, 40);
            Item.rare = ItemRarityID.Green;

            Item.damage = 15;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.shoot = ModContent.ProjectileType<ShadowsWisdomProjectile>();
            Item.shootSpeed = 7f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 18);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
