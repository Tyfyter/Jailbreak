using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

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
            if(parameters[0] is Vector2 vec2)return vec2-((parameters[1] as Vector2?)??new Vector2((float)parameters[1]));
            if(parameters[0] is Vector3 vec3)return vec3-((parameters[1] as Vector3?)??new Vector3((float)parameters[1]));
            return (float)parameters[0]-(float)parameters[1];
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
    public class SetParameterOperaion : ActionItem {
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0;
        public int index;
        public override object Execute(int i){
            parameters[index] = ActionContext.lastReturn;
            return null;
        }
    }
}
