using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using LifeStealClass.Common.Utils;
using LifeStealClass.Content.Core;

namespace LifeStealClass.Common.GlobalProjectiles
{
    public class LifestealEffectsProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool FromLifestealItem { get; private set; }
        public Item SourceItem { get; private set; }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource)
            {
                SourceItem = itemSource.Item;
                FromLifestealItem = SourceItem.ModItem is LifeStealItem;
            }
        }

        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (FromLifestealItem && hit.Crit)
            {
                LifestealHelper.MakeDust(
                    target.position,
                    target.width,
                    target.height,
                    DustID.LifeDrain);
            }
        }

        public override void AI(Projectile projectile)
        {
            if (FromLifestealItem && Main.rand.NextBool(5))
            {
                LifestealHelper.MakeDust(projectile.position, projectile.width, projectile.height, DustID.Blood);
            }
        }
    }
}
