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
        public RefWrapper<float> charge = 120;
        public float maxCharge = 120;
        public float recharge = 1;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Sets.Item.battery[item.type] = true;
        }
    }
    public class LensGlobalItem : GlobalItem {
        public const byte DefaultType = Normal;
        public const byte SapphireType = Normal;
        public static float maxCharge(Item item){
            if(item.modItem is PowerCellItem powerCell)return powerCell.maxCharge;
            switch(item.type) {
                case ItemID.LihzahrdPowerCell:
                return PowerCellGlobalItem.LihzahrdMaxCharge;
                case ItemID.MechanicalBatteryPiece:
                return PowerCellGlobalItem.MechMaxCharge;
            }
            return 0;
        }
    }
}
