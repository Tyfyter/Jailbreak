using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Jailbreak.Items {
    public class JumpIfControl : ActionItem {
        public override bool hasLiteral => true;
        public override float delay => 0.05f;
        public int count;
        public override object Execute(int i) {
            if(parameters[0].ToBool())context.Cursor+=count;
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
        public override float delay => time??(float)parameters[0];
        public float? time;
        public override TagCompound Save() {
            TagCompound o = new TagCompound();
            if(time.HasValue)o.Add("time",time.Value);
            return o;
        }
        public override void Load(TagCompound tag) {
            if(tag.ContainsKey("time"))time = tag.Get<float>("time");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(literal.Equals("null")) {
                time = null;
                return this;
            }
            time = float.Parse(literal);
            return this;
        }
    }
    public class GotoControl : ActionItem {
        public override bool hasLiteral => true;
        public override float delay => 0.05f;
        public int? target;
        public override object Execute(int i) {
            context.Cursor = target??(int)parameters[0];
            mod.Logger.Info($"returned to {context.Cursor} with parameters [{string.Join(", ", parameters)}]");
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
