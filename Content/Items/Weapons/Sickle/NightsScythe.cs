using LifeStealClass.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using LifeStealClass.Content.Items.Ingredients;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class NightsScythe : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 14, 50);
            Item.rare = ItemRarityID.Orange;

            Item.damage = 33;

            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.shoot = ModContent.ProjectileType<NightsScytheProjectile>();
            Item.shootSpeed = 12f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 2;
        }

        public override bool? UseItem(Player player)
        {
            Vector2 toMouse = Main.MouseWorld - player.Center;
            int direction = toMouse.X >= 0 ? 1 : -1;

            Vector2 spawnOffset = new Vector2(40f * direction, 0f);
            Vector2 spawnPos = player.Center + spawnOffset;
            Vector2 velocity = new Vector2(direction, 0f);

            Projectile proj1 = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), spawnPos, velocity, ProjectileID.NightsEdge, Item.damage / 2, Item.knockBack, player.whoAmI, direction, 25f, 1.2f);
            proj1.DamageType = ModContent.GetInstance<LifestealDamage>();
            Projectile proj2 = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), spawnPos, velocity, ProjectileID.NightsEdge, Item.damage / 2, Item.knockBack, player.whoAmI, direction * 0.1f, 30f, 1.3f);
            proj2.DamageType = ModContent.GetInstance<LifestealDamage>();

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<ShadowsWisdom>());
            recipe1.AddIngredient(ModContent.ItemType<AquaScythe>());
            recipe1.AddIngredient(ModContent.ItemType<OvergrowthScythe>());
            recipe1.AddIngredient(ModContent.ItemType<LavaSickle>());
            recipe1.AddTile(TileID.DemonAltar);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<TheSlaughtersScythe>());
            recipe2.AddIngredient(ModContent.ItemType<AquaScythe>());
            recipe2.AddIngredient(ModContent.ItemType<OvergrowthScythe>());
            recipe2.AddIngredient(ModContent.ItemType<LavaSickle>());
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.Register();
        }
    }
}
