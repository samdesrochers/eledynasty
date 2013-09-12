using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public static class TilesUtil
	{
		public static List<Vector2i> GetTileIndexesByType(int type)
		{
			List<Vector2i> indexes = new List<Vector2i>();
			
			Vector2i idleIndex = new Vector2i (0, 0);
			Vector2i activeIndex = new Vector2i (0, 0);
			Vector2i targetIndex = new Vector2i (0, 0);
			
			switch (type) {
				// GRASS
				case Constants.TILE_TYPE_GRASS_MIDDLE:
					idleIndex = new Vector2i (0, 3);
					activeIndex = new Vector2i(1, 3);
					targetIndex = new Vector2i(2, 3);
					break;
				
				// ROAD
				case Constants.TILE_TYPE_ROAD_VERTICAL:
					idleIndex = new Vector2i (1, 7);
					activeIndex = new Vector2i(3, 6);
					targetIndex = new Vector2i(1, 5);
					break;
				case Constants.TILE_TYPE_ROAD_HORIZONTAL:
					idleIndex = new Vector2i (0, 7);
					activeIndex = new Vector2i(2, 6);
					targetIndex = new Vector2i(0, 5);
					break;
				
				// ROAD - TURNS
				case Constants.TILE_TYPE_ROAD_VH_LEFT:
					idleIndex = new Vector2i (2, 7);
					activeIndex = new Vector2i(2, 5);
					targetIndex = new Vector2i(2, 4);
					break;
				case Constants.TILE_TYPE_ROAD_VH_RIGHT:
					idleIndex = new Vector2i (3, 7);
					activeIndex = new Vector2i(3, 5);
					targetIndex = new Vector2i(3, 4);
					break;
				case Constants.TILE_TYPE_ROAD_HV_LEFT:
					idleIndex = new Vector2i (1, 6);
					activeIndex = new Vector2i(1, 4);
					targetIndex = new Vector2i(5, 7);
					break;
				case Constants.TILE_TYPE_ROAD_HV_RIGHT:
					idleIndex = new Vector2i (0, 6);
					activeIndex = new Vector2i(0, 4);
					targetIndex = new Vector2i(4, 7);
					break;
				
				// TREES
				
				// MOUTAINS
			}
			
			indexes.Add(idleIndex);
			indexes.Add(activeIndex);
			indexes.Add(targetIndex);
			
			return indexes;
		}
		
		// Ajouter fonction qui check le type de terrain et donne le modifier de defense ici
	}
}

