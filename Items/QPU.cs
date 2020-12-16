using Jailbreak.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Jailbreak.Items
{
	public class CPU : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CPU");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 15;
			item.height = 15;
			item.value = 100;
			item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddIngredient(ItemID.CopperBar, 10);
			recipe.AddIngredient(ItemID.Amethyst, 10);
			recipe.AddTile(ModContent.TileType<GenericGemspark>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class RAM1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RAM");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 15;
			item.height = 15;
			item.value = 100;
			item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 5);
			recipe.AddIngredient(ItemID.Amethyst, 5);
			recipe.AddTile(ModContent.TileType<GenericGemspark>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class APU : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Advanced CPU");
			Tooltip.SetDefault("It's advanced");
		}
		public override void SetDefaults()
		{
			item.width = 15;
			item.height = 15;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddIngredient(ItemID.Wire, 10);
			recipe.AddTile(TileID.Autohammer);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class QPU : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("QPU");
			Tooltip.SetDefault("Stands for \"Quantum Processing Unit\"... or \"Quadruple Parallel Universe\".");
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.width = 15;
			item.height = 15;
			item.value = 1000000;
			item.rare = ItemRarityID.Lime;
			item.scale = 1.25f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ItemID.FragmentNebula, 10);
			recipe.AddTile(TileID.LunarMonolith);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
