using HarvesterClassMod.Common.GlobalItems.Other;
using HarvesterClassMod.Content.Items.Ingredients;
using HarvesterClassMod.Content.Projectiles.Weapon.Sickle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HarvesterClassMod.Content.Items.Weapons.Sickle
{
    public class TheSlaughtersScythe : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 54;
            Item.height = 44;
            Item.scale = 1.2f;

            Item.value = Item.sellPrice(0, 3, 40);
            Item.rare = ItemRarityID.Green;

            Item.damage = 18;

            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.shoot = ModContent.ProjectileType<TheSlaughtersScytheProjectile>();
            Item.shootSpeed = 8f;

            Item.GetGlobalItem<OnHitHeal>().baseHealOnHit = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 18);
            recipe.AddIngredient(ModContent.ItemType<LifeShard>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
