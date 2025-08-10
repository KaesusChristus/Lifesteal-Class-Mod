using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Projectiles.Weapon.FractaliteStaffProjectiles;
using Terraria.DataStructures;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Placeable;

namespace LifeStealClass.Content.Items.Weapons.FractaliteStaffs
{
    public class DarkbloodFractaliteStaff : FractaliteWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 5, 50);

            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item28;

            Item.crit = 8;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 12;
            Item.knockBack = 3f;

            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<DarkbloodFractaliteStaffProjectile>();

            Item.GetGlobalItem<HealthCost>().healthCost = 14;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

            Main.projectile[p].localAI[1] = 2f;

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 12);
            recipe1.AddIngredient(ModContent.ItemType<LifeShard>(), 6);
            recipe1.AddIngredient(ItemID.ShadowScale, 8);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<LifeBar>(), 12);
            recipe2.AddIngredient(ModContent.ItemType<LifeShard>(), 6);
            recipe2.AddIngredient(ItemID.TissueSample, 8);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }

    }
}
