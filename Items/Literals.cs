using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
            TagCompound o = new TagCompound {
                { "value", value }
            };
            return o;
        }
        public override void Load(TagCompound tag) {
            value = tag.Get<T>("value");
        }
        protected internal override string GetLiteral() {
            return value+"";
        }
    }
    public class NumberLiteral : Literal<float> {
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(float.TryParse(literal, out float val))value = val;
            return this;
        }
    }
    public class BooleanLiteral : Literal<bool> {
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(bool.TryParse(literal, out bool val))value = val;
            return this;
        }
    }
    public class StringLiteral : Literal<string> {
        protected internal override ActionItem ApplyLiteral(string literal) {
            value = literal;
            return this;
        }
    }
    public class Vec2Literal : Literal<Vector2> {
        protected internal override ActionItem ApplyLiteral(string literal) {
            string[] literals = literal.Split(',');
            value = new Vector2(float.Parse(literals[0]),float.Parse(literals[1]));
            return this;
        }
    }
    /*public class Vec3Literal : Literal<Vector3> {
        protected internal override void ApplyLiteral(string literal) {
            string[] literals = literal.Split(',');
            value = new Vector3(float.Parse(literals[0]),float.Parse(literals[1]),float.Parse(literals[2]));
        }
    }*/
}
