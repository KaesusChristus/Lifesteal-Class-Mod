using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using ReLogic.Graphics;

namespace LifeStealClass.Common.ModPlayers
{
    public class WarScytheDashLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.LastVanillaLayer);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            var dash = player.GetModPlayer<WarScytheDash>();

            if (dash == null)
                return;

            if (dash.MaxDashCooldown <= 0)
                return;

            if (dash.DashCooldownRemaining <= 0)
                return;

            float progress = 1f - (float)dash.DashCooldownRemaining / dash.MaxDashCooldown;

            Vector2 screenPos = player.Top - Main.screenPosition + new Vector2(0, 48f);

            SpriteBatch sb = Main.spriteBatch;

            // 🔹 BAR SETTINGS
            int width = 60;
            int height = 6;

            Vector2 barPos = screenPos - new Vector2(width / 2f, 0);

            // BACKGROUND
            DrawRect(sb, barPos, width, height, Color.Black * 0.6f);

            // FILLED BAR
            DrawRect(sb, barPos, (int)(width * progress), height,
                Color.Lerp(Color.Red, Color.LimeGreen, progress));

            // BORDER
            DrawRect(sb, barPos, width, height, Color.White * 0.8f, true);

            // 🔥 PERCENT TEXT (unter der Bar)
            string percentText = $"{(int)(progress * 100f)}%";

            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(percentText);
            Vector2 textPos = screenPos + new Vector2(-textSize.X / 2f, 8f);

            Color textColor = Color.Lerp(Color.Red, Color.LimeGreen, progress);

            // Border für Lesbarkeit
            DrawText(sb, percentText, textPos + new Vector2(1, 0), Color.Black);
            DrawText(sb, percentText, textPos + new Vector2(-1, 0), Color.Black);
            DrawText(sb, percentText, textPos + new Vector2(0, 1), Color.Black);
            DrawText(sb, percentText, textPos + new Vector2(0, -1), Color.Black);

            DrawText(sb, percentText, textPos, textColor);
        }

        private void DrawRect(SpriteBatch sb, Vector2 pos, int width, int height, Color color, bool outline = false)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.MagicPixel.Value;

            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);

            if (!outline)
            {
                sb.Draw(tex, rect, color);
            }
            else
            {
                sb.Draw(tex, new Rectangle(rect.X, rect.Y, rect.Width, 1), color);
                sb.Draw(tex, new Rectangle(rect.X, rect.Bottom, rect.Width, 1), color);
                sb.Draw(tex, new Rectangle(rect.X, rect.Y, 1, rect.Height), color);
                sb.Draw(tex, new Rectangle(rect.Right, rect.Y, 1, rect.Height), color);
            }
        }

        private void DrawText(SpriteBatch sb, string text, Vector2 pos, Color color)
        {
            sb.DrawString(FontAssets.MouseText.Value, text, pos, color);
        }
    }
}