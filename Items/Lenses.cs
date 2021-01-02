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
    public abstract class LensItem : ModItem {
        public byte ProjType => Normal;
        public Color color => new Color(0,90,255);
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Sets.Item.lens[item.type] = true;
        }

        public virtual void getStats(Item castingTool, ref float damage, ref float shootSpeed) { damage*=item.damage; }
    }
    public class LensGlobalItem : GlobalItem {
        public const byte DefaultType = Normal;
        public const byte SapphireType = Tracer;
        public const byte RubyType = OnHit|Paused|Tracer;
        public const byte MechType = Repeat;
        public const byte GolemType = Golem;
        public static byte projType(Item item){
            if(item.modItem is LensItem lens)return lens.ProjType;
            switch(item.type) {
                case ItemID.Lens:
                return DefaultType;
                case ItemID.Sapphire:
                return SapphireType;
                case ItemID.Ruby:
                return RubyType;
                case ItemID.MechanicalLens:
                return MechType;
                case ItemID.EyeoftheGolem:
                return GolemType;
            }
            return DefaultType;
        }
        public static void getStats(Item item, Item castingTool, ref float damage, ref float shootSpeed){
            if(item.modItem is LensItem lens) {
                lens.getStats(castingTool, ref damage, ref shootSpeed);
            }else if(!(item is null))switch(item.type) {
                case ItemID.Ruby:
                damage*=20;
                break;
                case ItemID.MechanicalLens:
                damage*=30;
                break;
                default:
                damage*=0;
                break;
            }
        }
        public static Color getColor(Item item){
            if(item.modItem is LensItem lens)return lens.color;
            switch(item.type) {
                case ItemID.Sapphire:
                return new Color(0,0,215);
                case ItemID.Ruby:
                return new Color(215,35,0);
                case ItemID.EyeoftheGolem:
                return new Color(255,150,0);
            }
            return new Color(0,90,255);
        }
    }
}
