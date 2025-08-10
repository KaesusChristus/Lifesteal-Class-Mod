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
    public class PlatinFractaliteStaff : FractaliteWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 50);

            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item28;

            Item.crit = 10;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 11;
            Item.knockBack = 3f;

            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<PlatinFractaliteStaffProjectile>();

            Item.GetGlobalItem<HealthCost>().healthCost = 9;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

            Main.projectile[p].localAI[1] = 1f;

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 16);
            recipe.AddIngredient(ItemID.Ruby, 2);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
