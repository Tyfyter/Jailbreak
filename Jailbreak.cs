using Jailbreak.Items;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Jailbreak {
	public class Jailbreak : Mod {

        internal static List<ActionItem> actions;

		public Jailbreak() {}
        public override void Load() {
            actions = new List<ActionItem>(){
                new GetCasterOperation(),
                new GetMotionOperation(),
                new AddMotionAction(),
            };
            foreach(ActionItem act in actions) {
                AddItem(act.GetType().Name, act);
            }
        }
        public override void Unload() {
            actions = null;
        }
    }
    public enum ActionType {
        Action,
        Operation,
        Flow,
        Literal
    }
}
