using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class StarScythe : LifestealSickle
    {
        protected override int HeavyHitHeal => 8;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 62;
            Item.height = 48;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 1, 20);
            Item.rare = ItemRarityID.Orange;

            Item.damage = 18;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.shoot = ModContent.ProjectileType<StarScytheProjectile>();
            Item.shootSpeed = 8f;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
