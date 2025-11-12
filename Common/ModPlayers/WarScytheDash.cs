using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using System;
using LifeStealClass.Common.GlobalItems.Other;

namespace LifeStealClass.Common.ModPlayers
{
    public class WarScytheDash : ModPlayer
    {
        private int dashTimer;
        private int dashCooldownTimer;
        private Vector2 dashVelocity;

        private int? activeSpearProjType;
        private float activeSpearSpeed;


        private int dashDamageBonus;
        private int dashCritBonus;

        private int lastDashCooldown = 0;

        public void TryDash(Vector2 direction, Item item, int? spearProjectileType = null, float spearSpeed = 0f)
        {
            if (dashCooldownTimer > 0 || dashTimer > 0)
                return;

            if (!(item.ModItem is IDashWeapon dashItem)) return; // nur DashWeapons

            // 🔹 DashHealthCost prüfen
            int dashCost = item.GetGlobalItem<HealthCost>().dashHealthCost;
            if (dashCost > 0)
            {
                int reduction = 0;
                if (Player.TryGetModPlayer(out LifestealEffectsPlayer modPlayer))
                    reduction = modPlayer.reduceLifecostFlat;

                int adjustedCost = Math.Max(0, dashCost - reduction);
                if (Player.statLife <= adjustedCost) return; // nicht genug HP
                Player.statLife -= adjustedCost;
                if (Main.netMode != NetmodeID.Server)
                    CombatText.NewText(Player.getRect(), Color.Red, $"-{adjustedCost}");
            }

            // 🔹 Dash starten
            dashVelocity = direction.SafeNormalize(Vector2.Zero) * dashItem.DashSpeed;
            dashTimer = dashItem.DashDuration;
            dashCooldownTimer = dashItem.DashCooldown;

            activeSpearProjType = spearProjectileType;
            activeSpearSpeed = spearSpeed;

            SoundEngine.PlaySound(SoundID.Item9, Player.position);

            // Bonuswerte merken
            dashDamageBonus = dashItem.DashDamageBonus;
            dashCritBonus = dashItem.DashCritBonus;

            // Visual Dash-Effekte
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke,
                    dashVelocity.X * 0.2f, dashVelocity.Y * 0.2f);

            // Projektil spawn, falls vorhanden
            if (Main.myPlayer == Player.whoAmI && activeSpearProjType.HasValue)
            {
                int projType = activeSpearProjType.Value;
                float speed = activeSpearSpeed;
                Vector2 velocity = dashVelocity.SafeNormalize(Vector2.Zero) * speed;

                var source = Player.HeldItem?.IsAir == false
                    ? Player.GetSource_ItemUse(Player.HeldItem)
                    : Player.GetSource_FromThis();

                int proj = Projectile.NewProjectile(
                    source,
                    Player.Center,
                    velocity,
                    projType,
                    Player.GetWeaponDamage(Player.HeldItem) + dashDamageBonus,
                    Player.whoAmI,
                    0,
                    dashCritBonus
                );

                Projectile p = Main.projectile[proj];
                Player.heldProj = proj;
                p.timeLeft = 60;
                p.netUpdate = true;

                Player.itemTime = Player.itemTimeMax = 20;
                Player.itemAnimation = Player.itemAnimationMax = 20;
                Player.itemRotation = (float)Math.Atan2(velocity.Y * Player.direction, velocity.X * Player.direction);
                Player.SetDummyItemTime(20);
            }
        }

        public override void PreUpdateMovement()
        {
            if (dashCooldownTimer > 0)
                dashCooldownTimer--;

            if (dashTimer > 0)
            {
                Player.velocity = dashVelocity;
                dashTimer--;

                Player.direction = dashVelocity.X > 0 ? 1 : -1;

                Player.immune = true;
                Player.immuneTime = dashTimer + 60;

                Player.noKnockback = true;

                if (dashTimer <= 0)
                {
                    Player.velocity *= 0.5f;

                    Player.noKnockback = false;
                }
            }
        }

        public override void PostUpdate()
        {
            // Cooldown gerade abgelaufen
            if (lastDashCooldown > 0 && dashCooldownTimer == 0)
            {
                CombatText.NewText(Player.getRect(), Color.LimeGreen, "Dash is Ready!");
            }

            lastDashCooldown = dashCooldownTimer;
        }

    }
}