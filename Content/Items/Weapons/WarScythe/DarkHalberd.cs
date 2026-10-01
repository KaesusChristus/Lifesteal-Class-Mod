using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Placeable;
using LifeStealClass.Content.Projectiles.Weapon.WarScythe;

namespace LifeStealClass.Content.Items.Weapons.WarScythe
{
    public class DarkHalberd : LifestealWarScytheWeapon
    {
        public override float DashSpeed => 20f;
        public override int DashDuration => 8;
        public override int DashCooldown => 180;
        public override int DashDamageBonus => 80;
        public override int DashCritBonus => 50;
        protected override int DashHealthCost => 25;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 5);

            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.crit = 25;

            Item.damage = 10;
            Item.knockBack = 6.5f;

            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<DarkHalberdProjectile>();

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
