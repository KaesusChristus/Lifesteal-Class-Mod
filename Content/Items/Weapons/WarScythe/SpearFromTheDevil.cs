using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Projectiles.Weapon.WarScythe;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Weapons.WarScythe
{
    public class SpearFromTheDevil : LifestealWarScytheWeapon
    {
        public override float DashSpeed => 23f;
        public override int DashDuration => 9;
        public override int DashCooldown => 600;
        public override int DashDamageBonus => 120;
        public override int DashCritBonus => 70;
        protected override int DashHealthCost => 34;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 15);

            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.crit = 20;

            Item.damage = 25;
            Item.knockBack = 3f;

            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<SpearFromTheDevilProjectile>();

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
