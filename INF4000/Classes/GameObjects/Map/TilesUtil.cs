using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public static class TilesUtil
	{
		public static Vector2i GetTileIndexesByType(int type)
		{
			Vector2i idleIndex = new Vector2i (0, 0);
			
			switch (type) {
				// GRASS
				case Constants.TILE_TYPE_GRASS_MIDDLE:
					idleIndex = new Vector2i (0, 3);
					break;
				case Constants.TILE_TYPE_GRASS_MIDDLE_2:
					idleIndex = new Vector2i (3, 2);
					break;	
				case Constants.TILE_TYPE_GRASS_TRANSIT:
					idleIndex = new Vector2i (7, 2);
					break;	
				
				// ROAD
				case Constants.TILE_TYPE_ROAD_VERTICAL:
					idleIndex = new Vector2i (1, 7);
					break;
				case Constants.TILE_TYPE_ROAD_HORIZONTAL:
					idleIndex = new Vector2i (0, 7);
					break;
				
				// ROAD - TURNS
				case Constants.TILE_TYPE_ROAD_VH_LEFT:
					idleIndex = new Vector2i (2, 7);
					break;
				case Constants.TILE_TYPE_ROAD_VH_RIGHT:
					idleIndex = new Vector2i (3, 7);
					break;
				case Constants.TILE_TYPE_ROAD_HV_LEFT:
					idleIndex = new Vector2i (1, 6);
					break;
				case Constants.TILE_TYPE_ROAD_HV_RIGHT:
					idleIndex = new Vector2i (0, 6);
					break;
				
				// TREES
				case Constants.TILE_TYPE_TREES_1:
					idleIndex = new Vector2i (0, 2);
					break;
				case Constants.TILE_TYPE_TREES_2:
					idleIndex = new Vector2i (4, 2);
					break;
				
				// MOUTAINS
				case Constants.TILE_TYPE_HILL:
					idleIndex = new Vector2i (0, 1);
					break;
				case Constants.TILE_TYPE_HILL_2:
					idleIndex = new Vector2i (5, 2);
					break;
				
				// WATER - DEFAULT
				case Constants.TILE_TYPE_WATER_MIDDLE:
					idleIndex = new Vector2i (5, 5);
					break;
				case Constants.TILE_TYPE_WATER_LEFT:
					idleIndex = new Vector2i (4, 5);
					break;
				case Constants.TILE_TYPE_WATER_RIGHT:
					idleIndex = new Vector2i (6, 5);
					break;
				case Constants.TILE_TYPE_WATER_UP:
					idleIndex = new Vector2i (5, 6);
					break;
				case Constants.TILE_TYPE_WATER_DOWN:
					idleIndex = new Vector2i (5, 4);
					break;
				case Constants.TILE_TYPE_WATER_UP_LEFT:
					idleIndex = new Vector2i (4, 6);
					break;
				case Constants.TILE_TYPE_WATER_UP_RIGHT:
					idleIndex = new Vector2i (6, 6);
					break;
				case Constants.TILE_TYPE_WATER_DW_RIGHT:
					idleIndex = new Vector2i (6, 4);
					break;
				case Constants.TILE_TYPE_WATER_DW_LEFT:
					idleIndex = new Vector2i (4, 4);
					break;
				case Constants.TILE_TYPE_WATER_CAN_VERT:
					idleIndex = new Vector2i (4, 3);
					break;
				case Constants.TILE_TYPE_WATER_CAN_HORIZ:
					idleIndex = new Vector2i (5, 3);
					break;
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_LEFT:
					idleIndex = new Vector2i (6, 3);
					break;
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_DW_LEFT:
					idleIndex = new Vector2i (7, 3);
					break;
				case Constants.TILE_TYPE_WATER_LEFT_EARTH_UP_RIGHT:
					idleIndex = new Vector2i (7, 4);
					break;
				case Constants.TILE_TYPE_WATER_UP_RIGHT_EARTH_DW_LEFT:
					idleIndex = new Vector2i (7, 5);
					break;
				case Constants.TILE_TYPE_WATER_UP_LEFT_EARTH_DW_RIGHT:
					idleIndex = new Vector2i (7, 6);
					break;
				case Constants.TILE_TYPE_WATER_UP_EARTH_DW_LEFT:
					idleIndex = new Vector2i (6, 7);
					break;
				case Constants.TILE_TYPE_WATER_MID_EARTH_UP_LF_R:
					idleIndex = new Vector2i (7, 7);
					break;

			}
			
			return idleIndex;	
		}
		
		public static Tuple<int, string> GetTileInfo(int type)
		{
			string name = "";
			int defense = 1;
			
			switch (type) {
				// GRASS
				case Constants.TILE_TYPE_GRASS_MIDDLE:
				case Constants.TILE_TYPE_GRASS_MIDDLE_2:
				case Constants.TILE_TYPE_GRASS_TRANSIT:
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

