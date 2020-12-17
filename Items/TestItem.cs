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
    public class TestItem : ModItem {
        internal static List<ActionItem> actions;
        public override void SetDefaults() {
            item.useStyle = 5;
            item.shoot = ModContent.ProjectileType<BasicGlyphProjectile>();
            item.shootSpeed = 9.5f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            BasicGlyphProjectile projectile = (BasicGlyphProjectile)Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), item.shoot, 0, 0, player.whoAmI).modProjectile;
            projectile.actions = actions;
            return false;
        }
    }
}
