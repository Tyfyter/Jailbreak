using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace Jailbreak.Items {
    public abstract class DriveItem : ModItem {
        protected List<ActionItem> actions;
        public virtual List<ActionItem> Actions {
            get => actions;
            set => actions = value;
        }
        public bool Readonly => false;
        public override TagCompound Save() {
            return new TagCompound() {
                {"actions", Actions.Select((i)=>i.item??Jailbreak.CreateNew(i).item).ToList()}
            };
        }
        public override void Load(TagCompound tag) {
            if(tag.ContainsKey("actions")) {
                Actions = tag.Get<List<Item>>("actions").Select((i) => (ActionItem)i.modItem).ToList();
            }
        }
    }
    //public class RADrive : DriveItem {}
    public class TestingDrive : DriveItem {
        public override List<ActionItem> Actions {
            get => TestItem.actions;
            set => TestItem.actions = value;
        }
        public override string Texture => "Jailbreak/Items/RAM1";
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenDriveEditor(this);
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
    }
}
