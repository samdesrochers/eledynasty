using System;

namespace INF4000
{
	public static class Constants
	{
		// General - Character Names
		public const string CHAR_KENJI = "Kenji";
		public const string CHAR_GOHZU = "Gohzu";
		
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
		public const int GLOBAL_STATE_BATTLE_ANIMATION	= 15;
		public const int GLOBAL_STATE_DIALOG			= 16;
		public const int GLOBAL_STATE_STARTING_GAME		= 17;

		
		/*******************************
		 * 	TERRAIN CONSTANTS
		 * *****************************/
		
		// Terrain - Types
		public const int TILE_TYPE_GRASS_MIDDLE 	= 10;
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
		public const int TILE_TYPE_BUILD_FORGE		= 54;
		public const int TILE_TYPE_BUILD_WIZARD		= 55;
		public const int TILE_TYPE_HILL				= 80;
		public const int TILE_TYPE_TREES_1			= 81;
		
		public const int TILE_TYPE_GRASS_MIDDLE_2 	= 30;
		public const int TILE_TYPE_HILL_2 			= 82;
		public const int TILE_TYPE_TREES_2 			= 83;
		public const int TILE_TYPE_GRASS_TRANSIT 	= 90;

		// Water 
		public const int TILE_TYPE_WATER_MIDDLE 	= 60;
		public const int TILE_TYPE_WATER_LEFT 		= 61;
		public const int TILE_TYPE_WATER_RIGHT	 	= 62;
		public const int TILE_TYPE_WATER_UP			= 63;
		public const int TILE_TYPE_WATER_DOWN	 	= 64;
		public const int TILE_TYPE_WATER_UP_LEFT 	= 65;
		public const int TILE_TYPE_WATER_UP_RIGHT 	= 66;
		public const int TILE_TYPE_WATER_DW_LEFT 	= 67;
		public const int TILE_TYPE_WATER_DW_RIGHT 	= 68;
		public const int TILE_TYPE_WATER_CAN_HORIZ 	= 69;
		public const int TILE_TYPE_WATER_CAN_VERT 	= 70;
		
		public const int TILE_TYPE_WATER_MID_EARTH_UP_LEFT 		= 71;
		public const int TILE_TYPE_WATER_MID_EARTH_UP_DW_LEFT 	= 72;
		public const int TILE_TYPE_WATER_LEFT_EARTH_UP_RIGHT	= 73;
		public const int TILE_TYPE_WATER_UP_RIGHT_EARTH_DW_LEFT = 74;
		public const int TILE_TYPE_WATER_UP_LEFT_EARTH_DW_RIGHT = 75;
		public const int TILE_TYPE_WATER_UP_EARTH_DW_LEFT 		= 76;
		public const int TILE_TYPE_WATER_MID_EARTH_UP_LF_R 		= 77;
		
		/*******************************
		 * 	BUILDING CONSTANTS
		 * *****************************/
		
		// Buildings - Types
		public const int BUILD_FORT 	= 50;
		public const int BUILD_TEMPLE 	= 51;
		public const int BUILD_FARM	 	= 52;
		public const int BUILD_ARCHERY 	= 53;
		public const int BUILD_FORGE	= 54;
		public const int BUILD_WIZARD	= 55;
		
		// Buildings - To Produce
		public const int BUILD_TEMPLE_GOLD_PRODUCE 	= 1;
		public const int BUILD_FORGE_GOLD_PRODUCE 	= 1;
		public const int BUILD_FARM_GOLD_PRODUCE 	= 1;
		
		// Buildings - Yeild
		public const int BUILD_TEMPLE_GOLD_YEILD 	= 2;
		public const int BUILD_FORGE_GOLD_YEILD 	= 3;
		public const int BUILD_FARM_GOLD_YEILD 		= 1;
		public const int BUILD_FORT_GOLD_YEILD 		= 5;
		
		// Buildings - Defense
		public const int BUILD_TEMPLE_GOLD_DEF 		= 3;
		public const int BUILD_FORGE_GOLD_DEF 		= 4;
		public const int BUILD_FARM_GOLD_DEF 		= 2;
		public const int BUILD_FORT_GOLD_DEF 		= 9;
		
		/*******************************
		 * 	UNIT CONSTANTS
		 * *****************************/
		
		// Unit - Units Type
		public const int UNIT_TYPE_NONE 	= 20;
		public const int UNIT_TYPE_FARMER 	= 21;
		public const int UNIT_TYPE_SAMURAI 	= 22;
		public const int UNIT_TYPE_MONK 	= 23;
		public const int UNIT_TYPE_WIZARD 	= 24;		
		
		// Unit - Life Points 
		public const int UNIT_HP_FARMER = 10;
		public const int UNIT_HP_SAMURAI = 10;
		public const int UNIT_HP_MONK = 10;
		public const int UNIT_HP_KNIGHT = 10;
		public const int UNIT_HP_WIZARD = 10;
		
		// Unit - Attack Damage 
		public const int UNIT_AD_FARMER 	= 4;
		public const int UNIT_AD_SAMURAI 	= 11;
		public const int UNIT_AD_MONK 		= 8;
		public const int UNIT_AD_KNIGHT 	= 10;
		public const int UNIT_AD_WIZARD 	= 7;
		
		// Unit - Defense 
		public const int UNIT_DEF_FARMER 	= 1;
		public const int UNIT_DEF_SAMURAI	= 6;
		public const int UNIT_DEF_MONK 		= 1;
		public const int UNIT_DEF_KNIGHT 	= 8;
		public const int UNIT_DEF_WIZARD 	= 2;
		
		// Unit - Base number of Move Points  
		public const int UNIT_MOVE_FARMER 	= 2;
		public const int UNIT_MOVE_SAMURAI	= 2;
		public const int UNIT_MOVE_MONK 	= 3;
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
		public const int UI_ELEMENT_STATSBOX_WIDTH 		= 135;
		public const int UI_ELEMENT_STATSBOX_HEIGHT 	= 110;
		
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL					= 0;
		public const int UI_ELEMENT_CONFIG_CANCEL_PRODUCE				= 1;
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK			= 2;
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE			= 3;
		public const int UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE_ATTACK	= 4;
		
		public const int UI_ELEMENT_CONFIG_STATS_UNIT					= 0;
		public const int UI_ELEMENT_CONFIG_STATS_TERRAIN				= 1;
		public const int UI_ELEMENT_CONFIG_STATS_BUILDING				= 2;
	
		public const int UI_ELEMENT_ACTION_TYPE_WAIT 	= 0;
		public const int UI_ELEMENT_ACTION_TYPE_CANCEL 	= 1;
		public const int UI_ELEMENT_ACTION_TYPE_ATTACK	= 2;
		public const int UI_ELEMENT_ACTION_TYPE_PRODUCE = 3;
		
		/*******************************
		 * 	BATTLE MANAGER CONSTANTS
		 * *****************************/
		public const int BATTLE_END_ATTACKER_TOTALWIN 	= 0;
		public const int BATTLE_END_DEFENDER_TOTALWIN 	= 1;
		public const int BATTLE_END_TIE 				= 2;
		
		/*******************************
		 * 	AI CONSTANTS
		 * *****************************/
		public const int AI_STATE_WAITING			= 0;
		public const int AI_STATE_UNIT_SELECTED 	= 1;
		public const int AI_STATE_ACTION_SELECTED 	= 2;
		public const int AI_STATE_EXECUTING_ACTION	= 3;
		
		public const int AI_ACTION_NONE				= 0;
		public const int AI_ACTION_MOVE 			= 5;
		public const int AI_ACTION_ATTACK 			= 6;
		public const int AI_ACTION_FORTIFY 			= 7;
		public const int AI_ACTION_PRODUCE 			= 8;
		
		/*******************************
		 * 	SOUNDS CONSTANTS
		 * *****************************/
		public const int SOUND_CURSOR_MOVE		= 1;
		public const int SOUND_CURSOR_SELECT	= 2;
		public const int SOUND_BATTLE_CLASH		= 3;
		public const int SOUND_CURSOR_CANCEL	= 4;
		public const int SOUND_PANEL_MOVE		= 5;
		public const int SOUND_TURN_START		= 6;
		public const int SOUND_UNIT_MARCH		= 7;
		public const int SOUND_COMBAT			= 8;
		
		/*******************************
		 * 	DIALOG CONSTANTS
		 * *****************************/
		public const string DIALOG_EOF	= "end_file";
		
		/*******************************
		 * 	CURSOR CONSTANTS
		 * *****************************/
		public const float CURSOR_TICK_TIME	= 0.15f;
		
		/*******************************
		 * 	OVERWORLD CONSTANTS
		 * *****************************/
		public const int OV_STATE_IDLE	= 0;
		public const int OV_STATE_STARTING_GAME	= 1;
		public const int OV_STATE_EXITING_GAME	= 2;

	}
}
