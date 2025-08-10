using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Projectiles.Weapon.FractaliteStaffProjectiles;
using Terraria.DataStructures;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Weapons.FractaliteStaffs
{
    public class FeatheredFractaliteStaff : FractaliteWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 50);

            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.UseSound = SoundID.Item63;

            Item.crit = 27;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 11;
            Item.knockBack = 3f;

            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<FeatheredFractaliteStaffProjectile>();

            Item.GetGlobalItem<HealthCost>().healthCost = 7;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

            Main.projectile[p].localAI[0] = 20f; // Homing-Speed
            Main.projectile[p].localAI[1] = 1f;  // Anzahl Split-Projektile

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.Feather, 12);
            recipe1.AddIngredient(ModContent.ItemType<LifeShard>(), 10);
            recipe1.AddTile(TileID.SkyMill);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.Feather, 12);
            recipe2.AddIngredient(ModContent.ItemType<LifeShard>(), 10);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}
