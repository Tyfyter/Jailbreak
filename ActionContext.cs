using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Jailbreak {
    public static class ActionContext {
        public static Entity Caster;
        public static Entity Projectile;
        public static Entity Target;
        public static int Cursor;
        public static List<object> parameters;
        public static object lastReturn;
    }
}
