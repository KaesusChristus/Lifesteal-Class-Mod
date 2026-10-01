using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LifeStealClass.Common.Systems
{
    public class BossDownedSystem : ModSystem
    {
        public static bool downedBloodyThornBoss = false;

        public override void ClearWorld()
        {
            downedBloodyThornBoss = false;
        }


        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedBloodyThornBoss"] = downedBloodyThornBoss;
        }


        public override void LoadWorldData(TagCompound tag)
        {
            downedBloodyThornBoss = tag.GetBool("downedBloodyThornBoss");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedBloodyThornBoss;
            writer.Write(flags);
        }


        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedBloodyThornBoss = flags[0];
        }
    }
}
