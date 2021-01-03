using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
//using System.Runtime.CompilerServices;

namespace Jailbreak.Items {
    public abstract class DriveItem : ModItem {
        protected List<ActionItem> actions;
        public virtual List<ActionItem> Actions {
            get => actions;
            set => actions = value;
        }
        public virtual bool Readonly => false;
        public virtual int maxSize => 0;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
            Sets.Item.drive[item.type] = true;
        }
        public override TagCompound Save() {
            try {
                return new TagCompound() {
                    {"actions", Actions.Select((i)=>i.item??Jailbreak.CreateNew(i).item).ToList()}
                };
            } catch(Exception) {
                return new TagCompound();
            }
        }
        public override void Load(TagCompound tag) {
            if(tag.ContainsKey("actions")) {
                Actions = tag.Get<List<Item>>("actions").Select((i) => (ActionItem)i.modItem).ToList();
            }
        }
    }
    //public class RADrive : DriveItem {}
    public class TestingDrive : DriveItem {
        public override List<ActionItem> Actions {
            get => TestItem.actions;
            set => TestItem.actions = value;
        }
        public override string Texture => "Jailbreak/Items/RAM1";
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenDriveEditor(this);
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
    }
    public class SlabDrive : DriveItem {
        public override bool Readonly {
            get {
                if(!Main.LocalPlayer.adjTile[TileID.HeavyWorkBench])return true;
                int i = Jailbreak.instance.glyphItemUI.currIndex;
                return i<actions.Count&&actions.Count>0;
            }
        }
        public override int maxSize => 8;
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenDriveEditor(this);
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneSlab);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if(actions is null) {
                Main.itemTexture[item.type] = Textures.Item.SlabDrive;
            } else {
                Main.itemTexture[item.type] = actions.Count<maxSize?Textures.Item.SlabDrive:Textures.Item.SlabDriveFull;
            }
            return true;
        }
    }
}
