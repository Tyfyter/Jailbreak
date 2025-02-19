﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Jailbreak.Projectiles;
using static Jailbreak.JailbreakExt;
using static Jailbreak.Projectiles.GlyphProjectileType;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Jailbreak.Items {
    public abstract class CastingTool : ModItem {
        protected int lastProj;
        protected List<ActionItem> actions;
        public virtual List<ActionItem> Actions => actions;
        public virtual RefWrapper<float> getCharge => null;
        public virtual float maxCharge => 100;
        public virtual float chargeRate => PowerCellGlobalItem.DefaultRecharge;
        protected byte projFields = GlyphProjectileType.Normal;
        public virtual byte ProjFields => projFields;
        public override void SetDefaults() {
            item.useStyle = 5;
            item.shoot = ModContent.ProjectileType<BasicGlyphProjectile>();
            item.shootSpeed = 9.5f;
            item.useAnimation = item.useTime = 30;
            item.noMelee = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            BasicGlyphProjectile projectile = (BasicGlyphProjectile)Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), item.shoot, damage, knockBack, player.whoAmI).modProjectile;
            projectile.actions = Actions;
            projectile.glyphType = ProjFields;
            lastProj = projectile.projectile.whoAmI;
            return false;
        }
        public void Recharge(){
            if(getCharge<maxCharge) {
                getCharge.value = Math.Min(getCharge.value+chargeRate, maxCharge);
                if(getCharge>=maxCharge) {
                    Main.NewText("fully charged");
                }
            }else if(getCharge>maxCharge) {
                getCharge.value = maxCharge;
            }
        }
    }
    public class ModularCastingTool : CastingTool {
        public Ref<Item> driveItem;
        public DriveItem drive => driveItem.Value.modItem as DriveItem;
        public Ref<Item> powerCellItem;
        public PowerCellItem powerCell => powerCellItem.Value.modItem as PowerCellItem;
        public override List<ActionItem> Actions => drive.Actions;
        public override RefWrapper<float> getCharge => (powerCell is null)?powerCellItem.Value.GetGlobalItem<PowerCellGlobalItem>().charge:powerCell.charge;
        public override float maxCharge => PowerCellGlobalItem.maxCharge(powerCellItem.Value);
        public override float chargeRate {
            get {
                if(!(powerCell is null))return powerCell.maxCharge;
                switch(powerCellItem.Value.type) {
                    case ItemID.MechanicalBatteryPiece:
                    return PowerCellGlobalItem.MechRecharge;
                    default:
                    return PowerCellGlobalItem.DefaultRecharge;
                }
            }
        }
        public override byte ProjFields => Normal;
        public override void SetDefaults() {
            base.SetDefaults();
            item.damage = 30;
            if(driveItem is null) {
                driveItem = new Ref<Item>(new Item());
                driveItem.Value.SetDefaults(0);
            }
            if(powerCellItem is null) {
                powerCellItem = new Ref<Item>(new Item());
                powerCellItem.Value.SetDefaults(0);
            }
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenModularEditor();
            Jailbreak.instance.modularUIState.SafeAddItemSlot(ref driveItem, ValidItemFunc:(i)=>i.IsAir||i.modItem is DriveItem);
            Jailbreak.instance.modularUIState.SafeAddItemSlot(ref powerCellItem, ValidItemFunc:(i)=>i.IsAir||Sets.Item.battery[i.type]);
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
        public override void UpdateInventory(Player player) {
            try {
                if(powerCellItem.Value is Item i)i.owner = item.owner;
                Recharge();
            } catch(NullReferenceException) {
                if(driveItem is null) {
                    driveItem = new Ref<Item>(new Item());
                    driveItem.Value.SetDefaults(0);
                }
                if(powerCellItem is null) {
                    powerCellItem = new Ref<Item>(new Item());
                    powerCellItem.Value.SetDefaults(0);
                }
            }
        }
        public override TagCompound Save() {
            try {
                return new TagCompound() {
                    {"drive", driveItem.Value},
                    {"powerCell", powerCellItem.Value}
                };
            } catch(Exception) {
                return new TagCompound();
            }
        }
        public override void Load(TagCompound tag) {
            if(tag.ContainsKey("drive")) {
                driveItem = new Ref<Item>(tag.Get<Item>("drive"));
            }
            if(tag.ContainsKey("powerCell")) {
                powerCellItem = new Ref<Item>(tag.Get<Item>("powerCell"));
            }
            if(driveItem is null) {
                driveItem = new Ref<Item>(new Item());
                driveItem.Value.SetDefaults(0);
            }
            if(powerCellItem is null) {
                powerCellItem = new Ref<Item>(new Item());
                powerCellItem.Value.SetDefaults(0);
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            float c = maxCharge;
            RefWrapper<float> projCharge = getCharge;
            //Main.NewText($"{getCharge}/{c}");
            if(powerCellItem.Value.type==ItemID.LihzahrdPowerCell) {
                RefWrapper<float> charge = powerCellItem.Value.GetGlobalItem<PowerCellGlobalItem>().charge;
                if(charge.value<PowerCellGlobalItem.LihzahrdMaxCharge) {
                    return false;
                }
                projCharge = maxCharge;
                charge.value = 0;
            }
            base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            ((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context.charge = projCharge;
            ((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context.Caster = player;
            return false;
            /*RefWrapper<float> c = maxCharge;
            getRefDelegate<float> chargeDel = getCharge;
            if(powerCellItem.type==ItemID.LihzahrdPowerCell) {
                PowerCellGlobalItem global = powerCellItem.GetGlobalItem<PowerCellGlobalItem>();
                if(global.charge<PowerCellGlobalItem.LihzahrdMaxCharge) {
                    return false;
                }
                chargeDel = () => { return ref c.value; };
                global.charge = 0;
            }
            base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            ((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context.charge = chargeDel;
            return false;*/
        }
    }
    public class CustomCastingTool : CastingTool {
        public override bool CloneNewInstances => true;
        public Ref<Item> driveItem;
        public DriveItem drive => driveItem.Value.modItem as DriveItem;
        public Item powerCellItem;
        public PowerCellItem powerCell => powerCellItem.modItem as PowerCellItem;
        public Item casingItem;
        public CasingItem casing => casingItem.modItem as CasingItem;
        public Item lensItem;
        public override List<ActionItem> Actions => drive.Actions;
        public override RefWrapper<float> getCharge {
            get {
                try {
                    return (powerCell is null) ? powerCellItem.GetGlobalItem<PowerCellGlobalItem>().charge : powerCell.charge;
                } catch(Exception e) {
                    return 100;
                }
            }
        }
        public override float maxCharge => PowerCellGlobalItem.maxCharge(powerCellItem);
        public override float chargeRate {
            get {
                if(!(powerCell is null))return 1;//powerCell.;
                switch(powerCellItem.type) {
                    case ItemID.MechanicalBatteryPiece:
                    return PowerCellGlobalItem.MechRecharge;
                    default:
                    return PowerCellGlobalItem.DefaultRecharge/4;
                }
            }
        }
        public override byte ProjFields => LensGlobalItem.projType(lensItem);
        public override void SetStaticDefaults() {
            //Terraria.ModLoader.ModContent.GetInstance<Jailbreak.Items.CustomCastingTool>().SetStaticDefaults();
            Tooltip.SetDefault("ModifyTooltips please add details\nlens\ndrive");
        }
        public override void SetDefaults() {
            if(item.owner<0||item.owner>=Main.player.Length||!Main.player[item.owner].active) {
                return;
            }
            base.SetDefaults();
            if(driveItem is null) {
                driveItem = new Ref<Item>(new Item());
                driveItem.Value.SetDefaults(0);
            }
            if(powerCellItem is null) {
                powerCellItem = new Item();
                powerCellItem.SetDefaults(0);
            }
            if(lensItem is null) {
                lensItem = new Item();
                lensItem.SetDefaults(0);
            }
            if(casingItem is null) {
                casingItem = new Item();
                casingItem.SetDefaults(0);
            }
            int dmg = item.damage;
            float damage = 1f;
            if(!(casing is null))casing.getStats(item, ref damage);
            if(!(powerCell is null))powerCell.getStats(item, ref damage);
            LensGlobalItem.getStats(lensItem, item, ref damage, ref item.shootSpeed);
            if(item.damage==dmg) {
                item.damage = (int)damage;
            }
        }
        public override bool AltFunctionUse(Player player) {
            return false;//true;
        }
        public override void RightClick(Player player) {
            Jailbreak.instance.OpenModularEditor();
            Jailbreak.instance.modularUIState.SafeAddItemSlot(ref driveItem, ValidItemFunc:(i)=>i.IsAir||i.modItem is DriveItem);
        }
        public override bool CanRightClick() {
            return true;
        }
        public override bool ConsumeItem(Player player) {
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            foreach(TooltipLine line in tooltips) {
                if(line.Name.Equals("Tooltip0")) {
                    line.text = $"Casing: {casingItem?.Name??"null"}\nPower Cell: {powerCellItem?.Name??"null"}; charge:{getCharge.value}";
                } else
                if(line.Name.Equals("Tooltip1")) {
                    line.text = $"Lens: {lensItem?.Name??"null"}";
                    line.overrideColor = LensGlobalItem.getColor(lensItem);
                }else
                if(line.Name.Equals("Tooltip2")) {
                    string name = driveItem?.Value?.Name;
                    if(string.IsNullOrWhiteSpace(name))name = "none";
                    line.text = $"Drive: {name}";
                    break;
                }
            }
        }
        public override void UpdateInventory(Player player) {
            try {
                if(item.useStyle == 0&&!(item.owner<0||item.owner>=Main.player.Length||!Main.player[item.owner].active))SetDefaults();
                if(powerCellItem is Item i)i.owner = item.owner;
                Recharge();
            } catch(NullReferenceException) {
                if(driveItem is null) {
                    driveItem = new Ref<Item>(new Item());
                    driveItem.Value.SetDefaults(0);
                }
                if(powerCellItem is null) {
                    powerCellItem = new Item();
                    powerCellItem.SetDefaults(0);
                }
            }
        }
        public override TagCompound Save() {
            try {
                return new TagCompound() {
                    {"drive", driveItem.Value},
                    {"powerCell", powerCellItem},
                    {"casing", casingItem},
                    {"lens", lensItem}
                };
            } catch(Exception) {
                return new TagCompound();
            }
        }
        public override void Load(TagCompound tag) {
            if(tag.ContainsKey("drive")) {
                driveItem = new Ref<Item>(tag.Get<Item>("drive"));
            }
            if(tag.ContainsKey("powerCell")) {
                powerCellItem = tag.Get<Item>("powerCell");
            }
            if(tag.ContainsKey("casing")) {
                casingItem = tag.Get<Item>("casing");
            }
            if(tag.ContainsKey("lens")) {
                lensItem = tag.Get<Item>("lens");
            }
            if(driveItem is null) {
                driveItem = new Ref<Item>(new Item());
                driveItem.Value.SetDefaults(0);
            }
            if(powerCellItem is null) {
                powerCellItem = new Item();
                powerCellItem.SetDefaults(0);
            }
            if(casingItem is null) {
                casingItem = new Item();
                casingItem.SetDefaults(0);
            }
            if(lensItem is null) {
                lensItem = new Item();
                lensItem.SetDefaults(0);
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(drive is null)return false;
            float c = maxCharge;
            RefWrapper<float> projCharge = getCharge;
            //Main.NewText($"{getCharge}/{c}");
            if(powerCellItem.type==ItemID.LihzahrdPowerCell) {
                RefWrapper<float> charge = powerCellItem.GetGlobalItem<PowerCellGlobalItem>().charge;
                if(charge.value<PowerCellGlobalItem.LihzahrdMaxCharge) {
                    return false;
                }
                projCharge = maxCharge;
                charge.value = 0;
            }
            base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            ActionContext context = ((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context;
            context.charge = projCharge;
            context.Caster = player;
            context.color = LensGlobalItem.getColor(lensItem);
            //((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context.charge = projCharge;
            //((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context.Caster = player;
            //((BasicGlyphProjectile)Main.projectile[lastProj].modProjectile).context.color = LensGlobalItem.getColor(lensItem);
            return false;
        }
    }
}
