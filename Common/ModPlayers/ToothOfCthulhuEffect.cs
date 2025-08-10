using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using System;
using LifeStealClass.Content.Core;
using LifeStealClass.Content.Projectiles.Accessories;

namespace LifeStealClass.Content
{
    public class ToothOfCthulhuEffect : ModPlayer
    {
        public int damage = 12;
        public float knockback = 2f;
        public bool hasToothOfCthulhuEquipped = true;
        private bool projectileSpawned = false;

        // Konstanten für minimale und maximale Offsets
        private const int MinOffsetX = 100;
        private const int MaxOffsetX = 300;

        private const int MinOffsetY = -200;
        private const int MaxOffsetY = 200;

        private int attacks = 0;

        public override void ResetEffects()
        {
            hasToothOfCthulhuEquipped = false;
            projectileSpawned = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasToothOfCthulhuEquipped && projectileSpawned == false && hit.DamageType == ModContent.GetInstance<LifestealDamage>())
            {
                Random random = new Random();

                if (random.Next(0, 100) <= 2) // 2% Chance das einer Spawnt
                {
                    SpawnCthulhuChargeProjectile(target);
                }
            }
        }

        
        private void SpawnCthulhuChargeProjectile(NPC target)
        {
            // Verhindern, dass das Projektil sich selbst erneut spawnen kann
            projectileSpawned = true;

            // Quelle für das Projektil holen
            IEntitySource source = Player.GetSource_ItemUse(Player.HeldItem);


            // Zufällige Offsets für die Spawnposition berechnen
            Random random = new Random();
            float xOffset = random.Next(MinOffsetX, MaxOffsetX + 1); // Zufälliger Wert zwischen MinOffset und MaxOffset
            float yOffset = random.Next(MinOffsetY, MaxOffsetY + 1); // Zufälliger Wert zwischen MinOffset und MaxOffset

            // Spawnposition bestimmen
            Vector2 spawnPosition;
            if (Player.position.X < target.position.X)
            {
                spawnPosition = new Vector2(target.position.X + target.width + 100 + xOffset, target.position.Y + yOffset);
            }
            else
            {
                spawnPosition = new Vector2(target.position.X - 100 + (xOffset * -1), target.position.Y + yOffset);
            }

            // Neues Projektil erstellen
            int newProj = Projectile.NewProjectile(source, spawnPosition, Vector2.Zero, ModContent.ProjectileType<ToothOfCthulhuProjectile>(), damage, knockback, Main.myPlayer, 0f, target.whoAmI);

            // Überprüfen, ob das Projektil erfolgreich erstellt wurde
            if (newProj >= 0 && newProj < Main.maxProjectiles)
            {
                // Nur wenn es sich um das ToothOfCthulhu-Projektil handelt, zusätzliche Logik anwenden
                if (Main.projectile[newProj].type == ModContent.ProjectileType<ToothOfCthulhuProjectile>())
                {
                    // AI[1] des Projektils setzen, um das Ziel-NPC zu speichern
                    Main.projectile[newProj].ai[1] = target.whoAmI;
                }
            }
        }
    }
}
