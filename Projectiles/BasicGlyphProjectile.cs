using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Jailbreak.Items;
using static Jailbreak.Projectiles.GlyphProjectileType;
using static Jailbreak.JailbreakExt;

namespace Jailbreak.Projectiles {
    public class BasicGlyphProjectile : ModProjectile {
        public override string Texture => "Jailbreak/Items/TestItem";
        public ActionContext context;
        public List<ActionItem> actions;
        public byte glyphType = GlyphProjectileType.Normal;
        public override void SetDefaults() {
            context = new ActionContext();
            context.Projectile = projectile;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }
        public override void AI() {
            ActionItem item;
            bool fail;
            if(context.Caster is null)context.Caster = Main.player[projectile.owner];
            int cycleCount = 0;
            object ret;
            if((glyphType&GlyphProjectileType.Tracer)!=0) {
                Dust.NewDustPerfect(projectile.Center, 267, new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f), 100, context.color, 0.75f).noGravity = true;
            }
            RefWrapper<float> charge = context.charge;
            if(context.costMult==-1&&context.Caster is Player player) {
                charge.value = player.statMana/20;
            }
            while(context.Delay<1&&(glyphType&GlyphProjectileType.Paused)==0) {
                try {
                    fail = context.Cursor>=actions.Count||(++cycleCount>255);
                    if(fail) {
                        if((glyphType&GlyphProjectileType.Repeat)!=0) {
                            context.Cursor = 0;
                            if((glyphType&GlyphProjectileType.OnHit)!=0) {
                                glyphType|=GlyphProjectileType.Paused;
                            }
                        } else {
                            projectile.Kill();
                        }
                        return;
                    }
                    item = actions[context.Cursor++];
                    item.context = context;
                    float cost = item.cost;
                    if(context.costMult>0) {
                        cost*=context.costMult;
                    }
                    if(cost<=(charge?.value??100)) {
                        if(!(charge is null)) {
                            ConsumeCharge(cost);
                        }
                        float delay = item.delay;
                        if(!(item is SleepControl)) {//if anyone has a reason why this should be a property instead of being uniqe to SleepControl, please tell me
                            switch(context.delayMult) {
                                case -1:
                                delay = 15;
                                break;
                                default:
                                delay*=context.delayMult;
                                break;
                            }
                        }
                        context.Delay+=delay;
                        ret = item.Execute(glyphType);
                        context.lastReturn = ret??context.lastReturn;
                    } else {
                        context.Cursor = actions.Count;
                        mod.Logger.Info($"Didn't have {cost} charge for {item.GetType()} at {context.Cursor}");
                    }
                } catch(InvalidTimeZoneException) {
                    projectile.Kill();
                    return;
                }
            }
            while(context.parameters.Count>16) {
                context.parameters.RemoveAt(16);
            }
            if((glyphType&GlyphProjectileType.Paused)==0)context.Delay--;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            context.Target = target;
            if((glyphType&(Paused|OnHit))==(Paused|OnHit))glyphType^=GlyphProjectileType.Paused;
            if(crit&&(glyphType&Golem)==Golem)ConsumeCharge(-damage/2);
        }
        public override void Kill(int timeLeft) {
            for(int i = 0; i<10; i++) {
                Dust.NewDustPerfect(projectile.Center, 267, Main.rand.NextVector2Circular(4,4)*Main.rand.NextFloat(0,1), 100, context.color, 0.75f).noGravity = true;
            }
        }
        void ConsumeCharge(float cost) {
            switch(context.costMult) {
                case -1:
                if(context.Caster is Player player) {
                    player.statMana-=(int)Math.Ceiling(cost*player.manaCost*20);
                    context.charge.value = player.statMana/20;
                }else goto default;
                break;
                default:
                context.charge.value-=cost;
                break;
            }
        }
    }
    public static class GlyphProjectileType {
        public const byte Normal = 0b0;
        public const byte Tracer = 0b1;
        public const byte OnHit  = 0b10;
        public const byte Repeat = 0b100;
        public const byte Paused = 0b1000;
        public const byte Golem  = 0b10000;
    }
}
