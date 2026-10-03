using System;
using LifeStealClass.Common.ModPlayers;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Prefixes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.WarScythe
{
    public abstract class BaseSpearProjectile : ModProjectile
    {
        private enum AttackStage
        {
            Thrust,
            Swing,
            Recover
        }

        private ref float AimAngle => ref Projectile.ai[0];
        private ref float Timer => ref Projectile.localAI[0];

        private bool IsDashAttack => Projectile.ai[2] == 1f;
        private Player Owner => Main.player[Projectile.owner];
        private WarScytheStats Stats => WarScythePrefixPool.ApplyAnimationStats(
            Owner.HeldItem,
            GetStats());
        private float AttackSpeed => Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float gripInset;
        private AttackStage currentStage;

        protected Vector2 AttackDirection => Projectile.rotation.ToRotationVector2();
        protected Vector2 WeaponTip => Projectile.Center
            + AttackDirection * (Stats.WeaponLength - gripInset) * Projectile.scale;

        public virtual WarScytheStats GetStats()
        {
            return new WarScytheStats
            {
                ThrustTime = 8f,
                SwingTime = 9f,
                RecoverTime = 5f,
                SwingArc = MathHelper.ToRadians(75f),
                ThrustStartGrip = 80f,
                ThrustEndGrip = 40f,
                SwingGrip = 60f,
                RecoverGrip = 80f,
                DashGrip = 60f,
                WeaponLength = 170f,
                HitboxWidth = 18f,
                Scale = 1f,
                RotationOffsetRight = MathHelper.ToRadians(45f),
                RotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            AimAngle = direction.ToRotation();
            Projectile.spriteDirection = direction.X >= 0f ? 1 : -1;
            Projectile.velocity = Vector2.Zero;
            gripInset = IsDashAttack ? Stats.DashGrip : Stats.ThrustStartGrip;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            Projectile.timeLeft = 2;

            if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }

            Owner.itemAnimation = 2;
            Owner.itemTime = 2;

            if (IsDashAttack)
            {
                UpdateDashAttack();
            }
            else
            {
                UpdateNormalAttack();
            }

            if (!Projectile.active)
                return;

            SetWeaponPosition();
            SpawnDust();
            Timer++;
        }

        public override bool? CanDamage()
        {
            if (IsDashAttack)
                return true;

            return currentStage == AttackStage.Swing;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects effects = Projectile.spriteDirection > 0
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;
            Vector2 origin = GetGripOrigin(texture);
            float rotationOffset = Projectile.spriteDirection > 0
                ? Stats.RotationOffsetRight
                : Stats.RotationOffsetLeft;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor * Projectile.Opacity,
                Projectile.rotation + rotationOffset,
                origin,
                Projectile.scale,
                effects);

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center;
            Vector2 end = WeaponTip;
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                Stats.HitboxWidth * Projectile.scale,
                ref collisionPoint);
        }

        public override void CutTiles()
        {
            Utils.PlotTileLine(
                Projectile.Center,
                WeaponTip,
                Stats.HitboxWidth * Projectile.scale,
                DelegateMethods.CutTiles);
        }

        private void UpdateDashAttack()
        {
            if (!Owner.GetModPlayer<WarScytheDash>().IsDashing)
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation = AimAngle;
            gripInset = Stats.DashGrip;
        }

        private void UpdateNormalAttack()
        {
            AttackStage stage = GetAttackStage(out float progress);
            currentStage = stage;
            float swingDirection = -Projectile.spriteDirection;
            float swingEndRotation = swingDirection * Stats.SwingArc;

            switch (stage)
            {
                case AttackStage.Thrust:
                    Projectile.rotation = AimAngle;
                    gripInset = MathHelper.SmoothStep(
                        Stats.ThrustStartGrip,
                        Stats.ThrustEndGrip,
                        progress);
                    break;

                case AttackStage.Swing:
                    Projectile.rotation = AimAngle
                        + MathHelper.SmoothStep(0f, swingEndRotation, progress);
                    gripInset = MathHelper.SmoothStep(
                        Stats.ThrustEndGrip,
                        Stats.SwingGrip,
                        progress);
                    break;

                default:
                    Projectile.rotation = AimAngle + swingEndRotation;
                    gripInset = MathHelper.SmoothStep(
                        Stats.SwingGrip,
                        Stats.RecoverGrip,
                        progress);

                    if (progress >= 1f)
                    {
                        Projectile.Kill();
                    }
                    break;
            }
        }

        private AttackStage GetAttackStage(out float progress)
        {
            float thrustTime = Math.Max(1f, Stats.ThrustTime / AttackSpeed);
            float swingTime = Math.Max(1f, Stats.SwingTime / AttackSpeed);
            float recoverTime = Math.Max(1f, Stats.RecoverTime / AttackSpeed);

            if (Timer < thrustTime)
            {
                progress = MathHelper.Clamp(Timer / thrustTime, 0f, 1f);
                return AttackStage.Thrust;
            }

            if (Timer < thrustTime + swingTime)
            {
                progress = MathHelper.Clamp(
                    (Timer - thrustTime) / swingTime,
                    0f,
                    1f);
                return AttackStage.Swing;
            }

            progress = MathHelper.Clamp(
                (Timer - thrustTime - swingTime) / recoverTime,
                0f,
                1f);
            return AttackStage.Recover;
        }

        private Vector2 GetGripOrigin(Texture2D texture)
        {
            Vector2 sourceHandle = new Vector2(texture.Width, texture.Height);
            Vector2 directionToBlade = -sourceHandle.SafeNormalize(Vector2.UnitX);
            Vector2 heldPoint = sourceHandle + directionToBlade * gripInset;

            if (Projectile.spriteDirection > 0)
            {
                return new Vector2(texture.Width - heldPoint.X, heldPoint.Y);
            }

            return heldPoint;
        }

        private void SetWeaponPosition()
        {
            Owner.direction = Projectile.spriteDirection;
            float armRotation = Projectile.rotation - MathHelper.PiOver2;
            Owner.SetCompositeArmFront(
                true,
                Player.CompositeArmStretchAmount.Full,
                armRotation);

            Vector2 armPosition = Owner.GetFrontHandPosition(
                Player.CompositeArmStretchAmount.Full,
                armRotation);
            armPosition.Y += Owner.gfxOffY;

            Projectile.Center = armPosition;
            Projectile.scale = Stats.Scale;
            Owner.heldProj = Projectile.whoAmI;
        }

        public virtual void SpawnDust() { }
    }
}
