using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jailbreak.Items {
    public class JumpIfControl : ActionItem {
        public override float delay => 0;
        public override object Execute(int i) {
            ActionContext.Cursor+=(int)parameters[0];
            return null;
        }
    }
    public class SleepControl : ActionItem {
        public override float delay => (float)parameters[0];
    }
}
