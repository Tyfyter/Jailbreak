using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Jailbreak.Projectiles;
using static Jailbreak.Projectiles.GlyphProjectileType;

namespace Jailbreak.Items {
    public class TestItem : CastingTool {
        internal new static List<ActionItem> actions;
        static bool altFire = false;
        public override List<ActionItem> Actions => actions;
        public override byte ProjFields => (byte)(GlyphProjectileType.Tracer|(altFire?OnHit|Paused:Normal));
        public override void SetDefaults() {
            base.SetDefaults();
            item.damage = 30;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenDriveEditor(new TestingDrive());
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            altFire = player.altFunctionUse == 2;
            base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            altFire = false;
            return false;
        }
    }
}
