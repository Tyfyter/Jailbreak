using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Jailbreak.Projectiles;

namespace Jailbreak.Items {
    public class TestItem : CastingTool {
        internal new static List<ActionItem> actions;
        public override List<ActionItem> Actions => actions;
        public override byte ProjFields => GlyphProjectileType.Tracer;
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenDriveEditor(new TestingDrive());
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
    }
}
