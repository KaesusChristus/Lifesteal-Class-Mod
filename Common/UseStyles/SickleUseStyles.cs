using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Common.UseStyles
{
    public class SickleUseStyles : GlobalItem
    {
        private int swingPhase = 0;
        private float swingTimer = 0f;
        private readonly float[] swingDurations = new float[3] { 20f, 20f, 40f }; // Phase 0+1 schnell, Phase 2 langsamer
        private float swingAngle = 0f;

        private bool playedChargeSound = false;
        private bool playedSwingSound = false;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.useStyle == CustomUseStyleID.SickleUseStyle;
        }

        public override void SetDefaults(Item item)
        {
            if (item.useStyle == CustomUseStyleID.SickleUseStyle)
            {
                item.noUseGraphic = true;
                item.noMelee = true;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return player.itemAnimation == 0 && item.useStyle == CustomUseStyleID.SickleUseStyle;
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (item.useStyle == CustomUseStyleID.SickleUseStyle)
            {
                swingPhase = 0;
                swingTimer = 0f;
                swingAngle = MathHelper.PiOver4;
                playedChargeSound = false;
                playedSwingSound = false;

                // Gesamtdauer: Summe aller Phasen
                int totalDuration = 0;
                foreach (float dur in swingDurations)
                    totalDuration += (int)dur;
                totalDuration *= 2; // Weil erste 2 Phasen je 2x dauern (runter+hoch)
                totalDuration += (int)(swingDurations[2] * 2); // 3. Phase auch 2x (ausholen + schnell)

                player.itemAnimation = totalDuration;
                player.itemTime = player.itemAnimation;

                return true;
            }
            return false;
        }

        public override void HoldItem(Item item, Player player)
        {
            if (item.useStyle != CustomUseStyleID.SickleUseStyle || player.itemAnimation <= 0)
                return;

            swingTimer++;

            // Phase 0 + 1: Kleine Schwünge jeweils 2x duration (runter + hoch)
            if (swingPhase == 0 || swingPhase == 1)
            {
                float totalDuration = swingDurations[swingPhase] * 2f;
                float phaseT = swingTimer / totalDuration;
                if (phaseT > 1f) phaseT = 1f;

                if (swingTimer == 1)
                {
                    SoundEngine.PlaySound(SoundID.Item71, player.Center);
                }

                if (phaseT <= 0.5f)
                {
                    float subT = phaseT / 0.5f;
                    swingAngle = MathHelper.Lerp(MathHelper.PiOver4, -MathHelper.PiOver4, SmoothEase(subT));
                }
                else
                {
                    float subT = (phaseT - 0.5f) / 0.5f;
                    swingAngle = MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, SmoothEase(subT));
                }

                if (swingTimer >= totalDuration)
                {
                    swingTimer = 0f;
                    swingPhase++;
                    playedChargeSound = false;
                    playedSwingSound = false;
                }
            }
            else if (swingPhase == 2)
            {
                if (swingTimer < swingDurations[2])
                {
                    if (!playedChargeSound)
                    {
                        SoundEngine.PlaySound(SoundID.Item78, player.Center);
                        playedChargeSound = true;
                    }
                    swingAngle = MathHelper.Lerp(MathHelper.PiOver4, MathHelper.PiOver2, swingTimer / swingDurations[2]);
                }
                else
                {
                    if (!playedSwingSound)
                    {
                        SoundEngine.PlaySound(SoundID.Item71, player.Center);
                        playedSwingSound = true;
                    }
                    float fastT = (swingTimer - swingDurations[2]) / swingDurations[2];
                    if (fastT > 1f) fastT = 1f;

                    swingAngle = MathHelper.Lerp(MathHelper.PiOver2, -MathHelper.PiOver2, SmoothEase(fastT));

                    if (swingTimer >= swingDurations[2] * 2)
                    {
                        player.itemAnimation = 0;
                    }
                }
            }
        }

        private float SmoothEase(float t)
        {
            return t * t * (3f - 2f * t);
        }

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (item.useStyle != CustomUseStyleID.SickleUseStyle)
                return true;

            Vector2 position = item.Center - Main.screenPosition;
            SpriteEffects effects = SpriteEffects.None;

            if (swingPhase == 1 && swingTimer < swingDurations[1])
                effects = SpriteEffects.FlipVertically;

            if (swingPhase == 2)
                scale = 1.5f;

            spriteBatch.Draw(
                TextureAssets.Item[item.type].Value,
                position,
                null,
                lightColor,
                swingAngle,
                new Vector2(item.width / 2, item.height / 2),
                scale,
                effects,
                0f);

            return false;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.useStyle == CustomUseStyleID.SickleUseStyle)
            {
                if (swingPhase == 2)
                    damage *= 1.5f;
            }
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (item.useStyle == CustomUseStyleID.SickleUseStyle)
            {
                if (swingPhase == 2)
                    crit += 10f;
            }
        }
    }
}
