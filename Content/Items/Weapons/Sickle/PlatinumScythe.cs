using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using LifeStealClass.Common.GlobalItems.Other;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class PlatinumScythe : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.sellPrice(0, 2, 30);

            Item.damage = 17;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.scale = 1.2f;
            Item.shoot = ModContent.ProjectileType<PlatinumScytheProjectile>();
            Item.shootSpeed = 7f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 12);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 6);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
