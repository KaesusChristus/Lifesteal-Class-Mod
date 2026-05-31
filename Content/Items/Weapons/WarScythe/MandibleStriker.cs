using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Projectiles.Weapon.WarScythe;
using LifeStealClass.Content.Core;
using LifeStealClass.Common.GlobalItems.Other;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Weapons.WarScythe
{
    public class MandibleStriker : LifestealWarScytheWeapon, IDashWeapon
    {
        public new float DashSpeed => 15f;
        public new int DashDuration => 10;
        public new int DashCooldown => 180;
        public new int DashDamageBonus => 40;
        public new int DashCritBonus => 16;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2);

            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.crit = 25;

            Item.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Item.damage = 12;
            Item.knockBack = 6.5f;

            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<MandibleStrikerProjectile>();

            Item.GetGlobalItem<HealthCost>().dashHealthCost = 12;

            var dashStats = Item.GetGlobalItem<DashBonusStats>();
            dashStats.dashDamageBonus = DashDamageBonus;
            dashStats.dashCritBonus = DashCritBonus;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AntlionMandible, 2);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 1);
            recipe.AddIngredient(ItemID.PalmWood, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
