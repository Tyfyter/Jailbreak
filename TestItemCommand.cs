using Jailbreak.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Jailbreak.Commands
{
	public class TestItemCommand : ModCommand
	{
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "glyph"; }
		}

		public override string Usage
		{
			get { return "/glyph <glyphs> or /glyphs += <actions>"; }
		}

		public override string Description
		{
			get { return ""; }
		}

		public override void Action(CommandCaller player, string input, string[] args){
            //try{
            string itemName;
            ActionItem item;
            string literal;
            ActionContext.Default.Caster = player.Player;
            int i = 0;
            if(args[0]=="+=") {
                i++;
            } else {
                TestItem.actions = new List<ActionItem>(){};
            }

            for(; i < args.Length; i++) {
                mod.Logger.Info($"parsing \'{args[i]}\'");
                itemName = args[i].Split('.')[0];
                item = Jailbreak.GetAction(itemName);
                if(item.hasLiteral) {
                    literal = args[i].Substring(itemName.Length+1);
                    item = Jailbreak.CreateNew(item);
                    item.ApplyLiteral(literal);
                    mod.Logger.Info($"set literal of {item} to {literal}");
                }
                TestItem.actions.Add(item);
            }
            /*}catch (Exception e){
                Main.NewText(e.Message);
            }*/
		}
	}
}