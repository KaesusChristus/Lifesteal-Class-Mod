using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Collections.Generic;
using LifeStealClass.Content.Items.Armor.DarbBloodArmor;

namespace LifeStealClass.Common.ModPlayers.SetBoni
{
    public class DarkBloodArmorSetBonus : ModPlayer
    {
        public bool setBonusActive;
        private int timer;
        private List<Dust> dustList = new List<Dust>();
        private List<int> dustLifetimes = new List<int>();
        private float ringRadius;
        private int ringLifetime = 180; // Lebensdauer des Rings in Ticks
        private float initialRingRadius = 100f; // Anfangsradius des Rings
        private float radiusDecreaseRate = 5f; // Rate, mit der sich der Ringradius verkleinert

        public override void UpdateDead()
        {
            // Stelle sicher, dass die Variable zurückgesetzt wird, wenn der Spieler stirbt
            setBonusActive = false;
            timer = 0;
            dustList.Clear();
            dustLifetimes.Clear();
        }

        public override void PostUpdate()
        {
            if (WearsFullSet(Player))
            {
                setBonusActive = true;
            }
            else
            {
                setBonusActive = false;
                timer = 0; // Reset des Timers, wenn der Set-Bonus deaktiviert wird
            }

            if (setBonusActive)
            {
                timer++;
                if (timer >= 300) // 300 Ticks = 5 Sekunden
                {
                    CreateHealingRing();
                    timer = 0;
                }

                UpdateHealingRing();
            }
        }

        private bool WearsFullSet(Player player)
        {
            return player.armor[0].type == ModContent.ItemType<DarkBloodHelmet>() &&
                   player.armor[1].type == ModContent.ItemType<DarkBloodBreastplate>() &&
                   player.armor[2].type == ModContent.ItemType<DarkBloodLeggins>();
        }

        private void CreateHealingRing()
        {
            ringRadius = initialRingRadius;
            int dustType = DustID.RedTorch;

            for (int i = 0; i < 360; i += 10) // Erstelle Partikel alle 10 Grad
            {
                Vector2 position = Player.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i)) * ringRadius;
                Dust dust = Dust.NewDustPerfect(position, dustType, Vector2.Zero);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dustList.Add(dust);
                dustLifetimes.Add(ringLifetime);
            }

            Player localPlayer = Main.LocalPlayer;

            localPlayer.GetModPlayer<LifestealEffectsPlayer>().SetHealAmount(12);
        }

        private void UpdateHealingRing()
        {
            ringRadius -= radiusDecreaseRate; // Verkleinere den Ringradius im Laufe der Zeit

            for (int i = 0; i < dustList.Count; i++)
            {
                Dust dust = dustList[i];
                int lifetime = dustLifetimes[i] - 1;
                dustLifetimes[i] = lifetime;

                if (lifetime > 0)
                {
                    dust.position = Player.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 10)) * ringRadius;
                }
                else
                {
                    dust.active = false;
                }
            }

            // Entferne inaktive Partikel aus der Liste
            for (int i = dustList.Count - 1; i >= 0; i--)
            {
                if (!dustList[i].active)
                {
                    dustList.RemoveAt(i);
                    dustLifetimes.RemoveAt(i);
                }
            }
        }
    }
}
