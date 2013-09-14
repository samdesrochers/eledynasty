using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public static class BuildingUtil
	{
		public static Vector2i GetTileIndexesByType(int type, int owner)
		{
			Vector2i index = new Vector2i (0, 0);			
			switch (type) {
				// FORT
				case Constants.BUILD_FORT:
					if(owner == 1)
						index = new Vector2i (0, 0);
					else
						index = new Vector2i (1, 0);
					break;
				
				// FARM
				case Constants.BUILD_FARM:
					if(owner == 1)
						index = new Vector2i (0, 1);
					else
						index = new Vector2i (1, 1);
					break;
				
			}
			return index;	
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
					defense = 8;
					goldPerTurn = 3;
					goldToProduce = 0;
					productionType = Constants.UNIT_TYPE_NONE;
					break;
				
				// FARM
				case Constants.BUILD_FARM:
					defense = 1;
					goldPerTurn = 2;
					goldToProduce = 4;
					productionType = Constants.UNIT_TYPE_FARMER;
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
	}
}

