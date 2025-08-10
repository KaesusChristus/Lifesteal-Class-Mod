using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Items.Ingredients;
using Microsoft.Xna.Framework;
using Terraria.Social.Base;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Content.Items.Weapons.Sickle
{
    public class OvergrowthScythe : LifestealSickle
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.sellPrice(0, 5, 30);

            Item.damage = 13;

            Item.useTime = 25;
            Item.useAnimation = 25;
            
            Item.shoot = ProjectileID.BladeOfGrass;
            Item.shootSpeed = 5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 120);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vine, 4);
            recipe.AddIngredient(ItemID.JungleSpores, 12);
            recipe.AddIngredient(ModContent.ItemType<BloodyStinger>(), 14);
            recipe.Register();
        }
    }
}
