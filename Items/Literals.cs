using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jailbreak.Items {
    public class Literal<T> : ActionItem {
        public override ActionType Type => ActionType.Literal;
        public override float delay => 0;
        public T value;
        public override object Execute(int i){
            return value;
        }
    }
}
