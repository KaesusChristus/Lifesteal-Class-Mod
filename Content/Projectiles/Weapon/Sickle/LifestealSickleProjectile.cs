using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using LifeStealClass.Content.Core;
using System.IO;

namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public abstract class LifestealSickleProjectile : ModProjectile
    {
        protected const float FIRSTHALFSWING = 0.45f;

        protected enum AttackType { Swing, Spin }
        protected enum AttackStage { Prepare, Execute, Unwind }

        protected AttackType CurrentAttack
        {
            get => (AttackType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        protected AttackStage CurrentStage
        {
            get => (AttackStage)Projectile.localAI[0];
            set
            {
                Projectile.localAI[0] = (float)value;
                Timer = 0;
            }
        }

        protected ref float InitialAngle => ref Projectile.ai[1];
        protected ref float Timer => ref Projectile.ai[2];
        protected ref float Progress => ref Projectile.localAI[1];
        protected ref float Size => ref Projectile.localAI[2];
        protected ref float ComboStep => ref Projectile.ai[2];

        protected Player Owner => Main.player[Projectile.owner];

        // -------------------------------------------------
        // INDIVIDUELLE WAFFENWERTE
        // -------------------------------------------------
        public virtual SickleStats GetStats()
        {
            return new SickleStats
            {
                SWINGRANGE = 1.67f * MathF.PI,
                SPINRANGE = 3.5f * MathF.PI,

                WINDUP = 0.15f,
                UNWIND = 0.4f,
                SPINTIME = 2.5f,

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

        private float PrepTime => Stats.PrepTime / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float ExecTime => Stats.ExecTime / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float HideTime => Stats.HideTime / Owner.GetTotalAttackSpeed(Projectile.DamageType);

        public override string Texture => "Terraria/Images/Item_" + ItemID.DirtBlock;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;

            Projectile.DamageType = ModContent.GetInstance<HarvesterDamage>();
            Projectile.ownerHitCheck = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
            float targetAngle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();

            if (CurrentAttack == AttackType.Spin)
            {
                InitialAngle = (float)(-Math.PI / 2 - Math.PI * 1 / 3 * Projectile.spriteDirection);
            }
            else
            {
                if (Projectile.spriteDirection == 1)
                {
                    targetAngle = MathHelper.Clamp(
                        targetAngle,
                        -MathHelper.Pi / 3,
                        MathHelper.Pi / 6);
                }
                else
                {
                    if (targetAngle < 0)
                        targetAngle += MathHelper.TwoPi;

                    targetAngle = MathHelper.Clamp(
                        targetAngle,
                        MathHelper.Pi * 5f / 6f,
                        MathHelper.Pi * 4f / 3f);
                }

                InitialAngle = targetAngle - FIRSTHALFSWING * Stats.SWINGRANGE * Projectile.spriteDirection;
            }
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
                    UnwindStrike();
                    break;
            }

            SetSwordPosition();
            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 origin;
            float rotationOffset;
            SpriteEffects effects;

            if (Projectile.spriteDirection > 0)
            {
                origin = new Vector2(0, Projectile.height);
                rotationOffset = Stats.rotationOffsetRight;
                effects = SpriteEffects.None;
            }
            else
            {
                origin = new Vector2(Projectile.width, Projectile.height);
                rotationOffset = Stats.rotationOffsetLeft;
                effects = SpriteEffects.FlipHorizontally;
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor * Projectile.Opacity,
                Projectile.rotation + rotationOffset,
                origin,
                Projectile.scale,
                effects,
                0f
            );

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2()
                * (Projectile.Size.Length() * Projectile.scale);

            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                Stats.hitboxWidth * Projectile.scale,
                ref collisionPoint);
        }

        public override void CutTiles()
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2()
                * (Projectile.Size.Length() * Projectile.scale);

            Utils.PlotTileLine(
                start,
                end,
                Stats.hitboxWidth * Projectile.scale,
                DelegateMethods.CutTiles);
        }

        public override bool? CanDamage()
        {
            if (CurrentStage == AttackStage.Prepare)
                return false;

            return base.CanDamage();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride =
                target.position.X > Owner.MountedCenter.X ? 1 : -1;

            if (CurrentAttack == AttackType.Spin)
                modifiers.Knockback += 1;

            modifiers.FinalDamage *= 1.4f;
        }

        public void SetSwordPosition()
        {
            Projectile.rotation =
                InitialAngle + Projectile.spriteDirection * Progress;

            Owner.SetCompositeArmFront(
                true,
                Player.CompositeArmStretchAmount.Full,
                Projectile.rotation - MathHelper.ToRadians(90f));

            Vector2 armPosition = Owner.GetFrontHandPosition(
                Player.CompositeArmStretchAmount.Full,
                Projectile.rotation - MathHelper.PiOver2);

            if (Owner.gravDir == -1f)
            {
                Projectile.rotation = -Projectile.rotation;
                armPosition.Y = Owner.Bottom.Y + (Owner.position.Y - armPosition.Y);
            }

            armPosition.Y += Owner.gfxOffY;

            Projectile.Center = armPosition;

            Projectile.scale =
                Size *
                Stats.scale *
                Owner.GetAdjustedItemScale(Owner.HeldItem);

            Owner.heldProj = Projectile.whoAmI;
        }

        private void PrepareStrike()
        {
            Progress =
                Stats.WINDUP *
                Stats.SWINGRANGE *
                (1f - Timer / PrepTime);

            Size =
                MathHelper.SmoothStep(
                    0,
                    1,
                    Timer / PrepTime);

            if (Timer >= PrepTime)
            {
                SoundEngine.PlaySound(SoundID.Item1);
                CurrentStage = AttackStage.Execute;
            }
        }

        private void ExecuteStrike()
        {
            if (CurrentAttack == AttackType.Swing)
            {
                Progress = MathHelper.SmoothStep(
                    0,
                    Stats.SWINGRANGE,
                    (1f - Stats.UNWIND) * Timer / ExecTime);

                if (Timer >= ExecTime)
                    CurrentStage = AttackStage.Unwind;
            }
            else
            {
                Progress = MathHelper.SmoothStep(
                    0,
                    Stats.SPINRANGE,
                    (1f - Stats.UNWIND / 2f) *
                    Timer / (ExecTime * Stats.SPINTIME));

                if (Timer == (int)(ExecTime * Stats.SPINTIME * 0.75f))
                {
                    SoundEngine.PlaySound(SoundID.Item1);
                    Projectile.ResetLocalNPCHitImmunity();
                }

                if (Timer >= ExecTime * Stats.SPINTIME)
                    CurrentStage = AttackStage.Unwind;
            }
        }

        private void UnwindStrike()
        {
            if (CurrentAttack == AttackType.Swing)
            {
                Progress = MathHelper.SmoothStep(
                    0,
                    Stats.SWINGRANGE,
                    (1f - Stats.UNWIND) +
                    Stats.UNWIND * Timer / HideTime);

                Size = 1f - MathHelper.SmoothStep(
                    0,
                    1,
                    Timer / HideTime);

                if (Timer >= HideTime)
                    Projectile.Kill();
            }
            else
            {
                Progress = MathHelper.SmoothStep(
                    0,
                    Stats.SPINRANGE,
                    (1f - Stats.UNWIND / 2f) +
                    Stats.UNWIND / 2f * Timer /
                    (HideTime * Stats.SPINTIME / 2f));

                Size = 1f - MathHelper.SmoothStep(
                    0,
                    1,
                    Timer / (HideTime * Stats.SPINTIME / 2f));

                if (Timer >= HideTime * Stats.SPINTIME / 2f)
                    Projectile.Kill();
            }
        }
    }
}