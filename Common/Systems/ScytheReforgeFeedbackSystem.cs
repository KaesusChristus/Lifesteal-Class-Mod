using LifeStealClass.Content.Prefixes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Common.Systems
{
    public sealed class ScytheReforgeFeedbackSystem : ModSystem
    {
        private static readonly SoundStyle BestReforgeSound = new(
            "Terraria/Sounds/Custom/best_reforge");

        private static Item lockedItem;
        private static bool pendingRainbowPopup;

        public static bool CanReforge(Item item)
        {
            return !ReferenceEquals(item, lockedItem);
        }

        public static void RegisterBestReforge(Item item)
        {
            lockedItem = item;
            pendingRainbowPopup = true;

            SoundEngine.PlaySound(BestReforgeSound);
            SpawnBestReforgeDust(item.Center);
        }

        public static void RegisterSoulforged(Item item)
        {
            RegisterBestReforge(item);
        }

        public override void PostUpdateEverything()
        {
            ApplyRainbowToReforgePopup();

            if (lockedItem != null
                && (!Main.InReforgeMenu
                    || !Main.mouseReforge
                    || !ReferenceEquals(Main.reforgeItem, lockedItem)))
            {
                lockedItem = null;
            }
        }

        public override void Unload()
        {
            lockedItem = null;
            pendingRainbowPopup = false;
        }

        private static void ApplyRainbowToReforgePopup()
        {
            if (!pendingRainbowPopup || lockedItem == null)
                return;

            PopupText newestMatchingPopup = null;
            foreach (PopupText popup in Main.popupText)
            {
                if (popup.active
                    && popup.context == PopupTextContext.ItemReforge
                    && popup.name == lockedItem.AffixName()
                    && (newestMatchingPopup == null
                        || popup.lifeTime > newestMatchingPopup.lifeTime))
                {
                    newestMatchingPopup = popup;
                }
            }

            if (newestMatchingPopup == null)
                return;

            newestMatchingPopup.rarity = ModContent.RarityType<SoulforgedRarity>();
            newestMatchingPopup.color = Main.DiscoColor;
            pendingRainbowPopup = false;
        }

        private static void SpawnBestReforgeDust(Vector2 center)
        {
            const int dustCount = 28;
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 velocity = (MathHelper.TwoPi * i / dustCount)
                    .ToRotationVector2()
                    * Main.rand.NextFloat(2.5f, 5.5f);
                Dust dust = Dust.NewDustPerfect(
                    center,
                    DustID.RainbowTorch,
                    velocity,
                    60,
                    Main.DiscoColor,
                    1.35f);
                dust.noGravity = true;
            }
        }
    }
}
