using Jailbreak.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace Jailbreak.UI
{
	// This class wraps the vanilla ItemSlot class into a UIElement. The ItemSlot class was made before the UI system was made, so it can't be used normally with UIState.
	// By wrapping the vanilla ItemSlot class, we can easily use ItemSlot.
	// ItemSlot isn't very modder friendly and operates based on a "Context" number that dictates how the slot behaves when left, right, or shift clicked and the background used when drawn.
	// If you want more control, you might need to write your own UIElement.
	// See ExamplePersonUI for usage and use the Awesomify chat option of Example Person to see in action.
	public class VanillaItemSlotWrapper : UIElement
	{
		internal Item Item;
		internal readonly int _context;
		internal readonly int color;
		private readonly float _scale;
		internal Func<Item, bool> ValidItemFunc;
        protected internal int index = -1;
		public VanillaItemSlotWrapper(int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float scale = 1f, Item item = null) {
			color = colorContext;
            _context = context;
			_scale = scale;
			if(item == null){
				Item = new Item();
				Item.SetDefaults(0);
			}else if(item.IsAir){
				Item = new Item();
				Item.SetDefaults(0);
			}else{
				Item = item;
			}

			Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
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
                    if(Item?.modItem is ActionItem action) item = action;
					ItemSlot.Handle(ref Item, _context);
                    ActionItem item2 = null;
                    if(Item?.modItem is ActionItem action2) item2 = action2;
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
			ItemSlot.Draw(spriteBatch, ref Item, color, rectangle.TopLeft());
			Main.inventoryScale = oldScale;
		}
	}
}