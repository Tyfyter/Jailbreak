using Jailbreak.Items;
using Jailbreak.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
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
		public AssemblerUI assemblerUIState;
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
            Sets.Item.casing = new bool[0];
            Sets.Item.drive = new bool[0];
            Sets.Item.lens = ItemID.Sets.Factory.CreateBoolSet(ItemID.Lens,ItemID.Sapphire,ItemID.Ruby,ItemID.MechanicalLens,ItemID.EyeoftheGolem);
            Sets.Item.battery = ItemID.Sets.Factory.CreateBoolSet(ItemID.StarinaBottle,ItemID.ShadowOrb,ItemID.CrimsonHeart,ItemID.LihzahrdPowerCell,ItemID.MechanicalBatteryPiece);
            NonFishItem.ResizeArrays+=()=>{
                Array.Resize(ref Sets.Item.casing, Item.staff.Length);
                Array.Resize(ref Sets.Item.drive, Item.staff.Length);
                Array.Resize(ref Sets.Item.lens, Item.staff.Length);
                Array.Resize(ref Sets.Item.battery, Item.staff.Length);
            };
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
            Sets.Item.casing = null;
            Sets.Item.drive = null;
            Sets.Item.lens = null;
            Sets.Item.battery = null;
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
            alias = alias.Replace("RemoveParameter","Pop");
            alias = alias.Replace("Remove","Pop");
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
            if(!(assemblerUIState is null)){
                assemblerUIState.Deactivate();
                assemblerUIState = null;
			}
            modularUIState = new ModularEditorUI();
            modularUIState.Activate();
            modularUI.SetState(modularUIState);
        }
        public void OpenAssemblerUI() {
            if(!(modularUIState is null)){
                modularUIState.Deactivate();
                modularUIState = null;
			}
            assemblerUIState = new AssemblerUI();
            assemblerUIState.Activate();
            modularUI.SetState(assemblerUIState);
        }
        public void OpenDriveEditor(DriveItem drive) {
            glyphItemUI = new GlyphItemsUI();
            glyphItemUI.drive = drive;
            glyphItemUI.maxSize = drive.maxSize;
            glyphItemUI.Activate();
            UI.SetState(glyphItemUI);
        }
    }
    public static class Sets {
        public static class Item {
            public static bool[] casing;
            public static bool[] drive;
            public static bool[] lens;
            public static bool[] battery;
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
        #region spritebatch extensions
        static SpriteSortMode sortMode;
        static BlendState blendState;
        static SamplerState samplerState;
        static DepthStencilState depthStencilState;
        static RasterizerState rasterizerState;
        static Effect effect;
        static Matrix transformMatrix;
        public static void Pause(this SpriteBatch spriteBatch) {
            try {
                if(_sortMode is null) {
                    _sortMode = typeof(SpriteBatch).GetField("spriteSortMode", BindingFlags.NonPublic|BindingFlags.Instance);
                    _blendState = typeof(SpriteBatch).GetField("blendState", BindingFlags.NonPublic|BindingFlags.Instance);
                    _samplerState = typeof(SpriteBatch).GetField("samplerState", BindingFlags.NonPublic|BindingFlags.Instance);
                    _depthStencilState = typeof(SpriteBatch).GetField("depthStencilState", BindingFlags.NonPublic|BindingFlags.Instance);
                    _rasterizerState = typeof(SpriteBatch).GetField("rasterizerState", BindingFlags.NonPublic|BindingFlags.Instance);
                    _effect = typeof(SpriteBatch).GetField("customEffect", BindingFlags.NonPublic|BindingFlags.Instance);
                    _transformMatrix = typeof(SpriteBatch).GetField("transformMatrix", BindingFlags.NonPublic|BindingFlags.Instance);
                }
                sortMode = (SpriteSortMode)_sortMode.GetValue(spriteBatch);
                blendState = (BlendState)_blendState.GetValue(spriteBatch);
                samplerState = (SamplerState)_samplerState.GetValue(spriteBatch);
                depthStencilState = (DepthStencilState)_depthStencilState.GetValue(spriteBatch);
                rasterizerState = (RasterizerState)_rasterizerState.GetValue(spriteBatch);
                effect = (Effect)_effect.GetValue(spriteBatch);
                transformMatrix = (Matrix)_transformMatrix.GetValue(spriteBatch);
                spriteBatch.End();
            } catch(InvalidOperationException e) {
			    throw new InvalidOperationException("called pause without spritebatch drawing", e);
            } catch(Exception e) {
			    throw new Exception("error while pausing spritebatch", e);
            }
        }
        public static void Resume(this SpriteBatch spriteBatch) {
            try {
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            } catch(InvalidOperationException e) {
			    throw new InvalidOperationException("called resume with spritebatch drawing", e);
            } catch(Exception e) {
			    throw new Exception("error while resuming spritebatch", e);
            }
        }
        private static FieldInfo _sortMode;
        private static FieldInfo _blendState;
        private static FieldInfo _samplerState;
        private static FieldInfo _depthStencilState;
        private static FieldInfo _rasterizerState;
        private static FieldInfo _effect;
        private static FieldInfo _transformMatrix;
        #endregion
        public class RefWrapper<T> {
            public T value;
            public RefWrapper(T v) => value = v;
            public override int GetHashCode() => value.GetHashCode();
            public override bool Equals(object o) => EqualityComparer<T>.Equals(value, o);
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
            public override string ToString() => "ref "+value.ToString();
            public static implicit operator RefWrapper<T>(T v) => new RefWrapper<T>(v);
        }
    }
    public sealed class NonFishItem : ModItem {
        public override string Texture => "Terraria/Item_2290";
        public static event Action ResizeArrays;
        public override bool IsQuestFish() {
            ResizeArrays();
            ResizeArrays = null;
            return false;
        }
    }
    public enum ActionType {
        Action,
        Operation,
        Control,
        Literal
    }
}
