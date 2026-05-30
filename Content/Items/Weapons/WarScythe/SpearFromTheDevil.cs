using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Projectiles.Weapon.WarScythe;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Weapons.WarScythe
{
    public class SpearFromTheDevil : LifestealWarScytheWeapon, IDashWeapon
    {
        public new float DashSpeed => 23f;
        public new int DashDuration => 9;
        public new int DashCooldown => 600;
        public new int DashDamageBonus => 120;
        public new int DashCritBonus => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 15);

            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.crit = 20;

            Item.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Item.damage = 25;
            Item.knockBack = 3f;

            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<SpearFromTheDevilProjectile>();

            Item.GetGlobalItem<HealthCost>().dashHealthCost = 34;
            var dashStats = Item.GetGlobalItem<DashBonusStats>();
            dashStats.dashDamageBonus = DashDamageBonus;
            dashStats.dashCritBonus = DashCritBonus;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DarkHalberd>());
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 20);
            recipe.AddIngredient(ModContent.ItemType<BloodyVeins>(), 10);
            recipe.AddIngredient(ModContent.ItemType<SoulOfBlood>(), 10);
            //recipe.AddIngredient(ModContent.ItemType<AquaHalberd>());
            //recipe.AddIngredient(ModContent.ItemType<VampiricLeafHalberd>());
            //recipe.AddIngredient(ModContent.ItemType<MoltenHalberd>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}