using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LifeStealClass.Common.Utils
{
    public static class LifestealHelper
    {
        public static void MakeDust(Vector2 position, int width, int height, int dustID, float scale = 1.2f)
        {
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(position, width, height, dustID, 0f, 0f, 150, default, scale);
                Main.dust[d].velocity *= 0.5f;
                Main.dust[d].noGravity = true;
            }
        }
    }
}
