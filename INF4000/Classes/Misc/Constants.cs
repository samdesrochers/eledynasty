using System;

namespace INF4000
{
	public static class Constants
	{
		// General - Tile Size
		public const int TILE_SIZE = 64;
		public const int TEXT_SIZE = 32;
		
		// General - In Game States
		public const int GAME_STATE_SELECTION_INACTIVE 		= 50;
		public const int GAME_STATE_UNIT_SELECTION_ACTIVE 	= 51;
		public const int GAME_STATE_BUIL_SELECTION_ACTIVE 	= 52;
		public const int GAME_STATE_ACTIONPANEL_ACTIVE 		= 53;
		public const int GAME_STATE_ATTACKPANEL_ACTIVE 		= 54;
		
		// General - Global States
		public const int GLOBAL_STATE_PLAYING_TURN 		= 10;
		public const int GLOBAL_STATE_SWITCHING_TURN 	= 11;
		public const int GLOBAL_STATE_GAMEOVER 			= 12;
		public const int GLOBAL_STATE_PAUSE 			= 13;
		public const int GLOBAL_STATE_LOADING			= 14;
		
		/*******************************
		 * 	TERRAIN CONSTANTS
		 * *****************************/
		
		// Terrain - Types
		public const int TILE_TYPE_GRASS_MIDDLE 	= 10;
		public const int TILE_TYPE_WATER_MIDDLE 	= 11;
		public const int TILE_TYPE_ROAD_HORIZONTAL 	= 12;
		public const int TILE_TYPE_ROAD_VERTICAL 	= 13;
		public const int TILE_TYPE_ROAD_VH_LEFT		= 40;
		public const int TILE_TYPE_ROAD_VH_RIGHT 	= 41;
		public const int TILE_TYPE_ROAD_HV_LEFT 	= 42;
		public const int TILE_TYPE_ROAD_HV_RIGHT 	= 43;
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
		public const int BUILD_FORT 	= 50;
		public const int BUILD_BARRACKS = 51;
		public const int BUILD_FARM	 	= 52;
		public const int BUILD_ARCHERY 	= 53;
		public const int BUILD_STABLES	= 54;
		public const int BUILD_WIZARD	= 55;
		
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
		public const string PATH_NONE   = "NONE";
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
		public const int ACTION_SLEEP = 4;
		public const int ACTION_NOMOVE_ATTACK = 5;
		public const int ACTION_PRODUCE = 6;
		public const int ACTION_PRODUCE_ATTACK = 7;
		
		/*******************************
		 * 	UI CONSTANTS
		 * *****************************/
		public const int UI_ELEMENT_ACTIONBOX_WIDTH 	= 200;
		public const int UI_ELEMENT_ACTIONBOX_HEIGHT 	= 125;
		public const int UI_ELEMENT_STATSBOX_WIDTH 	= 70;
		public const int UI_ELEMENT_STATSBOX_HEIGHT = 110;
		
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL					= 0;
		public const int UI_ELEMENT_CONFIG_CANCEL_PRODUCE				= 1;
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK			= 2;
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE			= 3;
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE_ATTACK	= 4;
		
		public const int UI_ELEMENT_CONFIG_STATS_UNIT					= 0;
		public const int UI_ELEMENT_CONFIG_STATS_TERRAIN				= 1;
		public const int UI_ELEMENT_CONFIG_STATS_BUILDING				= 2;
	
		public const int UI_ELEMENT_ACTION_TYPE_WAIT = 0;
		public const int UI_ELEMENT_ACTION_TYPE_CANCEL = 1;
		public const int UI_ELEMENT_ACTION_TYPE_ATTACK = 2;
		public const int UI_ELEMENT_ACTION_TYPE_PRODUCE = 3;
	}
}

