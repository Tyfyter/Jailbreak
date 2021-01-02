using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Jailbreak.Items;
using System;
using System.Linq;

namespace Jailbreak.UI {
	public class AssemblerUI : UIState {
        public CraftingItemSlot casingSlot;
        public CraftingItemSlot batterySlot;
        public CraftingItemSlot lensSlot;
        public RefItemSlot driveSlot;
        public CraftingItemSlot outputSlot;
        public Item outputItem;
        public override void OnInitialize() {
            Main.UIScaleMatrix.Decompose(out Vector3 scale, out Quaternion ignore, out Vector3 ignore2);
            Vector2 basePosition = new Vector2((float)(Main.screenWidth*0.05), (float)(Main.screenHeight*0.4));
            Vector2 slotOffset = new Vector2(40*scale.X,40*scale.Y);
            casingSlot = AddCraftingSlot(null,
                basePosition+slotOffset*new Vector2(1,2),
                false,
                (i)=>(i.modItem is CasingItem||i.IsAir)&&i.type!=casingSlot.item.type,
                slotScale:0.75f
                );
            batterySlot = AddCraftingSlot(null,
                basePosition+slotOffset*new Vector2(0,2),
                false,
                (i)=>(Sets.Item.battery[i.type]||i.IsAir)&&i.type!=batterySlot.item.type,
                slotScale:0.75f
                );
            lensSlot = AddCraftingSlot(null,
                basePosition+slotOffset*new Vector2(2,2),
                false,
                (i)=>(Sets.Item.lens[i.type]||i.IsAir)&&i.type!=lensSlot.item.type,
                slotScale:0.75f
                );

            outputItem = new Item();
            outputItem.SetDefaults(ModContent.ItemType<CustomCastingTool>());
            outputItem.owner = Main.myPlayer;
            outputSlot = AddCraftingSlot(outputItem,
                basePosition+slotOffset*new Vector2(1,0),
                false,
                (i)=>i.IsAir&&CanCraft(),
                slotScale:0.75f
                );
            outputSlot.DoDraw = CanCraft;
            outputSlot.ContentsChangedAction = (newItem, oldItem) => {
                if(newItem.IsAir) {
                    //oldItem.modItem.SetDefaults();
                    outputItem = new Item();
                    outputItem.SetDefaults(ModContent.ItemType<CustomCastingTool>());
                    outputItem.owner = Main.myPlayer;
                    casingSlot.DecrementStack();
                    batterySlot.DecrementStack();
                    lensSlot.DecrementStack();
                    if(((CustomCastingTool)outputItem.modItem).driveItem is null) {
                        ((CustomCastingTool)outputItem.modItem).driveItem = new Ref<Item>(new Item());
                        ((CustomCastingTool)outputItem.modItem).driveItem.Value.SetDefaults(0);
                    }
                    driveSlot.item = ((CustomCastingTool)outputItem.modItem).driveItem;
                }
            };
            CustomCastingTool castingTool = (CustomCastingTool)outputItem.modItem;
            casingSlot.ContentsChangedAction = (newItem, oldItem)=>{
                castingTool.casingItem = newItem;
                castingTool.SetDefaults();
            };
            batterySlot.ContentsChangedAction = (newItem, oldItem)=>{
                castingTool.powerCellItem = newItem;
                castingTool.SetDefaults();
            };
            lensSlot.ContentsChangedAction = (newItem, oldItem)=>{
                castingTool.lensItem = newItem;
                castingTool.SetDefaults();
            };
            if(castingTool.driveItem is null) {
                castingTool.driveItem = new Ref<Item>(new Item());
                castingTool.driveItem.Value.SetDefaults(0);
            }
            driveSlot = new RefItemSlot(_item:castingTool.driveItem, scale:0.75f) {
                    ValidItemFunc = (i)=>(i.modItem is DriveItem||i.IsAir)&&i.type!=driveSlot.item.Value.type,
                    Left = { Pixels = basePosition.X+slotOffset.X },
                    Top = { Pixels = basePosition.Y+slotOffset.X*3 }
            };
            Append(driveSlot);
        }
        private CraftingItemSlot AddCraftingSlot(Item item, Vector2 position, bool usePercent = false, Func<Item,bool> _ValidItemFunc = null, int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float slotScale = 1f) {
            CraftingItemSlot itemSlot = new CraftingItemSlot(_item:item, colorContext:colorContext, context:context, scale:slotScale) {
                    ValidItemFunc = _ValidItemFunc??(i => true),
            };
            if(usePercent) {
                itemSlot.Left = new StyleDimension{ Percent = position.X };
                itemSlot.Top = new StyleDimension{ Percent = position.Y };
            } else {
                itemSlot.Left = new StyleDimension{ Pixels = position.X };
                itemSlot.Top = new StyleDimension{ Pixels = position.Y };
            }
            Append(itemSlot);
            return itemSlot;
        }
        public override void OnDeactivate() {
            JailbreakExt.DropItem(Main.LocalPlayer.Center, casingSlot.item);
            JailbreakExt.DropItem(Main.LocalPlayer.Center, batterySlot.item);
            JailbreakExt.DropItem(Main.LocalPlayer.Center, lensSlot.item);
            JailbreakExt.DropItem(Main.LocalPlayer.Center, driveSlot.item.Value);
        }
        public bool CanCraft() {
            if(casingSlot?.item?.IsAir??true) {
                return false;
            }
            if(batterySlot?.item?.IsAir??true) {
                return false;
            }
            if(lensSlot?.item?.IsAir??true) {
                return false;
            }
            return true;
        }
    }
}