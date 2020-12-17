using Jailbreak.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace Jailbreak {
	public class Jailbreak : Mod {

        internal static Dictionary<string,ActionItem> Actions;
        internal static List<ActionItem> actions;

		public Jailbreak() {}
        public override void Load() {
            Actions = new Dictionary<string, ActionItem>(){
                { "Chat", new ChatAction() },
                { "GetCaster", new GetCasterOperation() },
                { "Number", new NumberLiteral() },
                { "Bool", new BooleanLiteral() },
                { "String", new StringLiteral() },
                { "Vec2", new Vec2Literal() },
                { "Vec3", new Vec3Literal() },
                { "SetParameter", new SetParameterOperation() },
                { "GetParameter", new GetParameterOperation() },
                { "Add", new AddOperation() },
                { "Sub", new SubtractOperation() },
                { "Mul", new MultiplyOperation() },
                { "Div", new DivideOperation() },
                { "Pow", new PowerOperation() },
                { "GetProjectile", new GetProjectileOperation() },
                { "GetPosition", new GetPositionOperation() },
                { "GetMotion", new GetMotionOperation() },
                { "AddMotion", new AddMotionAction() }
            };
            actions = new List<ActionItem>(){};
            foreach(KeyValuePair<string,ActionItem> act in Actions) {
                //AddItem(act.Value.GetType().Name, act.Value);
                actions.Add(act.Value);
            }
            ActionContext.Default = new ActionContext();
        }
        public override void Unload() {
            actions = null;
            Actions = null;
            ActionContext.Default = null;
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
            alias = alias.Replace("Vector3","Vec3");
            return alias;
        }
        public static int AddAction(ActionItem action, string name) {
            Actions.Add(name,action);
            actions.Add(action);
            return actions.Count-1;
        }
        public static ActionItem GetAction(string name, bool convertAlias = true) {
            if(convertAlias)name = ConvertAlias(name);
            return Actions[name];
        }
        public static ActionItem GetActionByID(int id) {
            return actions[id];
        }
    }
    public static class JailbreakExt {
        public static bool ToBool(this object value) {
            try {
                return (long)value!=0;
            } catch(Exception) {
                return (bool)value;
            }
        }
    }
    public enum ActionType {
        Action,
        Operation,
        Flow,
        Literal
    }
}
