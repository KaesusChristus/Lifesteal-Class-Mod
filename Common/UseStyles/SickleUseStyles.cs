using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using LifeStealClass.Common.Utils;

namespace LifeStealClass.Common.UseStyles
{
    public class SickleUseStyles : GlobalItem
    {
        public override bool InstancePerEntity => true;

        // Animation State
        public int swingPhase = 0;
        public float swingTimer = 0f;
        public float swingAngle = 0f;

        private readonly float[] swingDurations = new float[] { 20f, 20f, 40f };

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

        public override bool? UseItem(Item item, Player player)
        {
            if (item.useStyle != CustomUseStyleID.SickleUseStyle)
                return false;

            swingPhase = 0;
            swingTimer = 0;
            swingAngle = MathHelper.PiOver4;

            int totalDuration = (int)(swingDurations[0] * 2 + swingDurations[1] * 2 + swingDurations[2] * 2);

            player.itemAnimation = totalDuration;
            player.itemTime = totalDuration;

            // Sound only once
            SoundEngine.PlaySound(SoundID.Item71 with { Volume = 0.7f }, player.Center);

            return true;
        }

        public override void HoldItem(Item item, Player player)
        {
            if (player.itemAnimation <= 0 || item.useStyle != CustomUseStyleID.SickleUseStyle)
                return;

            swingTimer++;

            if (swingPhase <= 1)
            {
                float duration = swingDurations[swingPhase] * 2f;
                float t = Math.Clamp(swingTimer / duration, 0f, 1f);

                float half = 0.5f;

                if (t < half)
                {
                    swingAngle = MathHelper.Lerp(
                        MathHelper.PiOver4,
                        -MathHelper.PiOver4,
                        Smooth(t / half)
                    );
                }
                else
                {
                    swingAngle = MathHelper.Lerp(
                        -MathHelper.PiOver4,
                        MathHelper.PiOver4,
                        Smooth((t - half) / half)
                    );
                }

                if (swingTimer >= duration)
                {
                    swingTimer = 0;
                    swingPhase++;
                }
            }
            else if (swingPhase == 2)
            {
                // Ausholen
                if (swingTimer < swingDurations[2])
                {
                    swingAngle = MathHelper.Lerp(
                        MathHelper.PiOver4,
                        MathHelper.PiOver2,
                        swingTimer / swingDurations[2]
                    );
                }
                else
                {
                    // Schneller Schwung
                    float fastT = (swingTimer - swingDurations[2]) / swingDurations[2];
                    swingAngle = MathHelper.Lerp(MathHelper.PiOver2, -MathHelper.PiOver2, Smooth(fastT));
                }

                if (swingTimer >= swingDurations[2] * 2)
                {
                    player.itemAnimation = 0;
                }
            }
        }

        private float Smooth(float t)
        {
            return t * t * (3f - 2f * t);
        }
    }
}
