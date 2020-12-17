using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Jailbreak.Items;

namespace Jailbreak.Projectiles {
    public class BasicGlyphProjectile : ModProjectile {
        public override string Texture => "Jailbreak/Items/TestItem";
        public ActionContext context;
        public List<ActionItem> actions;
        public override void SetDefaults() {
            context = new ActionContext();
            context.Projectile = projectile;
        }
        public override void AI() {
            ActionItem item;
            bool fail;
            context.Caster = Main.player[projectile.owner];
            int cycleCount = 0;
            object ret;
            while(context.Delay<1) {
                //try {
                    item = actions[context.Cursor++];
                    item.context = context;
                    fail = context.Cursor>=actions.Count||(++cycleCount>255);
                    if(true) {
                        context.Delay+=item.delay;
                        ret = item.Execute(0);
                        context.lastReturn = ret??context.lastReturn;
                    }
                    if(fail) {
                        projectile.Kill();
                        return;
                    }
                /*} catch(Exception) {
                    projectile.Kill();
                    return;
                }*/
            }
            context.Delay--;
        }
    }
}
