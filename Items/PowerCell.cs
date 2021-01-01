using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using static Jailbreak.JailbreakExt;

namespace Jailbreak.Items {
    public abstract class PowerCellItem : ModItem {
        public RefWrapper<float> charge = 120;
        public float maxCharge => 120;
        public float recharge = 1;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Sets.Item.battery[item.type] = true;
        }
        public virtual void getStats(Item castingTool, ref float damage) {}
    }
    public class PowerCellGlobalItem : GlobalItem {
        public override bool InstancePerEntity => true;
        public override GlobalItem NewInstance(Item item) => base.NewInstance(item);
        public RefWrapper<float> charge = 0;
        public const float LihzahrdMaxCharge = 60;
        public const float DefaultRecharge = 1;
        public const float MechMaxCharge = 60;
        public const float MechRecharge = 3;
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
