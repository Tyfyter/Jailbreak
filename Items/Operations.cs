﻿using Microsoft.Xna.Framework;
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
    /// <summary>
    /// see also <seealso cref="GetProjectileOperation"/> and <seealso cref="GetTargetOperation"/>
    /// </summary>
    public class GetCasterOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return context.Caster;
        }
    }
    /// <summary>
    /// see also <seealso cref="GetCasterOperation"/> and <seealso cref="GetTargetOperation"/>
    /// </summary>
    public class GetProjectileOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return context.Projectile;
        }
    }
    /// <summary>
    /// see also <seealso cref="GetCasterOperation"/> and <seealso cref="GetProjectileOperation"/>
    /// </summary>
    public class GetTargetOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            return context.Target;
        }
    }
    public class NormalizeOperation : ActionItem {
        public override float cost => 0.25f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2.SafeNormalize(default);
            //if(parameters[0] is Vector3 vec3)return Vector3.Normalize(vec3);
            return Math.Sign((float)parameters[0]);
        }
    }
    public class AddOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2+(Vector2)parameters[1];
            if(parameters[1] is Vector2 vec2_1)return vec2_1+(Vector2)parameters[0];
            //if(parameters[0] is Vector3 vec3)return vec3+(Vector3)parameters[1];
            return (float)parameters[0]+(float)parameters[1];
        }
    }
    public class SubtractOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2-(Vector2)parameters[1];
            if(parameters[1] is Vector2 vec2_1)return (Vector2)parameters[0]-vec2_1;
            //if(parameters[0] is Vector3 vec3)return vec3-(Vector3)parameters[1];
            return (float)parameters[0]-(float)parameters[1];
        }
    }
    public class MultiplyOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2*((parameters[1] as Vector2?)??new Vector2((float)parameters[1]));
            if(parameters[1] is Vector2 vec2_1)return vec2_1*((parameters[0] as Vector2?)??new Vector2((float)parameters[0]));
            //if(parameters[0] is Vector3 vec3)return vec3*((parameters[1] as Vector3?)??new Vector3((float)parameters[1]));
            return (float)parameters[0]-(float)parameters[1];
        }
    }
    public class DivideOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2)return vec2/((parameters[1] as Vector2?)??new Vector2((float)parameters[1]));
            if(parameters[1] is Vector2 vec2_1)return ((parameters[0] as Vector2?)??new Vector2((float)parameters[0]))/vec2_1;
            //if(parameters[0] is Vector3 vec3)return vec3/((parameters[1] as Vector3?)??new Vector3((float)parameters[1]));
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
    /// <summary>
    /// see also <seealso cref="MaxOperation"/>
    /// </summary>
    public class MinOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2) {
                float f = (float)parameters[1];
                return vec2.Length()>f ?Vector2.Normalize(vec2)*f:vec2;
            }
            if(parameters[1] is Vector2 vec2_1) {
                float f = (float)parameters[0];
                return vec2_1.Length()>f ?Vector2.Normalize(vec2_1)*f:vec2_1;
            }
            /*if(parameters[0] is Vector3 vec3) {
                float f = (float)parameters[1];
                return vec3.Length()>f ?Vector3.Normalize(vec3)*f:vec3;
            }*/
            return Math.Min((float)parameters[0],(float)parameters[1]);
        }
    }
    /// <summary>
    /// see also <seealso cref="MinOperation"/>
    /// </summary>
    public class MaxOperation : ActionItem {
        public override float cost => 0.1f;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            if(parameters[0] is Vector2 vec2) {
                float f = (float)parameters[1];
                return vec2.Length()<f ?Vector2.Normalize(vec2)*f:vec2;
            }
            if(parameters[1] is Vector2 vec2_1) {
                float f = (float)parameters[0];
                return vec2_1.Length()<f ?Vector2.Normalize(vec2_1)*f:vec2_1;
            }
            /*if(parameters[0] is Vector3 vec3) {
                float f = (float)parameters[1];
                return vec3.Length()<f ?Vector3.Normalize(vec3)*f:vec3;
            }*/
            return Math.Max((float)parameters[0],(float)parameters[1]);
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
            ModContent.GetInstance<Jailbreak>().Logger.Info(context.ToString());
            return ((Entity)parameters[0]).velocity;
        }
    }
    /// <summary>
    /// see also <seealso cref="GetParameterOperation"/>
    /// </summary>
    public class SetParameterOperation : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0;
        public int index;
        public override object Execute(int i){
            ModContent.GetInstance<Jailbreak>().Logger.Info($"set parameter {index} to {context.lastReturn}");
            parameters[index] = context.lastReturn;
            return null;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound {
                { "index", index }
            };
            return o;
        }
        public override void Load(TagCompound tag) {
            index = tag.Get<int>("index");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(int.TryParse(literal, out int i))index = i;
            return this;
        }
        protected internal override string GetLiteral() {
            return index+"";
        }
    }
    /// <summary>
    /// see also <seealso cref="SetParameterOperation"/>
    /// </summary>
    public class GetParameterOperation : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0;
        public int index;
        public override object Execute(int i){
            return parameters[index];
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound {
                { "index", index }
            };
            return o;
        }
        public override void Load(TagCompound tag) {
            index = tag.Get<int>("index");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(int.TryParse(literal, out int i))index = i;
            return this;
        }
        protected internal override string GetLiteral() {
            return index+"";
        }
    }
    public class GetListSizeOperation : ActionItem {
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0.25f;
        public override object Execute(int i){
            return ((ICollection)parameters[0]).Count;
        }
    }
    /// <summary>
    /// see also <seealso cref="PopParameterOperation"/>
    /// </summary>
    public class PushParameterOperation : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0f;
        public int index = 0;
        public override object Execute(int i){
            parameters.Insert(index,context.lastReturn);
            mod.Logger.Info($"pushed, parameters now [{string.Join(", ", parameters)}]");
            return null;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound {
                { "index", index }
            };
            return o;
        }
        public override void Load(TagCompound tag) {
            index = tag.Get<int>("index");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(int.TryParse(literal, out int i))index = i;
            return this;
        }
        protected internal override string GetLiteral() {
            return index+"";
        }
    }
    /// <summary>
    /// see also <seealso cref="PushParameterOperation"/>
    /// </summary>
    public class PopParameterOperation : ActionItem {
        public override bool hasLiteral => true;
        public override ActionType Type => ActionType.Operation;
        public override float cost => 0f;
        public int index = 0;
        public override object Execute(int i){
            //object o = parameters[0];
            parameters.RemoveAt(index);
            mod.Logger.Info($"popped, parameters now [{string.Join(", ", parameters)}]");
            return null;//o;
        }
        public override TagCompound Save() {
            TagCompound o = new TagCompound {
                { "index", index }
            };
            return o;
        }
        public override void Load(TagCompound tag) {
            index = tag.Get<int>("index");
        }
        protected internal override ActionItem ApplyLiteral(string literal) {
            if(int.TryParse(literal, out int i))index = i;
            return this;
        }
        protected internal override string GetLiteral() {
            return index+"";
        }
    }
    public class GetNearbyEnemiesOperation : ActionItem {
        public override float cost => (float)parameters[(parameters[0] is Vector2||parameters[0] is Entity)?1:0];
        public override float delay => 2;
        public override ActionType Type => ActionType.Operation;
        public override object Execute(int i){
            Vector2 position = context.Projectile.Center;
            int rangeIndex = 0;
            if(parameters[0] is Vector2 vec) {
                position = vec;
                rangeIndex++;
            }else if(parameters[0] is Entity ent) {
                position = ent.Center;
                rangeIndex++;
            }
            float range = (float)parameters[rangeIndex]*16;
            List<NPC> targets = new List<NPC>{};
            NPC targ;
            for(int n = 0; n<Main.maxNPCs; n++) {
                targ = Main.npc[n];
                if(targ.active&&targ.CanBeChasedBy()&&(targ.Center-position).Length()<range) {
                    targets.Add(targ);
                }
            }
            return targets;
        }
    }
}
