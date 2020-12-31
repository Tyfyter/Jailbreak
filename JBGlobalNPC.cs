using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Jailbreak.Items;

namespace Jailbreak {
    public partial class JailbreakWorld : ModWorld {
        internal bool droppedAssembler = false;
        public override void Load(TagCompound tag) {
            if(tag.HasTag("droppedAssembler"))droppedAssembler = tag.GetBool("droppedAssembler");
        }
        public override TagCompound Save() {
            return new TagCompound {
                { "droppedAssembler", droppedAssembler }
            };
        }
    }
    public class JBGlobalNPC : GlobalNPC {
        public override void NPCLoot(NPC npc) {
            if(npc.type==NPCID.KingSlime&&(!ModContent.GetInstance<JailbreakWorld>().droppedAssembler||Main.rand.Next(5)==0)) {
                Item.NewItem(npc.Center, ModContent.ItemType<Assembler>());
            }
        }
    }
}
