using System;

namespace INF4000
{
	public static class Constants
	{
		// General - Tile Size
		public const int TILE_SIZE = 64;
		public const int TEXT_SIZE = 32;
		
		// General - Game States
		public const int STATE_SELECT_IDLE 		= 0;
		public const int STATE_SELECT_ACTIVE 	= 1;
		
		/*******************************
		 * 	TERRAIN CONSTANTS
		 * *****************************/
		
		// Terrain - Types
		public const int TILE_TYPE_GRASS_MIDDLE 	= 10;
		public const int TILE_TYPE_WATER_MIDDLE 	= 11;
		public const int TILE_TYPE_ROAD_HORIZONTAL 	= 12;
		public const int TILE_TYPE_ROAD_VERTICAL 	= 13;
		public const int TILE_TYPE_ROAD_TURN_0 		= 40;
		public const int TILE_TYPE_ROAD_TURN_90 	= 41;
		public const int TILE_TYPE_ROAD_TURN_180 	= 42;
		public const int TILE_TYPE_ROAD_TURN_270 	= 43;
		public const int TILE_TYPE_BUILD_FORT 		= 50;
		public const int TILE_TYPE_BUILD_BARRACKS 	= 51;
		public const int TILE_TYPE_BUILD_FARM	 	= 52;
		public const int TILE_TYPE_BUILD_ARCHERY 	= 53;
		public const int TILE_TYPE_BUILD_STABLES	= 54;
		public const int TILE_TYPE_BUILD_WIZARD		= 55;
		
		/*******************************
		 * 	BUILDING CONSTANTS
		 * *****************************/
		
		// Buildings - Types
		public const int BUILD_NONE		= 0;
		public const int BUILD_FORT 	= 1;
		public const int BUILD_BARRACKS = 2;
		public const int BUILD_FARM	 	= 3;
		public const int BUILD_ARCHERY 	= 4;
		public const int BUILD_STABLES	= 5;
		public const int BUILD_WIZARD	= 6;
		
		/*******************************
		 * 	UNIT CONSTANTS
		 * *****************************/
		
		// Unit - Units Type
		public const int UNIT_TYPE_NONE 	= 10;
		public const int UNIT_TYPE_FARMER 	= 11;
		public const int UNIT_TYPE_SWORD 	= 12;
		public const int UNIT_TYPE_ARCHER 	= 13;
		public const int UNIT_TYPE_KNIGHT 	= 14;
		public const int UNIT_TYPE_WIZARD 	= 15;		
		
		// Unit - Life Points 
		public const int UNIT_HP_FARMER = 5;
		public const int UNIT_HP_SWORD 	= 16;
		public const int UNIT_HP_ARCHER = 10;
		public const int UNIT_HP_KNIGHT = 20;
		public const int UNIT_HP_WIZARD = 7;
		
		// Unit - Attack Damage 
		public const int UNIT_AD_FARMER = 3;
		public const int UNIT_AD_SWORD 	= 8;
		public const int UNIT_AD_ARCHER = 5;
		public const int UNIT_AD_KNIGHT = 10;
		public const int UNIT_AD_WIZARD = 7;
		
		// Unit - Defense 
		public const int UNIT_DEF_FARMER 	= 1;
		public const int UNIT_DEF_SWORD 	= 6;
		public const int UNIT_DEF_ARCHER 	= 4;
		public const int UNIT_DEF_KNIGHT 	= 8;
		public const int UNIT_DEF_WIZARD 	= 2;
		
		// Unit - Base number of Move Points  
		public const int UNIT_MOVE_FARMER 	= 5;
		public const int UNIT_MOVE_SWORD 	= 6;
		public const int UNIT_MOVE_ARCHER 	= 5;
		public const int UNIT_MOVE_KNIGHT 	= 9;
		public const int UNIT_MOVE_WIZARD 	= 5;

		/*******************************
		 * 	PATH CONSTANTS
		 * *****************************/
		public const string PATH_LEFT 	= "LEFT";
		public const string PATH_UP 	= "UP";
		public const string PATH_RIGHT 	= "RIGHT";
		public const string PATH_DOWN 	= "DOWN";
		public const int PATH_TICKS 	= 8;
		public const int PATH_STEP 		= 8;
		
		// Actions - Different actions possible for a turn
		public const int ACTION_CANCEL 	= 0;
		public const int ACTION_MOVE 	= 1;
		public const int ACTION_ATTACK 	= 2;
		public const int ACTION_CAPTURE = 3;

		
		
	}
}

