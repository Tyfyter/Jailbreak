using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Jailbreak {
    public class ActionContext {
        public static ActionContext Default;
        public Entity Caster;
        public Entity Projectile;
        public Entity Target;
        public int Cursor;
        public float Delay;
        public List<object> parameters;
        public object lastReturn;
        public ActionContext() {
            parameters = new List<object>(){new object(),new object(),new object(),new object(),new object()};
        }
    }
}
