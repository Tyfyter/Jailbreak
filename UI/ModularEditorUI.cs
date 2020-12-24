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
	public class ModularEditorUI : UIState {
        public List<RefItemSlot> itemSlots = new List<RefItemSlot>(){};
        protected internal Queue<Action> itemSlotQueue = new Queue<Action>(){};
        /// <summary>
        /// Ensures that item is not null then passes the parameters to SafeAddItemSlot
        /// </summary>
        public void SafeAddItemSlot(ref Ref<Item> item, Vector2? position = null, bool usePercent = false, Func<Item, bool> ValidItemFunc = null, int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float slotScale = 0.75f) {
            if(item is null) {
                item = new Ref<Item>(new Item());
                item.Value.SetDefaults(0);
            }
            SafeAddItemSlot(item, position, usePercent, ValidItemFunc, colorContext, context, slotScale);
        }
        /// <summary>
        /// Passes the parameters to an action to be added in Update
        /// </summary>
        public void SafeAddItemSlot(Ref<Item> item, Vector2? position = null, bool usePercent = false, Func<Item, bool> ValidItemFunc = null, int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float slotScale = 0.75f) {
            if(item.Value is null) {
                item.Value = new Item();
                item.Value.SetDefaults(0);
            }
            itemSlotQueue.Enqueue(()=>AddItemSlot(item, position, usePercent, ValidItemFunc, colorContext, context, slotScale));
        }
        /// <summary>
        /// Adds a reference-based item slot to the ui state
        /// </summary>
        /// <param name="item"> the item that should be referenced by the new slot</param>
        /// <param name="_position">the position of the slot, leave as null to automatically place the slot</param>
        /// <param name="usePercent">ignored if position is null</param>
        /// <param name="_ValidItemFunc">passed to RefItemSlot constructor</param>
        /// <param name="colorContext">passed to RefItemSlot constructor</param>
        /// <param name="context">passed to RefItemSlot constructor</param>
        /// <param name="slotScale">passed to RefItemSlot constructor</param>
        public void AddItemSlot(Ref<Item> item, Vector2? _position = null, bool usePercent = false, Func<Item,bool> _ValidItemFunc = null,int colorContext = ItemSlot.Context.CraftingMaterial, int context = ItemSlot.Context.InventoryItem, float slotScale = 1f) {
            Vector2 position;
            if(_position is null) {
                Main.UIScaleMatrix.Decompose(out Vector3 scale, out Quaternion ignore, out Vector3 ignore2);
                position.X = (float)((Main.screenWidth*0.05)+(itemSlots.Count*40*scale.X));
                position.Y = (float)((Main.screenHeight*0.4)+(40*scale.Y));
                usePercent = false;
            } else {
                position = _position.Value;
            }
            RefItemSlot itemSlot = new RefItemSlot(_item:item, colorContext:colorContext, context:context, scale:slotScale) {
                    ValidItemFunc = _ValidItemFunc??(i => true),
            };
            if(usePercent) {
                itemSlot.Left = new StyleDimension{ Percent = position.X };
                itemSlot.Top = new StyleDimension{ Percent = position.Y };
            } else {
                itemSlot.Left = new StyleDimension{ Pixels = position.X };
                itemSlot.Top = new StyleDimension{ Pixels = position.Y };
            }
            itemSlots.Add(itemSlot);
            Append(itemSlot);
        }
        public override void Update(GameTime gameTime) {
            while(itemSlotQueue.Count>0) {
                itemSlotQueue.Dequeue()();
            }
        }
    }
}