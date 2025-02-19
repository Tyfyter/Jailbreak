﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static Jailbreak.JailbreakExt;

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
        public float delayMult = 1f;
        public float costMult = 1f;
        public RefWrapper<float> charge;//getRefDelegate<float> charge;
        public Color color = new Color(0,90,255);
        public ActionContext() {
            parameters = new List<object>(){new object(),new object(),new object(),new object(),new object()};
        }
    }
}
