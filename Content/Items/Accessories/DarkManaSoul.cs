using LifeStealClass.Common.ModPlayers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Items.Ingredients;


namespace LifeStealClass.Content.Items.Accessories
{
    public class DarkManaSoul : LifeStealItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // Registriere die Animation für dieses Item
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(20, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 10, 50);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DarkManaSoulEffect>().hasDarkManaSoulEquipped = true;
        }

        private int currentFrame = 0;
        private int frameCounter = 0;

        public override void PostUpdate()
        {
            frameCounter++; // Inkrementiere den Frame-Counter
            if (frameCounter >= 20) // 20 Ticks pro Frame
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % 4; // 4 Frames in der Animation
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            int frameHeight = texture.Height / 4; // Falls dein Spritesheet 4 Frames hat
            Rectangle sourceRectangle = new(0, currentFrame * frameHeight, texture.Width, frameHeight);

            Vector2 drawPosition = Item.Center - Main.screenPosition;
            spriteBatch.Draw(texture, drawPosition, sourceRectangle, lightColor, rotation, sourceRectangle.Size() / 2f, scale, SpriteEffects.None, 0f);

            return false; // Verhindert das Standard-Rendering, falls nötig
        }

        public override void UpdateInventory(Player player)
        {
            frameCounter++;
            if (frameCounter >= 20) // 20 Ticks pro Frame
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % 4; // 4 Frames
            }
        }


        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            int frameHeight = texture.Height / 4; // Falls dein Spritesheet 4 Frames hat

            // Terraria schneidet manchmal Frames falsch, deshalb manuell anpassen
            Rectangle sourceRectangle = new Rectangle(0, currentFrame * frameHeight + 1, texture.Width, frameHeight - 1);

            Vector2 adjustedPosition = position + new Vector2(0, 1); // Falls nötig, leicht nach unten verschieben

            spriteBatch.Draw(texture, adjustedPosition, sourceRectangle, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false; // Verhindert das Standard-Rendering
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ManaCrystal, 5);
            recipe.AddIngredient(ModContent.ItemType<GreaterLifeCrystal>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SoulOfBlood>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
