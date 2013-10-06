using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public static class BuildingUtil
	{
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
					indexN = new Vector2i (1, 0);
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
				
				// TEMPLS
				case Constants.BUILD_TEMPLE:
					defense = Constants.BUILD_TEMPLE_GOLD_DEF;
					goldPerTurn = Constants.BUILD_TEMPLE_GOLD_YEILD;
					goldToProduce = Constants.BUILD_TEMPLE_GOLD_PRODUCE;
					productionType = Constants.UNIT_TYPE_MONK;
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
			}			
			return name;
		}
		
		public static int GetHighLightedBuildingGoldToProduce()
		{
			Tile t = GameScene.Instance.CurrentMap.GetTile(GameScene.Instance.Cursor.WorldPosition);
			if(t.CurrentBuilding != null)
				return t.CurrentBuilding.GoldToProduce;
			return -1;
		}
	}
}

