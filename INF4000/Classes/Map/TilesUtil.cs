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
				case Constants.TILE_TYPE_TREES_1:
					idleIndex = new Vector2i (0, 2);
					activeIndex = new Vector2i(1, 2);
					targetIndex = new Vector2i(2, 2);
					break;
				
				// MOUTAINS
				case Constants.TILE_TYPE_HILL:
					idleIndex = new Vector2i (0, 1);
					activeIndex = new Vector2i(1, 1);
					targetIndex = new Vector2i(2, 1);
					break;
				
				// WATER - DEFAULT
				case Constants.TILE_TYPE_WATER_MIDDLE:
					idleIndex = new Vector2i (5, 5);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_LEFT:
					idleIndex = new Vector2i (4, 5);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_RIGHT:
					idleIndex = new Vector2i (6, 5);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_UP:
					idleIndex = new Vector2i (5, 6);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_DOWN:
					idleIndex = new Vector2i (5, 4);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_UP_LEFT:
					idleIndex = new Vector2i (4, 6);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_UP_RIGHT:
					idleIndex = new Vector2i (6, 6);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_DW_RIGHT:
					idleIndex = new Vector2i (6, 4);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_DW_LEFT:
					idleIndex = new Vector2i (4, 4);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_CAN_VERT:
					idleIndex = new Vector2i (4, 3);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_CAN_HORIZ:
					idleIndex = new Vector2i (5, 3);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_LEFT:
					idleIndex = new Vector2i (6, 3);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_DW_LEFT:
					idleIndex = new Vector2i (7, 3);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_LEFT_EARTH_UP_RIGHT:
					idleIndex = new Vector2i (7, 4);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_UP_RIGHT_EARTH_DW_LEFT:
					idleIndex = new Vector2i (7, 5);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_UP_LEFT_EARTH_DW_RIGHT:
					idleIndex = new Vector2i (7, 6);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_UP_EARTH_DW_LEFT:
					idleIndex = new Vector2i (6, 7);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_LF_R:
					idleIndex = new Vector2i (7, 7);
					activeIndex = idleIndex;
					targetIndex = idleIndex;
					break;
				
				
				
			}
			
			indexes.Add(idleIndex);
			indexes.Add(activeIndex);
			indexes.Add(targetIndex);
			
			return indexes;
		}
		
		public static Tuple<int, string> GetTileInfo(int type)
		{
			string name = "";
			int defense = 1;
			
			switch (type) {
				// GRASS
				case Constants.TILE_TYPE_GRASS_MIDDLE:
					name = "Grass";
					defense = 1;
					break;
				
				// ROAD
				case Constants.TILE_TYPE_ROAD_VERTICAL:
				case Constants.TILE_TYPE_ROAD_HORIZONTAL:
				case Constants.TILE_TYPE_ROAD_VH_LEFT:
				case Constants.TILE_TYPE_ROAD_VH_RIGHT:
				case Constants.TILE_TYPE_ROAD_HV_LEFT:
				case Constants.TILE_TYPE_ROAD_HV_RIGHT:
					name = "Road";
					defense = 1;
					break;
				
				// TREES
				case Constants.TILE_TYPE_TREES_1:
					name = "Forest";
					defense = 2;
					break;
				
				// MOUTAINS
				case Constants.TILE_TYPE_HILL:
					name = "Hills";
					defense = 4;
					break;
				
				// WATER
				case Constants.TILE_TYPE_WATER_UP:
				case Constants.TILE_TYPE_WATER_MIDDLE:
				case Constants.TILE_TYPE_WATER_LEFT:
				case Constants.TILE_TYPE_WATER_RIGHT:
				case Constants.TILE_TYPE_WATER_DOWN:
				case Constants.TILE_TYPE_WATER_UP_LEFT:
			 	case Constants.TILE_TYPE_WATER_UP_RIGHT:
				case Constants.TILE_TYPE_WATER_DW_LEFT:
				case Constants.TILE_TYPE_WATER_DW_RIGHT:
				case Constants.TILE_TYPE_WATER_CAN_HORIZ:
				case Constants.TILE_TYPE_WATER_CAN_VERT:		
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_LEFT:
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_DW_LEFT:
				case Constants.TILE_TYPE_WATER_LEFT_EARTH_UP_RIGHT:
				case Constants.TILE_TYPE_WATER_UP_RIGHT_EARTH_DW_LEFT:
				case Constants.TILE_TYPE_WATER_UP_LEFT_EARTH_DW_RIGHT:
				case Constants.TILE_TYPE_WATER_UP_EARTH_DW_LEFT:
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_LF_R:
					name = "Water";
					defense = 0;
					break;
			}
			
			return new Tuple<int, string>(defense, name);
		}
		
		// Ajouter fonction qui check le type de terrain et donne le modifier de defense ici
	}
}

