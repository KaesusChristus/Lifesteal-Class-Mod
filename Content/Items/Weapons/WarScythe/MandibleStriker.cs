using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Projectiles.Weapon.WarScythe;
using LifeStealClass.Content.Items.Ingredients;

namespace LifeStealClass.Content.Items.Weapons.WarScythe
{
    public class MandibleStriker : LifestealWarScytheWeapon
    {
        public override float DashSpeed => 15f;
        public override int DashDuration => 10;
        public override int DashCooldown => 180;
        public override int DashDamageBonus => 40;
        public override int DashCritBonus => 16;
        protected override int DashHealthCost => 12;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2);

            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.crit = 25;

            Item.damage = 12;
            Item.knockBack = 6.5f;

            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<MandibleStrikerProjectile>();

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
