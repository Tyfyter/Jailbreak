using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Jailbreak.Items {
    public abstract class Literal<T> : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Literal;
        public override float delay => 0;
        public T value;
        public override object Execute(int i){
            return value;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound();
            o.Add("value", value);
            return o;
        }
        public override void Load(TagCompound tag) {
            value = tag.Get<T>("value");
        }
    }
    public class NumberLiteral : Literal<float> {
        protected internal override void ApplyLiteral(string literal) {
            value = float.Parse(literal);
        }
    }
    public class BooleanLiteral : Literal<bool> {
        protected internal override void ApplyLiteral(string literal) {
            value = bool.Parse(literal);
        }
    }
    public class StringLiteral : Literal<string> {
        protected internal override void ApplyLiteral(string literal) {
            value = literal;
        }
    }
    public class Vec2Literal : Literal<Vector2> {
        protected internal override void ApplyLiteral(string literal) {
            string[] literals = literal.Split(',');
            value = new Vector2(float.Parse(literals[0]),float.Parse(literals[1]));
        }
    }
    public class Vec3Literal : Literal<Vector3> {
        protected internal override void ApplyLiteral(string literal) {
            string[] literals = literal.Split(',');
            value = new Vector3(float.Parse(literals[0]),float.Parse(literals[1]),float.Parse(literals[2]));
        }
    }
}
