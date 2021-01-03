using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;
using System.Collections;
using ReLogic.Graphics;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;

namespace Jailbreak.UI {
    public class LiteralElement : UIElement {
        protected internal Action<string> setValue;
        protected internal Func<string> getValue;
        protected internal string value;
        protected internal static bool focused = false;
        public LiteralElement(float scale) : base() {
            scale/=0.75f;
			Width.Set(Textures.LiteralBackTexture.Width * scale, 0f);
			Height.Set(Textures.LiteralBackTexture.Height * scale, 0f);
        }
        /*public override void Update(GameTime gameTime) {
            if(focused)DoKeyboardInput();
        }*/
        internal void DoKeyboardInput() {
	        KeyboardState state = Keyboard.GetState();
            string oldValue = value;
            Main.hasFocus = true;
            Main.instance.HandleIME();
            value = Main.GetInputText(value);
            if(!oldValue.Equals(value))try {
                setValue(value);
            } catch(Exception) {}
            if(Main.inputTextEnter) {
                focused = false;
                value = getValue();
            }
        }
        private void DoInput() {
            bool clicking = Main.mouseLeftRelease && Main.mouseLeft;
			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
				Main.LocalPlayer.mouseInterface = true;
                if(clicking){
                    focused = true;
                }
            } else if(clicking){
                focused = true;
            }
			//Main.blockInput = focused;
            PlayerInput.WritingText = focused;
            Main.editSign = focused;
            Main.hasFocus = focused;
        }
        public override void OnDeactivate() {
            base.OnDeactivate();
            setValue = null;
            getValue = null;
            if(focused) {
                //Main.blockInput = false;
                PlayerInput.WritingText = false;
                Main.editSign = false;
                Main.hasFocus = false;
            }
            focused = false;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch){
		    base.DrawSelf(spriteBatch);
            DoInput();
		    CalculatedStyle dimensions = GetDimensions();
            int c = IsMouseHovering||focused ? 255 : 225;
            Color color = new Color(c, c, c, c); //new Color(255, 255, 255) : new Color(180, 180, 180);
		    Vector2 position = new Vector2(dimensions.X, dimensions.Y);
            //Vector2 scale = new Vector2(dimensions.Width/Main.inventoryBack9Texture.Width, dimensions.Height/Main.inventoryBack9Texture.Height);
            Texture2D texture = Textures.LiteralBackTexture;
            spriteBatch.Draw(texture, position + new Vector2(0f, 2f), null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            string text = value;
            spriteBatch.DrawString(Main.fontMouseText, text, position+new Vector2(dimensions.Width-Main.fontMouseText.MeasureString(text).X, 0), Main.mouseTextColorReal, 0f, default, (dimensions.Width/texture.Width)*0.75f, SpriteEffects.None, 0f);
            //ConfigElement.DrawPanel2(spriteBatch, position, Main.inventoryBack9Texture, dimensions.Width, dimensions.Height, color);
	    }
    }
}
