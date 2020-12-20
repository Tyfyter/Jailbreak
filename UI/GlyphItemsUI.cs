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

namespace Jailbreak.UI {
	public class GlyphItemsUI : UIState {
        public List<VanillaItemSlotWrapper> itemSlots = new List<VanillaItemSlotWrapper>(){};
        public DriveItem drive;
        protected internal bool dirty = false;
        public override void OnInitialize(){
            int l = 1;
            if(drive!=null){
                if(drive.Actions == null)drive.Actions = new List<ActionItem>() { };
                l+=drive.Actions.Count;
            }
            Main.UIScaleMatrix.Decompose(out Vector3 scale, out Quaternion ignore, out Vector3 ignore2);
            ActionItem current;
            int layers = 0;
            int xPos = 0;
            for (int i = 0; i < l; i++){
                xPos++;
                if(i>=itemSlots.Count)itemSlots.Add(null);
                current = drive.Actions.Count>i?drive.Actions[i]:null;
                itemSlots[i] = new VanillaItemSlotWrapper(scale:0.75f,context:drive.Readonly?ItemSlot.Context.CraftingMaterial:ItemSlot.Context.InventoryItem){
				Left = { Pixels = (float)((Main.screenWidth*0.05)+(xPos*40*scale.X)) },
                Top = { Pixels = (float)((Main.screenHeight*0.4)+(layers*40*scale.Y)) },
                ValidItemFunc = item => item.IsAir || (!item.IsAir && (item.modItem is ActionItem))
				};
                if(xPos*40*scale.X>Main.screenWidth/2) {
                    xPos = 0;
                    layers++;
                }
                if(drive!=null){
                    if(current!=null){
                        itemSlots[i].Item = current?.item??Jailbreak.CreateNew(current).item;
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
            if(dirty&&drive!=null)UpdateItems();
        }
        public void UpdateItems(){
            //List<ActionItem> actions = new List<ActionItem>(){};
            drive.Actions.Clear();
			for(int i = 0; i < itemSlots.Count; i++)drive.Actions.Add(itemSlots[i].Item.modItem as ActionItem);
            RemoveAllChildren();
            itemSlots = new List<VanillaItemSlotWrapper>(){};
            OnInitialize();
            //drive.actions = actions;
		}
    }
}