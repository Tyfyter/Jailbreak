using Jailbreak.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Jailbreak.Commands
{
	public class ActionCommand : ModCommand
	{
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "action"; }
		}

		public override string Usage
		{
			get { return "/action <action>"; }
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
                ActionContext.Caster = player.Player;
                for(int i = 0; i < args.Length; i++) {
                    mod.Logger.Info($"parsing \'{args[i]}\'");
                    itemName = args[i].Split('.')[0];
                    item = Jailbreak.GetAction(itemName);
                    if(item.hasLiteral) {
                        literal = args[i].Substring(itemName.Length+1);
                        item = (ActionItem)item.NewInstance(null);
                        item.ApplyLiteral(literal);
                        mod.Logger.Info($"set literal of {item} to {literal}");
                    }
                    ActionContext.lastReturn = item.Execute(0)??ActionContext.lastReturn;
                }
            /*}catch (Exception e){
                Main.NewText(e.Message);
            }*/
		}
	}
}