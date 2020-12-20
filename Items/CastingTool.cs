using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Jailbreak.Projectiles;

namespace Jailbreak.Items {
    public abstract class CastingTool : ModItem {
        protected List<ActionItem> actions;
        public virtual List<ActionItem> Actions => actions;
        protected byte projFields = GlyphProjectileType.Normal;
        public virtual byte ProjFields => projFields;
        public override void SetDefaults() {
            item.useStyle = 5;
            item.shoot = ModContent.ProjectileType<BasicGlyphProjectile>();
            item.shootSpeed = 9.5f;
            item.useAnimation = item.useTime = 30;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            BasicGlyphProjectile projectile = (BasicGlyphProjectile)Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), item.shoot, damage, knockBack, player.whoAmI).modProjectile;
            projectile.actions = Actions;
            projectile.glyphType = ProjFields;
            return false;
        }
    }
}
