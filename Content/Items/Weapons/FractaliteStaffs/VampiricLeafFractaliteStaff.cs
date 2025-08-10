using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Projectiles.Weapon.FractaliteStaffProjectiles;
using Terraria.DataStructures;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Placeable;

namespace LifeStealClass.Content.Items.Weapons.FractaliteStaffs
{
    public class VampiricLeafFractaliteStaff : FractaliteWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 4);

            Item.width = 64;
            Item.height = 64;
            Item.scale = 1.1f;

            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item63;

            Item.crit = 27;

            Item.DamageType = ModContent.GetInstance<LifestealDamage>();
            Item.damage = 17;
            Item.knockBack = 3f;

            Item.shootSpeed = 7f;
            Item.shoot = ModContent.ProjectileType<VampiricLeafFractaliteStaffProjectile>();

            Item.GetGlobalItem<HealthCost>().healthCost = 24;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);

            Main.projectile[p].localAI[0] = 30f; // Homing-Speed
            Main.projectile[p].localAI[1] = 2f;  // Anzahl Split-Projektile

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.Stinger, 16);
            recipe1.AddIngredient(ItemID.JungleSpores, 20);
            recipe1.AddIngredient(ModContent.ItemType<LifeBar>(), 8);
            recipe1.AddIngredient(ItemID.ShadowScale, 6);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.Stinger, 16);
            recipe2.AddIngredient(ItemID.JungleSpores, 20);
            recipe2.AddIngredient(ModContent.ItemType<LifeBar>(), 8);
            recipe2.AddIngredient(ItemID.TissueSample, 6);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }

    }
}
