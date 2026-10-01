using System;
using System.IO;
using LifeStealClass.Content.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public abstract class LifestealSickleProjectile : ModProjectile
    {
        protected enum AttackStage
        {
            Prepare,
            Execute,
            Recover
        }

        private readonly struct AttackProfile
        {
            public AttackProfile(
                float startDegrees,
                float endDegrees,
                float windupDegrees,
                float prepareMultiplier,
                float executeMultiplier,
                float recoveryMultiplier)
            {
                StartAngle = MathHelper.ToRadians(startDegrees);
                EndAngle = MathHelper.ToRadians(endDegrees);
                WindupAngle = MathHelper.ToRadians(windupDegrees);
                PrepareMultiplier = prepareMultiplier;
                ExecuteMultiplier = executeMultiplier;
                RecoveryMultiplier = recoveryMultiplier;
            }

            public float StartAngle { get; }
            public float EndAngle { get; }
            public float WindupAngle { get; }
            public float PrepareMultiplier { get; }
            public float ExecuteMultiplier { get; }
            public float RecoveryMultiplier { get; }
        }

        private float visualScale;

        protected SickleAttackType CurrentAttack => (SickleAttackType)Projectile.ai[0];
        protected Player Owner => Main.player[Projectile.owner];

        private ref float AimAngle => ref Projectile.ai[1];
        private ref float Timer => ref Projectile.localAI[0];

        private AttackStage CurrentStage
        {
            get => (AttackStage)Projectile.localAI[1];
            set
            {
                Projectile.localAI[1] = (float)value;
                Timer = 0f;
            }
        }

        public virtual SickleStats GetStats()
        {
            return new SickleStats
            {
                PrepTime = 12f,
                ExecTime = 8f,
                HideTime = 12f,
                scale = 1.2f,
                hitboxWidth = 15f,
                rotationOffsetRight = MathHelper.ToRadians(45f),
                rotationOffsetLeft = MathHelper.ToRadians(135f)
            };
        }

        private SickleStats Stats => GetStats();
        private AttackProfile Profile => GetAttackProfile(CurrentAttack);
        private float AttackSpeed => Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float PrepTime => Math.Max(1f, Stats.PrepTime * Profile.PrepareMultiplier / AttackSpeed);
        private float ExecTime => Math.Max(1f, Stats.ExecTime * Profile.ExecuteMultiplier / AttackSpeed);
        private float RecoverTime => Math.Max(1f, Stats.HideTime * Profile.RecoveryMultiplier / AttackSpeed);

        public override string Texture => "Terraria/Images/Item_" + ItemID.DirtBlock;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
            AimAngle = Projectile.velocity.SafeNormalize(Vector2.UnitX).ToRotation();
            Projectile.velocity = Vector2.Zero;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((sbyte)Projectile.spriteDirection);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.spriteDirection = reader.ReadSByte();
        }

        public override void AI()
        {
            Projectile.timeLeft = 2;
            Owner.itemAnimation = 2;
            Owner.itemTime = 2;

            if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }

            switch (CurrentStage)
            {
                case AttackStage.Prepare:
                    PrepareStrike();
                    break;
                case AttackStage.Execute:
                    ExecuteStrike();
                    break;
                default:
                    RecoverStrike();
                    break;
            }

            SetWeaponPosition();
            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = GetDrawOrigin(texture);
            SpriteEffects effects = GetSpriteEffects();
            float rotationOffset = GetRotationOffset();

            if (CurrentAttack == SickleAttackType.HeavySlash && CurrentStage != AttackStage.Prepare)
            {
                int trailLength = Math.Min((int)Timer, 5);
                for (int i = trailLength; i >= 1; i--)
                {
                    float strength = (trailLength - i + 1f) / (trailLength + 1f);
                    Color trailColor = new Color(180, 25, 35, 0) * (0.3f * strength);
                    Main.EntitySpriteDraw(
                        texture,
                        Projectile.Center - Main.screenPosition,
                        null,
                        trailColor,
                        Projectile.oldRot[i] + rotationOffset,
                        origin,
                        Projectile.scale,
                        effects);
                }
            }

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
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * GetWeaponReach();
            float collisionPoint = 0f;
            float widthMultiplier = CurrentAttack == SickleAttackType.HeavySlash ? 1.25f : 1f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                Stats.hitboxWidth * Projectile.scale * widthMultiplier,
                ref collisionPoint);
        }

        public override void CutTiles()
        {
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * GetWeaponReach();
            Utils.PlotTileLine(
                start,
                end,
                Stats.hitboxWidth * Projectile.scale,
                DelegateMethods.CutTiles);
        }

        public override bool? CanDamage()
        {
            return CurrentStage == AttackStage.Execute;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = target.Center.X > Owner.Center.X ? 1 : -1;

            if (CurrentAttack == SickleAttackType.QuickSlash)
            {
                modifiers.FinalDamage *= 0.9f;
            }
            else if (CurrentAttack == SickleAttackType.HeavySlash)
            {
                modifiers.FinalDamage *= 1.65f;
                modifiers.Knockback *= 1.6f;
            }
        }

        protected virtual void OnAttackStarted()
        {
        }

        protected void SpawnSpectralEcho(int itemType, int variant)
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 direction = GetAimRotation().ToRotationVector2();
            float damageMultiplier = CurrentAttack == SickleAttackType.HeavySlash ? 0.9f : 0.55f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Owner.MountedCenter + direction * 24f,
                direction * 8.5f,
                ModContent.ProjectileType<SpectralScytheEchoProjectile>(),
                Math.Max(1, (int)(Projectile.damage * damageMultiplier)),
                Projectile.knockBack * 0.65f,
                Projectile.owner,
                itemType,
                variant,
                CurrentAttack == SickleAttackType.HeavySlash ? 1f : 0f);
        }

        protected Vector2 GetAttackDirection()
        {
            return GetAimRotation().ToRotationVector2();
        }

        private static AttackProfile GetAttackProfile(SickleAttackType attack)
        {
            return attack switch
            {
                SickleAttackType.QuickSlash => new AttackProfile(-65f, 55f, 20f, 0.6f, 0.75f, 0.6f),
                SickleAttackType.FollowupSlash => new AttackProfile(-65f, 55f, 20f, 0.6f, 0.75f, 0.6f),
                _ => new AttackProfile(-145f, 100f, 40f, 1.3f, 1.45f, 1.2f)
            };
        }

        private void PrepareStrike()
        {
            float swingDirection = Math.Sign(Profile.EndAngle - Profile.StartAngle);
            float windupStart = Profile.StartAngle - swingDirection * Profile.WindupAngle;
            float progress = MathHelper.Clamp(Timer / PrepTime, 0f, 1f);

            Projectile.rotation = GetWorldRotation(
                MathHelper.SmoothStep(windupStart, Profile.StartAngle, progress));
            visualScale = MathHelper.SmoothStep(0.35f, 1f, progress);

            if (Timer >= PrepTime)
            {
                CurrentStage = AttackStage.Execute;
                Projectile.ResetLocalNPCHitImmunity();
                OnAttackStarted();
            }
        }

        private void ExecuteStrike()
        {
            float progress = MathHelper.Clamp(Timer / ExecTime, 0f, 1f);
            Projectile.rotation = GetWorldRotation(
                MathHelper.SmoothStep(Profile.StartAngle, Profile.EndAngle, progress));
            visualScale = 1f;

            if (Timer >= ExecTime)
            {
                CurrentStage = AttackStage.Recover;
            }
        }

        private void RecoverStrike()
        {
            float progress = MathHelper.Clamp(Timer / RecoverTime, 0f, 1f);
            float followThrough = Math.Sign(Profile.EndAngle - Profile.StartAngle)
                * MathHelper.ToRadians(CurrentAttack == SickleAttackType.HeavySlash ? 18f : 6f);

            Projectile.rotation = GetWorldRotation(
                MathHelper.SmoothStep(Profile.EndAngle, Profile.EndAngle + followThrough, progress));
            visualScale = 1f - MathHelper.SmoothStep(0f, 1f, progress);

            if (Timer >= RecoverTime)
            {
                Projectile.Kill();
            }
        }

        private void SetWeaponPosition()
        {
            float armRotation = Projectile.rotation - MathHelper.PiOver2;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotation);

            Vector2 armPosition = Owner.GetFrontHandPosition(
                Player.CompositeArmStretchAmount.Full,
                armRotation);

            if (Owner.gravDir == -1f)
            {
                Projectile.rotation = -Projectile.rotation;
                armPosition.Y = Owner.Bottom.Y + (Owner.position.Y - armPosition.Y);
            }

            armPosition.Y += Owner.gfxOffY;
            Projectile.Center = armPosition;
            float attackScale = CurrentAttack == SickleAttackType.HeavySlash ? 1.18f : 1f;
            Projectile.scale = visualScale
                * attackScale
                * Stats.scale
                * Owner.GetAdjustedItemScale(Owner.HeldItem);
            Projectile.spriteDirection = Owner.direction = Projectile.spriteDirection;
            Owner.heldProj = Projectile.whoAmI;
        }

        private float GetWorldRotation(float localSwingAngle)
        {
            float facingAngle = Projectile.spriteDirection > 0 ? 0f : MathHelper.Pi;
            return facingAngle + Projectile.spriteDirection * (GetLocalAimAngle() + localSwingAngle);
        }

        private float GetAimRotation()
        {
            float facingAngle = Projectile.spriteDirection > 0 ? 0f : MathHelper.Pi;
            return facingAngle + Projectile.spriteDirection * GetLocalAimAngle();
        }

        private float GetLocalAimAngle()
        {
            float localAngle = Projectile.spriteDirection > 0
                ? MathHelper.WrapAngle(AimAngle)
                : MathHelper.WrapAngle(MathHelper.Pi - AimAngle);

            return MathHelper.Clamp(
                localAngle,
                MathHelper.ToRadians(-35f),
                MathHelper.ToRadians(35f));
        }

        private Vector2 GetDrawOrigin(Texture2D texture)
        {
            const float gripInset = 2f;
            float originX = Projectile.spriteDirection > 0
                ? gripInset
                : texture.Width - gripInset;
            return new Vector2(originX, texture.Height - gripInset);
        }

        private SpriteEffects GetSpriteEffects()
        {
            return Projectile.spriteDirection > 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
        }

        private float GetRotationOffset()
        {
            return Projectile.spriteDirection > 0
                ? Stats.rotationOffsetRight
                : Stats.rotationOffsetLeft;
        }

        private float GetWeaponReach()
        {
            return Projectile.Size.Length() * Projectile.scale * 0.95f;
        }
    }
}
