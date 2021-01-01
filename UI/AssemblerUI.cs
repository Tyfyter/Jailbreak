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
                (i)=>i.modItem is CasingItem,
                slotScale:1f
                );
            batterySlot = AddCraftingSlot(null,
                basePosition+slotOffset*new Vector2(0,2),
                false,
                (i)=>Sets.Item.battery[i.type],
                slotScale:1f
                );
            lensSlot = AddCraftingSlot(null,
                basePosition+slotOffset*new Vector2(2,2),
                false,
                (i)=>Sets.Item.lens[i.type],
                slotScale:1f
                );

            outputItem = new Item();
            outputItem.SetDefaults(ModContent.ItemType<CustomCastingTool>());
            outputSlot = AddCraftingSlot(outputItem,
                basePosition+slotOffset*new Vector2(1,0),
                false,
                (i)=>i.IsAir&&CanCraft(),
                slotScale:1f
                );
            outputSlot.DoDraw = CanCraft;
            outputSlot.ContentsChangedAction = (newItem, oldItem) => {
                if(newItem.IsAir) {
                    outputItem = new Item();
                    outputItem.SetDefaults(ModContent.ItemType<CustomCastingTool>());
                    casingSlot.SetItem(null);
                    batterySlot.SetItem(null);
                    lensSlot.SetItem(null);
                }
            };
            CustomCastingTool castingTool = ((CustomCastingTool)outputItem.modItem);
            casingSlot.ContentsChangedAction = (newItem, oldItem)=>{
                castingTool.casingItem = newItem;
            };
            batterySlot.ContentsChangedAction = (newItem, oldItem)=>{
                castingTool.powerCellItem = newItem;
            };
            lensSlot.ContentsChangedAction = (newItem, oldItem)=>{
                castingTool.lensItem = newItem;
            };
            if(castingTool.driveItem is null) {
                castingTool.driveItem = new Ref<Item>(new Item());
                castingTool.driveItem.Value.SetDefaults(0);
            }
            driveSlot = new RefItemSlot(_item:castingTool.driveItem, scale:scale.X) {
                    ValidItemFunc = (i)=>i.modItem is DriveItem,
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