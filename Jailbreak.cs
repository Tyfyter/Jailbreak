using Jailbreak.Items;
using Jailbreak.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Jailbreak {
	public class Jailbreak : Mod {

        internal static Dictionary<string,ActionItem> Actions;
        internal static List<ActionItem> actions;
        internal static Jailbreak instance;
        internal static Jailbreak Instance => instance;

		internal UserInterface UI;
		public GlyphItemsUI glyphItemUI;
		internal UserInterface modularUI;
		public ModularEditorUI modularUIState;
        public static Texture2D LiteralBackTexture;

		public Jailbreak() {}
        public override void Load() {
            instance = this;
            Actions = new Dictionary<string, ActionItem>(){
                { "Chat", new ChatAction() },
                { "GetCaster", new GetCasterOperation() },
                { "Num", new NumberLiteral() },
                { "Bool", new BooleanLiteral() },
                { "String", new StringLiteral() },
                { "Vec2", new Vec2Literal() },
                //{ "Vec3", new Vec3Literal() },
                { "Set", new SetParameterOperation() },
                { "Get", new GetParameterOperation() },
                { "Add", new AddOperation() },
                { "Sub", new SubtractOperation() },
                { "Mul", new MultiplyOperation() },
                { "Div", new DivideOperation() },
                { "Pow", new PowerOperation() },
                { "GetProjectile", new GetProjectileOperation() },
                { "GetPosition", new GetPositionOperation() },
                { "GetMotion", new GetMotionOperation() },
                { "AddMotion", new AddMotionAction() },
                { "Push", new PushParameterOperation() },
                { "Pop", new PopParameterOperation() },
                { "Sleep", new SleepControl() },
                { "Jump", new JumpIfControl() },
                { "Goto", new GotoControl() }
            };
            actions = new List<ActionItem>(){};
            foreach(KeyValuePair<string,ActionItem> act in Actions) {
                AddItem(act.Value.GetType().Name, act.Value);
                actions.Add(act.Value);
            }
            ActionContext.Default = new ActionContext();
			if (!Main.dedServ){
				UI = new UserInterface();
				modularUI = new UserInterface();
                LiteralBackTexture = GetTexture("UI/Literal_Back");
			}
        }
        public override void Unload() {
            actions = null;
            Actions = null;
            UI = null;
            modularUI = null;
            glyphItemUI = null;
            modularUIState = null;
            ActionContext.Default = null;
            LiteralBackTexture = null;
            instance = null;
        }
        public override void PostUpdateInput() {
            if(glyphItemUI?.literalUI is LiteralElement element && LiteralElement.focused) {
                element.DoKeyboardInput();
            }
        }
        public static ActionItem CreateNew(ActionItem old) {
            Item item = new Item();
            item.SetDefaults(old.mod.ItemType(old.Name));
            return (ActionItem)item.modItem;
        }
        public static string ConvertAlias(string alias) {
            List<char> chars = alias.ToCharArray().ToList();
            chars[0] = char.ToUpper(chars[0]);
            for(int i = 0; i < chars.Count; i++) {
                if(chars[i]=='_') {
                    chars.RemoveAt(i);
                    chars[i] = char.ToUpper(chars[i]);
                }
            }
            alias = new string(chars.ToArray());
            alias = alias.Replace("SetParameter","Set");
            alias = alias.Replace("GetParameter","Get");
            alias = alias.Replace("Number","Num");
            alias = alias.Replace("Boolean","Bool");
            alias = alias.Replace("+","Add");
            alias = alias.Replace("Subtract","Sub");
            alias = alias.Replace("-","Sub");
            alias = alias.Replace("Multiply","Mul");
            alias = alias.Replace("*","Mul");
            alias = alias.Replace("Divide","Div");
            alias = alias.Replace("/","Div");
            alias = alias.Replace("Power","Pow");
            alias = alias.Replace("Exp","Pow");
            alias = alias.Replace("Exponent","Pow");
            alias = alias.Replace("^","Pow");
            alias = alias.Replace("Vector2","Vec2");
            //alias = alias.Replace("Vector3","Vec3");
            alias = alias.Replace("PushParameter","Push");
            alias = alias.Replace("PopParameter","Pop");
            alias = alias.Replace("JumpIf","Jump");
            return alias;
        }
        public static int AddAction(ActionItem action, string name) {
            Actions.Add(name,action);
            actions.Add(action);
            return actions.Count-1;
        }
        public static ActionItem GetAction(string name, bool convertAlias = true) {
            try {
                if(convertAlias)name = ConvertAlias(name);
                return Actions[name];
            } catch(Exception) {
                return actions[int.Parse(name)];
            }
        }
        public static ActionItem GetActionByID(int id) {
            return actions[id];
        }
		public override void UpdateUI(GameTime gameTime) {
            if(!Main.playerInventory) {
                if(UI.CurrentState!=null){
				    UI.SetState(null);
			    }
                if(modularUI.CurrentState!=null){
				    modularUI.SetState(null);
			    }
            }
			UI?.Update(gameTime);
			modularUI?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
					"Jailbreak: ModularEditUI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						modularUI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
					"Jailbreak: DriveEditUI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						UI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
        public void OpenModularEditor() {
            modularUIState = new ModularEditorUI();
            modularUIState.Activate();
            modularUI.SetState(modularUIState);
        }
        public void OpenDriveEditor(DriveItem drive) {
            glyphItemUI = new GlyphItemsUI();
            glyphItemUI.drive = drive;
            glyphItemUI.Activate();
            UI.SetState(glyphItemUI);
        }
    }
    public static class JailbreakExt {
        public delegate ref T getRefDelegate<T>();
        public static bool ToBool(this object value) {
            try {
                return (long)value!=0;
            } catch(Exception) {
                return (bool)value;
            }
        }
        public class RefWrapper<T> {
            public T value;
            public RefWrapper(T v) {
                value = v;
            }
            public static bool operator ==(RefWrapper<T> t, T o) => EqualityComparer<T>.Default.Equals(t.value, o);
            public static bool operator !=(RefWrapper<T> t, T o) => !EqualityComparer<T>.Default.Equals(t.value, o);
            public static bool operator ==(T o, RefWrapper<T> t) => EqualityComparer<T>.Default.Equals(t.value, o);
            public static bool operator !=(T o, RefWrapper<T> t) => !EqualityComparer<T>.Default.Equals(t.value, o);
            public static bool operator <(RefWrapper<T> t, T o) => Comparer<T>.Default.Compare(t.value, o)<0;
            public static bool operator >(RefWrapper<T> t, T o) => Comparer<T>.Default.Compare(t.value, o)>0;
            public static bool operator <(T o, RefWrapper<T> t) => Comparer<T>.Default.Compare(o, t.value)<0;
            public static bool operator >(T o, RefWrapper<T> t) => Comparer<T>.Default.Compare(o, t.value)>0;
            public static bool operator <=(RefWrapper<T> t, T o) => Comparer<T>.Default.Compare(t.value, o)<=0;
            public static bool operator >=(RefWrapper<T> t, T o) => Comparer<T>.Default.Compare(t.value, o)>=0;
            public static bool operator <=(T o, RefWrapper<T> t) => Comparer<T>.Default.Compare(o, t.value)<=0;
            public static bool operator >=(T o, RefWrapper<T> t) => Comparer<T>.Default.Compare(o, t.value)>=0;
            public override string ToString() {
                return "ref "+value.ToString();
            }
            /* operator cannot be applied between operands of types 'T' and 'T'
            public static bool operator <(RefWrapper<T> t, T o) => t.value<o;
            public static bool operator >(RefWrapper<T> t, T o) => t.value>o;
            public static bool operator <(T o, RefWrapper<T> t) => o<t.value;
            public static bool operator >(T o, RefWrapper<T> t) => o>t.value;
            public static bool operator <=(RefWrapper<T> t, T o) => t.value<=o;
            public static bool operator >=(RefWrapper<T> t, T o) => t.value>=o;
            public static bool operator <=(T o, RefWrapper<T> t) => o<=t.value;
            public static bool operator >=(T o, RefWrapper<T> t) => o>=t.value;
            public static bool operator ==(RefWrapper<T> t, RefWrapper<T> o) => t.value==o.value;
            public static bool operator !=(RefWrapper<T> t, RefWrapper<T> o) => t.value!=o.value;//*/
            /*public static bool operator <=(RefWrapper<T> t, T o) {
                return t<=o;
            }
            public static bool operator >=(RefWrapper<T> t, T o) {
                return t>=o;
            }*/
            public static implicit operator RefWrapper<T>(T v) => new RefWrapper<T>(v);
        }
    }
    public enum ActionType {
        Action,
        Operation,
        Control,
        Literal
    }
}
