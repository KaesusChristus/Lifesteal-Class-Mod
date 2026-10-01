using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using HarvesterClassMod.Common.Utils;

namespace HarvesterClassMod.Common.AIStyles
{
    public class SickleAI : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.aiStyle == CustomAiStyleID.Sickle;
        }
    }
}
