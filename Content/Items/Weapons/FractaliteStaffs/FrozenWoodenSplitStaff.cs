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
    public class FrozenWoodenSplitStaff : FractaliteWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 1, 50);

            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.UseSound = SoundID.Item63;

            Item.crit = 12;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 7;
            Item.knockBack = 2f;

            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<FrozenWoodenSplitStaffProjectile>();

            Item.GetGlobalItem<HealthCost>().healthCost = 4;
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
            recipe.AddIngredient(ModContent.ItemType<WoodenSplitStaff>(), 1);
            recipe.AddIngredient(ItemID.IceTorch, 6);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
