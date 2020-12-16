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
            if(parameters[0].ToBool())ActionContext.Cursor+=count;
            return null;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound();
            o.Add("count", count);
            return o;
        }
        public override void Load(TagCompound tag) {
            count = tag.Get<int>("count");
        }
        protected internal override void ApplyLiteral(string literal) {
            count = int.Parse(literal);
        }
    }
    public class SleepControl : ActionItem {
        public override float delay => (float)parameters[0];
    }
}
