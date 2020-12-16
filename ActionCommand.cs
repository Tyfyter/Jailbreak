using Jailbreak.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Light.Commands
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
            try{
				//ActionBase.Deserialize(String.Join(" ", args)).Execute(0);
            }catch (Exception e){
                Main.NewText(e.Message);
            }
		}
	}
}