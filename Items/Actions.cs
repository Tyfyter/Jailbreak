using Jailbreak.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Jailbreak.Items{
    public class ActionItem : ModItem {
        public List<object> parameters => ActionContext.Default.parameters;
        public virtual bool hasLiteral => false;
        public virtual ActionType Type => ActionType.Action;
        public virtual float cost => 0.1f;
        public virtual float delay => Type==ActionType.Action?1:0.1f;
        public ActionContext context;
        public virtual object Execute(int i){return null;}
        public override bool Autoload(ref string name) {
            return false;
        }
        protected internal virtual void ApplyLiteral(string literal) {}
        public ActionItem() {
            context = ActionContext.Default;
        }
    }
    /// <summary>
    /// see also <seealso cref="GetMotionOperation"/>
    /// </summary>
    public class AddMotionAction : ActionItem {
        public override float cost => ((Vector2)parameters[1]).Length()*(ReferenceEquals(parameters[0],context.Projectile)?0.25f:1);
        public override object Execute(int i){
            ModContent.GetInstance<Jailbreak>().Logger.Info(parameters[0].GetType());
            ModContent.GetInstance<Jailbreak>().Logger.Info(parameters[0] is Entity);
            ((Entity)parameters[0]).velocity+=(Vector2)parameters[1];
            if(parameters[0] is Player player) {
                NetMessage.SendData(MessageID.SyncPlayer, number:player.whoAmI);
            }else if(parameters[0] is NPC npc) {
                npc.netUpdate = true;
            }else if(parameters[0] is Projectile projectile) {
                projectile.netUpdate = true;
            }
            return null;
        }
    }
    public class ChatAction : ActionItem {
        public override float cost => 0;
        public override float delay => 0;
        public override object Execute(int i){
            Main.NewText(parameters[0].ToString());
            return null;
        }
    }
}