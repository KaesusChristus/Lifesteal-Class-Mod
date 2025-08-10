using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Placeable;
using LifeStealClass.Content.Projectiles.Weapon.Spears;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.GlobalItems.Other;
namespace LifeStealClass.Content.Items.Weapons.Spears
{
    public class DarkHalberd : LifestealSpearWeapon
    {

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 5);

            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.crit = 25;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 10;
            Item.knockBack = 6.5f;

            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<DarkHalberdProjectile>();

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 12);
            recipe1.AddIngredient(ItemID.TissueSample, 8);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<LifeBar>(), 12);
            recipe2.AddIngredient(ItemID.ShadowScale, 8);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}