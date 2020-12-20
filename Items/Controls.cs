using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Jailbreak.Items {
    public class JumpIfControl : ActionItem {
        public override bool hasLiteral => true;
        public override float delay => 0;
        public int count;
        public override object Execute(int i) {
            if(parameters[0].ToBool())ActionContext.Default.Cursor+=count;
            return null;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound {
                { "count", count }
            };
            return o;
        }
        public override void Load(TagCompound tag) {
            count = tag.Get<int>("count");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            count = int.Parse(literal);
            return this;
        }
    }
    public class SleepControl : ActionItem {
        public override float delay => (float)parameters[0];
    }
    public class GotoControl : ActionItem {
        public override bool hasLiteral => true;
        public override float delay => 0;
        public int? target;
        public override object Execute(int i) {
            ActionContext.Default.Cursor = target??(int)parameters[0];
            return null;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound();
            if(target.HasValue)o.Add("target",target.Value);
            return o;
        }
        public override void Load(TagCompound tag) {
            if(tag.ContainsKey("target"))target = tag.Get<int>("target");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(literal.Equals("null")) {
                target = null;
                return this;
            }
            target = int.Parse(literal);
            return this;
        }
    }
}
