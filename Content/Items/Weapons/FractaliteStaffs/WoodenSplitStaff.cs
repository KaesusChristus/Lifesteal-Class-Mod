using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Projectiles.Weapon.FractaliteStaffProjectiles;
using Terraria.DataStructures;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Weapons.FractaliteStaffs
{
    public class WoodenSplitStaff : FractaliteWeapon
    {

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 1);

            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.UseSound = SoundID.Item63;

            Item.crit = 10;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 5;
            Item.knockBack = 2f;

            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<WoodenSplitStaffProjectile>();

            Item.GetGlobalItem<HealthCost>().healthCost = 3;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

            Main.projectile[p].localAI[1] = 0f;

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 12);
            recipe.AddIngredient(ItemID.Torch, 6);
            recipe.AddIngredient(ModContent.ItemType<BloodCells>(), 6);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
