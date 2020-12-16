using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;

namespace Jailbreak.Items {
    public class GetCasterOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return ActionContext.Caster;
        }
    }
    public class GetProjectileOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return ActionContext.Projectile;
        }
    }
    public class GetTargetOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return ActionContext.Target;
        }
    }
    public class NormalizeOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2.SafeNormalize(default);
            if(parameters[0] is Vector3 vec3)return Vector3.Normalize(vec3);
            return Math.Sign((float)parameters[0]);
        }
    }
    public class AddOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2+(Vector2)parameters[1];
            if(parameters[0] is Vector3 vec3)return vec3+(Vector3)parameters[1];
            return (float)parameters[0]+(float)parameters[1];
        }
    }
    public class SubtractOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2-(Vector2)parameters[1];
            if(parameters[0] is Vector3 vec3)return vec3-(Vector3)parameters[1];
            return (float)parameters[0]-(float)parameters[1];
        }
    }
    public class MultiplyOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2*((parameters[1] as Vector2?)??new Vector2((float)parameters[1]));
            if(parameters[0] is Vector3 vec3)return vec3*((parameters[1] as Vector3?)??new Vector3((float)parameters[1]));
            return (float)parameters[0]-(float)parameters[1];
        }
    }
    public class DivideOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2/((parameters[1] as Vector2?)??new Vector2((float)parameters[1]));
            if(parameters[0] is Vector3 vec3)return vec3/((parameters[1] as Vector3?)??new Vector3((float)parameters[1]));
            return (float)parameters[0]-(float)parameters[1];
        }
    }
    public class PowerOperation : ActionItem {
        public override float cost => 0.2f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return Math.Pow((float)parameters[0],(float)parameters[1]);
        }
    }
    public class GetPositionOperation : ActionItem {
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0.25f;
        public override object Execute(int i){
            return ((Entity)parameters[0]).Center;
        }
    }
    /// <summary>
    /// see also <seealso cref="AddMotionAction"/>
    /// </summary>
    public class GetMotionOperation : ActionItem {
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0.25f;
        public override object Execute(int i){
            return ((Entity)parameters[0]).velocity;
        }
    }
    public class SetParameterOperation : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0;
        public int index;
        public override object Execute(int i){
            ModContent.GetInstance<Jailbreak>().Logger.Info($"set parameter {index} to {ActionContext.lastReturn}");
            parameters[index] = ActionContext.lastReturn;
            return null;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound();
            o.Add("index", index);
            return o;
        }
        public override void Load(TagCompound tag) {
            index = tag.Get<int>("index");
        }
        protected internal override void ApplyLiteral(string literal) {
            index = int.Parse(literal);
        }
    }
    public class GetListSizeOperation : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0.25f;
        public override object Execute(int i){
            return ((ICollection)parameters[0]).Count;
        }
    }
    public class PushParameterOperation : ActionItem {
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0f;
        public override object Execute(int i){
            parameters.Insert(0,null);
            return null;
        }
    }
    public class PopParameterOperation : ActionItem {
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0f;
        public override object Execute(int i){
            object o = parameters[0];
            parameters.RemoveAt(0);
            return o;
        }
    }
}
