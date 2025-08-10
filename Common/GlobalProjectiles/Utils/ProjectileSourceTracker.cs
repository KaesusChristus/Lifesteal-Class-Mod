using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;

public class ProjectileSourceTracker : GlobalProjectile
{
    public Item sourceItem;

    public override bool InstancePerEntity => true;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source is EntitySource_ItemUse_WithAmmo itemSource)
        {
            sourceItem = itemSource.Item;
        }
    }
}

