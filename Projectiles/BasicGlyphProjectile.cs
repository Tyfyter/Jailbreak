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
            context.Caster = Main.player[projectile.owner];
            int cycleCount = 0;
            object ret;
            if((glyphType&GlyphProjectileType.Tracer)!=0) {
                Dust.NewDustPerfect(projectile.Center, 267, new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f), 100, new Color(0,90,255), 0.75f).noGravity = true;
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
                    if(item.cost<=100) {
                        context.Delay+=item.delay;
                        ret = item.Execute(glyphType);
                        context.lastReturn = ret??context.lastReturn;
                    }
                } catch(Exception) {
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
        }
        public override void Kill(int timeLeft) {
            for(int i = 0; i<10; i++) {
                Dust.NewDustPerfect(projectile.Center, 267, Main.rand.NextVector2Circular(4,4)*Main.rand.NextFloat(0,1), 100, new Color(0, 90, 255), 0.75f).noGravity = true;
            }
        }
    }
    public static class GlyphProjectileType {
        public static byte Normal = 0b0;
        public static byte Tracer = 0b1;
        public static byte OnHit  = 0b10;
        public static byte Repeat = 0b100;
        public static byte Paused = 0b1000;
    }
}
