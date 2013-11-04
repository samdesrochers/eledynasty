using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public static class BuildingUtil
	{
		public static Stack<Building> LastCapturedBuildings;
		
		public static List<Vector2i> GetTileIndexesByType(int type, int owner)
		{
			List<Vector2i> indexes = new List<Vector2i>();
			Vector2i indexP1 = new Vector2i (0, 0);
			Vector2i indexP2 = new Vector2i (0, 0);
			Vector2i indexN = new Vector2i (0, 0);
			
			switch (type) {
				// FORT
				case Constants.BUILD_FORT:
					indexP1 = new Vector2i (2, 2);
					indexP2 = new Vector2i (3, 2);
					indexN = new Vector2i (1, 0);
					break;
				
				// FARM
				case Constants.BUILD_FARM:
					indexP1 = new Vector2i (2, 3);
					indexP2 = new Vector2i (3, 3);
					indexN = new Vector2i (1, 0);
					break;
				
				// TEMPLE
				case Constants.BUILD_TEMPLE:
					indexP1 = new Vector2i (0, 3);
					indexP2 = new Vector2i (1, 3);
					indexN = new Vector2i (1, 2);
					break;
				
				// FORGE
				case Constants.BUILD_FORGE:
					indexP1 = new Vector2i (1, 1);
					indexP2 = new Vector2i (0, 1);
					indexN = new Vector2i (0, 2);
					break;
				
			}
			indexes.Add(indexP1);
			indexes.Add(indexP2);
			indexes.Add(indexN);
			return indexes;	
		}
		
		public static List<int> GetStatsByType(int type)
		{
			List<int> stats = new List<int>();
			
			int defense = 1;
			int goldPerTurn = 1;
			int goldToProduce = 1;
			int productionType = 1;
			
			switch (type) {
				// FORT
				case Constants.BUILD_FORT:
					defense = Constants.BUILD_FORT_GOLD_DEF;
					goldPerTurn = Constants.BUILD_FORT_GOLD_YEILD;
					goldToProduce = 0;
					productionType = Constants.UNIT_TYPE_NONE;
					break;
				
				// FARM
				case Constants.BUILD_FARM:
					defense = Constants.BUILD_FARM_GOLD_DEF;
					goldPerTurn = Constants.BUILD_FARM_GOLD_YEILD;
					goldToProduce = Constants.BUILD_FARM_GOLD_PRODUCE;
					productionType = Constants.UNIT_TYPE_FARMER;
					break;
				
				// TEMPLE
				case Constants.BUILD_TEMPLE:
					defense = Constants.BUILD_TEMPLE_GOLD_DEF;
					goldPerTurn = Constants.BUILD_TEMPLE_GOLD_YEILD;
					goldToProduce = Constants.BUILD_TEMPLE_GOLD_PRODUCE;
					productionType = Constants.UNIT_TYPE_MONK;
					break;
				
				// FORGE
				case Constants.BUILD_FORGE:
					defense = Constants.BUILD_FORGE_GOLD_DEF;
					goldPerTurn = Constants.BUILD_FORGE_GOLD_YEILD;
					goldToProduce = Constants.BUILD_FORGE_GOLD_PRODUCE;
					productionType = Constants.UNIT_TYPE_SAMURAI;
					break;
			}
			
			stats.Add(defense);
			stats.Add(goldPerTurn);
			stats.Add(goldToProduce);
			stats.Add(productionType);
			
			return stats;
		}
		
		public static string GetNameByType(int type)
		{
			string name = "";			
			switch (type) {
				case Constants.BUILD_FORT:
					name = "Fortress";
					break;
				case Constants.BUILD_FARM:
					name = "Farm";
					break;
				case Constants.BUILD_TEMPLE:
					name = "Temple";
					break;
				case Constants.BUILD_FORGE:
					name = "Forge";
					break;
			}			
			return name;
		}
		
		public static int GetHighLightedBuildingGoldToProduce()
		{
			Tile t = Utilities.GetTile(GameScene.Instance.Cursor.WorldPosition);
			if(t.CurrentBuilding != null)
				return t.CurrentBuilding.GoldToProduce;
			return -1;
		}
		
		static float time = 0;
		static Building currentBuilding;
		static TextImage pointsImage;
		public static void Update(float dt)
		{
			// Init
			if(time == 0) {
				currentBuilding = LastCapturedBuildings.Pop();
				
				string toDisplay = "CAP : " + (20 - currentBuilding.PointsToCapture).ToString();
				pointsImage = new TextImage(toDisplay, currentBuilding.Position);
				
				if(GameScene.Instance.ActivePlayer.IsHuman && currentBuilding.OwnerName == Constants.CHAR_KENJI)
					pointsImage.Color = new Sce.PlayStation.Core.Vector4(0.1f, 0.1f, 1, 1);
				else if(GameScene.Instance.ActivePlayer.IsHuman && currentBuilding.OwnerName != Constants.CHAR_KENJI)
					pointsImage.Color = new Sce.PlayStation.Core.Vector4(1f, 0.1f, 0.1f, 1);
				
				GameScene.Instance.AddChild(pointsImage);
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}
			
			if(time <= 0.4f)
			{
				pointsImage.Position = new Sce.PlayStation.Core.Vector2(pointsImage.Position.X, pointsImage.Position.Y + 1);
			}
			
			if(time > 0.4f)
			{
				GameScene.Instance.RemoveChild(pointsImage, false);
				time = 0;
			}
			
			time += dt;
			
			if(BuildingUtil.LastCapturedBuildings.Count == 0) {
				GameScene.Instance.CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
			}
		}
	}
}

