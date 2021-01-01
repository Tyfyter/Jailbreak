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
	public class GlyphItemsUI : UIState {
        /// <summary>
        /// values below 1 are special: 0:infinite
        /// </summary>
        protected internal int maxSize = 0;
        public List<GlyphItemSlot> itemSlots = new List<GlyphItemSlot>(){};
        public DriveItem drive;
        protected internal bool driveDirty = false;
        protected internal int literalIndex = -1;
        protected internal ActionItem LiteralTarget;
		public LiteralElement literalUI;
        public override void OnInitialize(){
            int l = 1;
            if(drive!=null){
                if(drive.Actions == null)drive.Actions = new List<ActionItem>() { };
                l+=drive.Actions.Count;
            }
            if(maxSize>0&&l>maxSize) {
                l = maxSize;
            }
            Main.UIScaleMatrix.Decompose(out Vector3 scale, out Quaternion ignore, out Vector3 ignore2);
            ActionItem current;
            int layers = 0;
            int xPos = 0;
            for (int i = 0; i < l; i++){
                if(i>=itemSlots.Count)itemSlots.Add(null);
                current = drive.Actions.Count>i?drive.Actions[i]:null;
                itemSlots[i] = new GlyphItemSlot(scale: 0.75f, context: drive.Readonly ? ItemSlot.Context.CraftingMaterial : ItemSlot.Context.InventoryItem) {
                    Left = { Pixels = (float)((Main.screenWidth*0.05)+(xPos++*40*scale.X)) },
                    Top = { Pixels = (float)((Main.screenHeight*0.4)+(layers*40*scale.Y)) },
                    ValidItemFunc = item => item.IsAir || (!item.IsAir && (item.modItem is ActionItem)),
                    index = i
				};
                if(xPos*40*scale.X>Main.screenWidth*0.4) {
                    xPos = 0;
                    layers++;
                }
                if(drive!=null){
                    if(current!=null){
                        itemSlots[i].item = current?.item??Jailbreak.CreateNew(current).item;
                    }
                }
                Append(itemSlots[i]);
                if(current == null)break;
            }
        }
        /*public override void Update(GameTime gameTime){
            base.Update(gameTime);
            if(item!=null){
                item.UpdateInventory(Main.LocalPlayer);
            }
        }*/
        /*
        public override void OnDeactivate(){
			UpdateItems();
		}
        /*public override void Draw(SpriteBatch spriteBatch) {
            if(!Main.playerInventory) {
                Deactivate();
            }
			//UpdateItem();
		}
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if(dirty)UpdateItems();
        }//*/
        public override void Update(GameTime gameTime) {
            if(driveDirty&&drive!=null)UpdateItems();
            if(literalIndex!=-1)UpdateLiteralEditor();
        }
        public void UpdateLiteralEditor() {
            int index = literalIndex;
            literalIndex = -1;
            if(literalUI!=null) {
                literalUI.Deactivate();
                RemoveChild(literalUI);
            }
            if(LiteralTarget == null) {
                return;
            }
            Main.UIScaleMatrix.Decompose(out Vector3 scale, out Quaternion ignore, out Vector3 ignore2);
            GlyphItemSlot slot = itemSlots[index];
            LiteralElement actionLiteralUI = literalUI = new LiteralElement(scale.X*0.75f) {
                Left = { Pixels = slot.Left.Pixels },
                Top = {
                    Pixels = slot.Top.Pixels - slot.Height.Pixels/2
                }
            };
            //actionLiteralUI.Top.Pixels = this.Top.Pixels - actionLiteralUI.Height.Pixels*1.25f;
            actionLiteralUI.setValue = LiteralTarget.SetLiteral;
            actionLiteralUI.getValue = LiteralTarget.GetLiteral;
            actionLiteralUI.value = LiteralTarget.GetLiteral();
            actionLiteralUI.Activate();
            Append(actionLiteralUI);
            LiteralTarget = null;
        }
        public void UpdateItems(){
            driveDirty = false;
            //List<ActionItem> actions = new List<ActionItem>(){};
            lock(drive.Actions){
                drive.Actions.Clear();
                ActionItem item;
                for(int i = 0; i < itemSlots.Count; i++) {
                    item = itemSlots[i].item.modItem as ActionItem;
                    if(item!=null) {
                        drive.Actions.Add(item);
                    }
                }
                if((itemSlots.Count>0&&!itemSlots[itemSlots.Count-1].item.IsAir)||(itemSlots.Count>1&&itemSlots[itemSlots.Count-2].item.IsAir)) {
                    ResizeSlotList();
                }
            }
            //drive.actions = actions;
		}
        protected void ResizeSlotList() {
            RemoveAllChildren();
            itemSlots = new List<GlyphItemSlot>(){};
            OnInitialize();
        }
    }
}