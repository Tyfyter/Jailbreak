using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Jailbreak.Tiles
{
    public class GenericGemspark : ModTile{
        public override void SetDefaults(){
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Gemspark Block");
			AddMapEntry(new Color(255, 255, 255), name);
        }
    }
    public class GenericMonolith : ModTile{
        public override void SetDefaults(){
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Lunar Monolith");
			AddMapEntry(new Color(255, 255, 255), name);
        }
    }
    public class JailbreakGlobalTile : GlobalTile{
        public override int[] AdjTiles(int type){
            if(type>=TileID.AmethystGemspark&&type<=TileID.AmberGemspark){
                return new int[]{ModContent.TileType<GenericGemspark>()};
            }
            if(type==TileID.LunarMonolith)return new int[]{ModContent.TileType<GenericMonolith>()};
            return new int[0];
        }
    }
}