using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.ID;

namespace Jailbreak.Items {
    public class Assembler : ModItem {
        public override void SetDefaults() {
            item.CloneDefaults(ItemID.Furnace);
            item.createTile = ModContent.TileType<AssemblerTile>();
        }
    }
    public class AssemblerTile : ModTile {
        public override bool Autoload(ref string name, ref string texture) {
            texture = "Jailbreak/Items/Assembler";
            return base.Autoload(ref name, ref texture);
        }
        public override void SetDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Assembler");
			AddMapEntry(new Color(57, 55, 72), name);
		}
		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            if(frameX == 0 && frameY == 0)Item.NewItem(new Vector2(i,j)*16, ModContent.ItemType<Assembler>());
        }
        public override bool NewRightClick(int i, int j) {
            Jailbreak.instance.OpenAssemblerUI();
            return false;
        }
    }
}
