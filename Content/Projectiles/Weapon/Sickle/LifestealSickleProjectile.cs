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
        private const float SWINGRANGE = 1.67f * MathF.PI;
        private const float SPINRANGE = 3.5f * MathF.PI;

        private const float FIRSTHALFSWING = 0.45f;
        private const float WINDUP = 0.15f;
        private const float UNWIND = 0.4f;
        private const float SPINTIME = 2.5f;

        private enum AttackType { Swing, Spin }
        private enum AttackStage { Prepare, Execute, Unwind }

        private AttackType CurrentAttack
        {
            get => (AttackType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private AttackStage CurrentStage
        {
            get => (AttackStage)Projectile.localAI[0];
            set
            {
                Projectile.localAI[0] = (float)value;
                Timer = 0;
            }
        }

        private ref float InitialAngle => ref Projectile.ai[1];
        private ref float Timer => ref Projectile.ai[2];
        private ref float Progress => ref Projectile.localAI[1];
        private ref float Size => ref Projectile.localAI[2];

        private Player Owner => Main.player[Projectile.owner];

        private float PrepTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float ExecTime => 8f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float HideTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);

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
                InitialAngle = (float)(-Math.PI / 2 - Math.PI * 1 / 3 * Projectile.spriteDirection); // For the spin, starting angle is designated based on direction of hit
            }
            else
            {
                if (Projectile.spriteDirection == 1)
                {
                    // However, we limit the rangle of possible directions so it does not look too ridiculous
                    targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 1 / 3, (float)Math.PI * 1 / 6);
                }
                else
                {
                    if (targetAngle < 0)
                    {
                        targetAngle += 2 * (float)Math.PI; // This makes the range continuous for easier operations
                    }

                    targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
                }

                InitialAngle = targetAngle - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection; // Otherwise, we calculate the angle
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            // Projectile.spriteDirection for this projectile is derived from the mouse position of the owner in OnSpawn, as such it needs to be synced. spriteDirection is not one of the fields automatically synced over the network. All Projectile.ai slots are used already, so we will sync it manually. 
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
            // Calculate origin of sword (hilt) based on orientation and offset sword rotation (as sword is angled in its sprite)
            Vector2 origin;
            float rotationOffset;
            SpriteEffects effects;

            if (Projectile.spriteDirection > 0)
            {
                origin = new Vector2(0, Projectile.height);
                rotationOffset = MathHelper.ToRadians(45f);
                effects = SpriteEffects.None;
            }
            else
            {
                origin = new Vector2(Projectile.width, Projectile.height);
                rotationOffset = MathHelper.ToRadians(135f);
                effects = SpriteEffects.FlipHorizontally;
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

            // Since we are doing a custom draw, prevent it from normally drawing
            return false;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
        }

        public override void CutTiles()
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
            Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
        }

        // We make it so that the projectile can only do damage in its release and unwind phases
        public override bool? CanDamage()
        {
            if (CurrentStage == AttackStage.Prepare)
                return false;
            return base.CanDamage();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Make knockback go away from player
            modifiers.HitDirectionOverride = target.position.X > Owner.MountedCenter.X ? 1 : -1;

            // If the NPC is hit by the spin attack, increase knockback slightly
            if (CurrentAttack == AttackType.Spin)
                modifiers.Knockback += 1;
            modifiers.FinalDamage *= 1.4f;
        }

        // Function to easily set projectile and arm position
        public void SetSwordPosition()
        {
            Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress; // Set projectile rotation

            // Set composite arm allows you to set the rotation of the arm and stretch of the front and back arms independently
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
            Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); // get position of hand

            // Adjust the position for reversed gravity.
            if (Owner.gravDir == -1f)
            {
                Projectile.rotation = 0f - Projectile.rotation;
                armPosition.Y = Owner.Bottom.Y + (Owner.position.Y - armPosition.Y);
            }

            armPosition.Y += Owner.gfxOffY;
            Projectile.Center = armPosition; // Set projectile to arm position
            Projectile.scale = Size * 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem); // Slightly scale up the projectile and also take into account melee size modifiers

            Owner.heldProj = Projectile.whoAmI; // set held projectile to this projectile
        }

        // Function facilitating the taking out of the sword
        private void PrepareStrike()
        {
            Progress = WINDUP * SWINGRANGE * (1f - Timer / PrepTime); // Calculates rotation from initial angle
            Size = MathHelper.SmoothStep(0, 1, Timer / PrepTime); // Make sword slowly increase in size as we prepare to strike until it reaches max

            if (Timer >= PrepTime)
            {
                SoundEngine.PlaySound(SoundID.Item1); // Play sword sound here since playing it on spawn is too early
                CurrentStage = AttackStage.Execute; // If attack is over prep time, we go to next stage
            }
        }

        // Function facilitating the first half of the swing
        private void ExecuteStrike()
        {
            if (CurrentAttack == AttackType.Swing)
            {
                Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) * Timer / ExecTime);

                if (Timer >= ExecTime)
                {
                    CurrentStage = AttackStage.Unwind;
                }
            }
            else
            {
                Progress = MathHelper.SmoothStep(0, SPINRANGE, (1f - UNWIND / 2) * Timer / (ExecTime * SPINTIME));

                if (Timer == (int)(ExecTime * SPINTIME * 3 / 4))
                {
                    SoundEngine.PlaySound(SoundID.Item1); // Play sword sound again
                    Projectile.ResetLocalNPCHitImmunity(); // Reset the local npc hit immunity for second half of spin
                }

                if (Timer >= ExecTime * SPINTIME)
                {
                    CurrentStage = AttackStage.Unwind;
                }
            }
        }

        // Function facilitating the latter half of the swing where the sword disappears
        private void UnwindStrike()
        {
            if (CurrentAttack == AttackType.Swing)
            {
                Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / HideTime);
                Size = 1f - MathHelper.SmoothStep(0, 1, Timer / HideTime); // Make sword slowly decrease in size as we end the swing to make a smooth hiding animation

                if (Timer >= HideTime)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Progress = MathHelper.SmoothStep(0, SPINRANGE, (1f - UNWIND / 2) + UNWIND / 2 * Timer / (HideTime * SPINTIME / 2));
                Size = 1f - MathHelper.SmoothStep(0, 1, Timer / (HideTime * SPINTIME / 2));

                if (Timer >= HideTime * SPINTIME / 2)
                {
                    Projectile.Kill();
                }
            }
        }
    }
}