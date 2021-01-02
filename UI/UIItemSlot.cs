using Jailbreak.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using static Jailbreak.JailbreakExt;

namespace Jailbreak.UI
{
	// This class wraps the vanilla ItemSlot class into a UIElement. The ItemSlot class was made before the UI system was made, so it can't be used normally with UIState.
	// By wrapping the vanilla ItemSlot class, we can easily use ItemSlot.
	// ItemSlot isn't very modder friendly and operates based on a "Context" number that dictates how the slot behaves when left, right, or shift clicked and the background used when drawn.
	// If you want more control, you might need to write your own UIElement.
	// See ExamplePersonUI for usage and use the Awesomify chat option of Example Person to see in action.
	public class GlyphItemSlot : UIElement
	{
		internal Item item;
		internal readonly int _context;
		internal readonly int color;
		private readonly float _scale;
		internal Func<Item, bool> ValidItemFunc;
        protected internal int index = -1;
		public GlyphItemSlot(int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float scale = 1f, Item _item = null) {
			color = colorContext;
            _context = context;
			_scale = scale;
            SetItem(_item);
			Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
		}
        public void SetItem(Item _item) {
			if(_item == null){
				item = new Item();
				item.SetDefaults(0);
			}else if(_item.IsAir){
				item = new Item();
				item.SetDefaults(0);
			}else{
				item = _item;
			}
        }
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = _scale;
			Rectangle rectangle = GetDimensions().ToRectangle();

			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem)) {
                    // Handle handles all the click and hover actions based on the context.
                    ActionItem item = null;
                    if(this.item?.modItem is ActionItem action) item = action;
                    ItemSlot.Handle(ref this.item, _context);
                    ActionItem item2 = null;
                    if(this.item?.modItem is ActionItem action2) item2 = action2;
                    GlyphItemsUI parent = ((GlyphItemsUI)Parent);
                    if(item!=item2) {
                        parent.driveDirty = true;
                    } else if(ActionItem.rightClicked&&item!=null) {
                        parent.literalIndex = index;
                        parent.LiteralTarget = item;
                        //Jailbreak.instance.literalUI.SetState(actionLiteralUI);
                    }
                    ActionItem.rightClicked = false;
					/*try {
						((GunItemsUI)Parent).UpdateItem();
					}catch (System.Exception){}*/
				}
			}
			// Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
			ItemSlot.Draw(spriteBatch, ref item, color, rectangle.TopLeft());
			Main.inventoryScale = oldScale;
		}
	}
    public class RefItemSlot : UIElement
	{
		internal Ref<Item> item;
		internal readonly int _context;
		internal readonly int color;
		private readonly float _scale;
		internal Func<Item, bool> ValidItemFunc;
        protected internal int index = -1;
		public RefItemSlot(int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float scale = 1f, Ref<Item> _item = null) {
			color = colorContext;
            _context = context;
			_scale = scale;
			item = _item;
			Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
		}
        /*public void SetItem(Item _item) {
			if(_item == null){
				item = new Item();
				item.SetDefaults(0);
			}else if(_item.IsAir){
				item = new Item();
				item.SetDefaults(0);
			}else{
				item = _item;
			}
        }*/
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = _scale;
			Rectangle rectangle = GetDimensions().ToRectangle();

			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem)) {
                    ItemSlot.Handle(ref item.Value, _context);
				}
			}
			// Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
			ItemSlot.Draw(spriteBatch, ref item.Value, color, rectangle.TopLeft());
			Main.inventoryScale = oldScale;
		}
	}
    public class CraftingItemSlot : UIElement{
		internal Item item;
		internal readonly int _context;
		internal readonly int color;
		private readonly float _scale;
		internal Func<Item, bool> ValidItemFunc;
        /// <summary>
        /// Called whenever the slot's contents are changed, param 1 is the new item, param 2 is the old item
        /// </summary>
		internal Action<Item, Item> ContentsChangedAction;
		internal Func<bool> DoDraw;
        protected internal int index = -1;
		public CraftingItemSlot(int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float scale = 1f, Item _item = null) {
			color = colorContext;
            _context = context;
			_scale = scale;
            SetItem(_item);
			Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
		}
        public void SetItem(Item _item) {
			if(_item == null){
				item = new Item();
				item.SetDefaults(0);
			}else if(_item.IsAir){
				item = new Item();
				item.SetDefaults(0);
			}else{
				item = _item;
			}
        }
        public void DecrementStack(int amount = 1) {
            item = item.Clone();
            item.stack-=amount;
            if(item.stack<1)item.TurnToAir();
        }
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = _scale;
			Rectangle rectangle = GetDimensions().ToRectangle();

			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
				if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem)) {
                    Item item2 = item;
                    ItemSlot.Handle(ref item, _context);
                    if(item!=item2) {
                        ContentsChangedAction(item, item2);
                    }
				}
			}
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            bool drawSlot = true;
            if(!(DoDraw is null))drawSlot = DoDraw();
            if(drawSlot) {
                ItemSlot.Draw(spriteBatch, ref item, color, rectangle.TopLeft());
            }else{
                Item _item = new Item();
                _item.TurnToAir();
                ItemSlot.Draw(spriteBatch, ref _item, color, rectangle.TopLeft());
            }

            Main.inventoryScale = oldScale;
		}
	}
}