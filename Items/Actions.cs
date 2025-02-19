using Jailbreak.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Jailbreak.UI;

namespace Jailbreak.Items{
    public class ActionItem : ModItem {
        public List<object> parameters => context.parameters;
        public virtual bool hasLiteral => false;
        public virtual ActionType Type => ActionType.Action;
        public virtual float cost => 0.1f;
        public virtual float delay => Type==ActionType.Action?1:0.1f;
        public ActionContext context;
        protected internal static bool rightClicked = false;
        public virtual object Execute(int i){return null;}
		public override void AutoStaticDefaults() {
            try {
                /*switch(Type) {
                    case ActionType.Literal:
                    Main.itemTexture[item.type] = ModContent.GetTexture("Literal");
                    break;
                    default:*/
                Main.itemTexture[item.type] = ModContent.GetTexture("Jailbreak/"+Type.ToString()+"s/"+Name.Replace(Type.ToString(), ""));
                /*break;
            }*/
            } catch(Exception) {
                Main.itemTexture[item.type] = ModContent.GetTexture("Jailbreak/Empty");
            }

			if (DisplayName.IsDefault())DisplayName.SetDefault(Regex.Replace(Name, "(?<!^)([A-Z])", " $1").Trim());
		}
        public override bool Autoload(ref string name) {
            return false;
        }
        protected internal virtual ActionItem ApplyLiteral(string literal) { return this; }
        protected internal void SetLiteral(string literal) { ApplyLiteral(literal); }
        protected internal virtual string GetLiteral() { return ""; }
        public ActionItem() {
            context = ActionContext.Default;
        }
        public override void UpdateInventory(Player player) {
            rightClicked = false;
        }
        public override bool CanRightClick() {
            return hasLiteral;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
        public override void RightClick(Player player) {
            rightClicked = true;
        }

        public override bool Equals(object obj) => (obj is ActionItem)?(ActionItem)obj == this:false;

        /// <summary>
        /// override this if an action class contains data
        /// </summary>
        /// <returns>true</returns>
        protected virtual bool Equals(ActionItem other) {
            return true;
        }

        public override int GetHashCode() {
            var hashCode = 440132047;
            hashCode=hashCode*-1521134295+hasLiteral.GetHashCode();
            hashCode=hashCode*-1521134295+Type.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ActionItem a, ActionItem b) {
            if(a is null)return b is null;
            if(b is null)return false;//a can never be null here
            return (a.GetType()==b.GetType())?a.Equals(b):false;
        }
        public static bool operator !=(ActionItem a, ActionItem b) {
            if(a is null)return !(b is null);
            if(b is null)return true;//a can never be null here
            return (a.GetType()==b.GetType())?!a.Equals(b):true;
        }
    }
    /// <summary>
    /// see also <seealso cref="GetMotionOperation"/>
    /// </summary>
    public class AddMotionAction : ActionItem {
        public override float cost => ((Vector2)parameters[1]).Length()*(ReferenceEquals(parameters[0],context.Projectile)?0.25f:1);
        public override object Execute(int i){
            //ModContent.GetInstance<Jailbreak>().Logger.Info(parameters[0].GetType());
            //ModContent.GetInstance<Jailbreak>().Logger.Info(parameters[0] is Entity);
            Vector2 velocity = (Vector2)parameters[1];
            Entity target;
            List<Entity> targets = parameters[0].ToEntityList<Entity>();
            if(targets is null)return null;
            for(int j = 0; j < targets.Count; j++) {
                target = targets[j];
                target.velocity+=velocity;
                if(target is Player player) {
                    NetMessage.SendData(MessageID.SyncPlayer, number: player.whoAmI);
                } else if(target is NPC npc) {
                    npc.netUpdate = true;
                } else if(target is Projectile projectile) {
                    projectile.netUpdate = true;
                }
            }
            return null;
        }
    }
    public class ChatAction : ActionItem {
        public override float cost => 0;
        public override float delay => 0;
        public override object Execute(int i){
            Main.NewText(parameters[0] is null?"null":parameters[0].ToString());
            return null;
        }
    }
}