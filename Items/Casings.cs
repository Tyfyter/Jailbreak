using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using static Jailbreak.Projectiles.GlyphProjectileType;
using static Jailbreak.JailbreakExt;

namespace Jailbreak.Items {
    public abstract class CasingItem : ModItem {
        public byte ProjType => Normal;
        public Color color => new Color(0,90,255);
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Sets.Item.casing[item.type] = true;
        }
        public virtual void getStats(Item castingTool, ref float damage) {}
    }
}
