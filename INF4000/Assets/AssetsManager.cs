using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class AssetsManager
	{
		private static AssetsManager _Instance;

		public static AssetsManager Instance {
			get {
				if (_Instance == null) {
					_Instance = new AssetsManager ();
				}
				return _Instance;
			}
		}
		
		public bool Loaded;
		
		public UIFont PixelFont;
		public UIFont PixelFont_18;
		public UIFont PixelFont_48;
		public UIFont XenoFont;
		public UIFont XenoFont_24;
		public UIFont XenoFont_36;

		
		public Texture2D CursorTexture;
		public TextureInfo CursorTextureInfo;
		
		public Texture2D UnitsTexture;
		public TextureInfo UnitsTextureInfo;
		
		public Texture2D TerrainTexture;
		public TextureInfo TerrainTextureInfo;
		
		public Texture2D BuildingsTexture;
		public TextureInfo BuildingsTextureInfo;
		
		public Texture2D BattleTexture;
		public TextureInfo BattleTextureInfo;
		
		public Texture2D OverworldMapTexture;
		public TextureInfo OverworldMapTextureInfo;
		
		public Texture2D AvatarTexture;
		public TextureInfo AvatarTextureInfo;
		
		public ImageAsset Image_TurnSwitch_BG;
		public ImageAsset Image_Black_BG;
		public ImageAsset Image_White_BG;
		public ImageAsset Image_Player_1_Icon;
		public ImageAsset Image_Action_Panel_BG;
		public ImageAsset Image_Panel_Attack_Icon;
		public ImageAsset Image_Panel_Wait_Icon;
		public ImageAsset Image_Panel_Cancel_Icon;
		public ImageAsset Image_Panel_OddsBubble;
		public ImageAsset Image_Button_EndTurn;
		
		public ImageAsset Image_Stats_Panel_BG;
		public ImageAsset Image_Panel_Def_Icon;
		public ImageAsset Image_Panel_Dmg_Icon;
		public ImageAsset Image_Panel_HP_Icon;
		public ImageAsset Image_Panel_Gld_Icon;
		public ImageAsset Image_Panel_Flag_Icon;
		
		public ImageAsset Image_Kenji_UI_Turn;
		public ImageAsset Image_Gohzu_UI_Turn;

		// Icons
		public ImageAsset Image_Icon_Grass;
		public ImageAsset Image_Icon_Road;
		public ImageAsset Image_Icon_Hills;
		public ImageAsset Image_Icon_Trees;
		public ImageAsset Image_Icon_Water;
		
		public ImageAsset Image_Icon_Farm_Kenji;
		public ImageAsset Image_Icon_Farm_En;
		public ImageAsset Image_Icon_Temple_Kenji;
		public ImageAsset Image_Icon_Temple_En;
		public ImageAsset Image_Icon_Forge_Kenji;
		public ImageAsset Image_Icon_Forge_En;
		public ImageAsset Image_Icon_Fort_Kenji;
		public ImageAsset Image_Icon_Fort_En;
		public ImageAsset Image_Icon_Forge_Neutral;
		public ImageAsset Image_Icon_Temple_Neutral;
		
		public ImageAsset Image_Icon_Farmer_Kenji;
		public ImageAsset Image_Icon_Farmer_En;
		public ImageAsset Image_Icon_Monk_Kenji;
		public ImageAsset Image_Icon_Monk_En;
		public ImageAsset Image_Icon_Samurai_Kenji;
		public ImageAsset Image_Icon_Samurai_En;

		
		// Dialog
		public ImageAsset Image_Dialog_Overlay_Red;
		public ImageAsset Image_Dialog_Overlay_Blue;
		public ImageAsset Image_Dialog_Kenji_1;
		public ImageAsset Image_Dialog_Gohzu_1;
		
		public AssetsManager ()
		{
			Loaded = false;
		}
		
		public void LoadAssets()
		{
			if(!Loaded){
				try
				{
					// Fonts
					PixelFont = new UIFont("/Application/Assets/fonts/half_bold_pixel-7.ttf", 24, FontStyle.Bold);
					PixelFont_18 = new UIFont("/Application/Assets/fonts/half_bold_pixel-7.ttf", 18, FontStyle.Bold);
					PixelFont_48 = new UIFont("/Application/Assets/fonts/half_bold_pixel-7.ttf", 48, FontStyle.Bold);
					XenoFont = new UIFont("/Application/Assets/fonts/Xenogears_font.ttf", 72, FontStyle.Regular);
					XenoFont_24 = new UIFont("/Application/Assets/fonts/Xenogears_font.ttf", 24, FontStyle.Regular);
					XenoFont_36 = new UIFont("/Application/Assets/fonts/Xenogears_font.ttf", 36, FontStyle.Regular);

					
					// Cursor Textures
					CursorTexture = new Texture2D ("/Application/Assets/Items/gameItems.png", false);
					CursorTextureInfo = new TextureInfo (CursorTexture, new Vector2i (2,2));
					
					// Units Textures
					UnitsTexture = new Texture2D ("/Application/Assets/Units/units.png", false);
					UnitsTextureInfo = new TextureInfo (UnitsTexture, new Vector2i (4, 4));
					
					// Terrain Textures
					TerrainTexture = new Texture2D ("/Application/Assets/Tiles/TilesMap_Final.png", false);
					TerrainTextureInfo = new TextureInfo (TerrainTexture, new Vector2i (8, 8));
					
					// Buildings Textures
					BuildingsTexture = new Texture2D ("/Application/Assets/Buildings/build_1.png", false);
					BuildingsTextureInfo = new TextureInfo (BuildingsTexture, new Vector2i (4, 4));
					
					// Battle Terrain's Textures
					BattleTexture = new Texture2D ("/Application/Assets/BattleTerrain/battleTerrain.png", false);
					BattleTextureInfo = new TextureInfo (BattleTexture, new Vector2i (2, 3));
					
					// Overworld Textures
					OverworldMapTexture = new Texture2D ("/Application/Assets/Map/overworld.png", false);
					OverworldMapTextureInfo = new TextureInfo (OverworldMapTexture, new Vector2i (1, 1));
					
					// Avatar Textures
					AvatarTexture = new Texture2D ("/Application/Assets/Players/avatar.png", false);
					AvatarTextureInfo = new TextureInfo (AvatarTexture, new Vector2i (3, 4));

					
					// UI Elements
					Image_TurnSwitch_BG = new ImageAsset("/Application/Assets/UI/turnswitch_bg.png",false);
					Image_Black_BG = new ImageAsset("/Application/Assets/UI/blackbg.png",false);
					Image_White_BG = new ImageAsset("/Application/Assets/UI/whitebg.png",false);
					Image_Player_1_Icon = new ImageAsset("/Application/Assets/Players/araki.png",false);
					Image_Action_Panel_BG = new ImageAsset("/Application/Assets/UI/panel_bg.png",false);
					Image_Panel_Wait_Icon = new ImageAsset("/Application/Assets/UI/wait_icon.png",false);
					Image_Panel_Attack_Icon = new ImageAsset("/Application/Assets/UI/attack_icon.png",false);
					Image_Panel_Cancel_Icon = new ImageAsset("/Application/Assets/UI/back_icon.png",false);
					Image_Panel_OddsBubble = new ImageAsset("/Application/Assets/UI/odds_bubble.png",false);
					Image_Button_EndTurn = new ImageAsset("/Application/Assets/UI/endturn_but.png",false);
					
					Image_Stats_Panel_BG = new ImageAsset("/Application/Assets/UI/stats_panel.png",false);
					Image_Panel_Def_Icon = new ImageAsset("/Application/Assets/UI/def_ico.png",false);
					Image_Panel_Dmg_Icon = new ImageAsset("/Application/Assets/UI/dmg_ico.png",false);
					Image_Panel_HP_Icon = new ImageAsset("/Application/Assets/UI/hp_ico.png",false);
					Image_Panel_Gld_Icon = new ImageAsset("/Application/Assets/UI/gold_ico.png",false);
					Image_Panel_Flag_Icon = new ImageAsset("/Application/Assets/UI/flag_ico.png",false);
					
					// Stat Icons
					Image_Icon_Grass = new ImageAsset("/Application/Assets/UI/Icons/grass_ico.png",false);
					Image_Icon_Road = new ImageAsset("/Application/Assets/UI/Icons/road_ico.png",false);
					Image_Icon_Hills = new ImageAsset("/Application/Assets/UI/Icons/hills_ico.png",false);
					Image_Icon_Trees = new ImageAsset("/Application/Assets/UI/Icons/trees_ico.png",false);
					Image_Icon_Water = new ImageAsset("/Application/Assets/UI/Icons/water_ico.png",false);
					
					Image_Icon_Farm_Kenji = new ImageAsset("/Application/Assets/UI/Icons/farm_ico_kenji.png",false);
					Image_Icon_Farm_En = new ImageAsset("/Application/Assets/UI/Icons/farm_ico_en.png",false);
					Image_Icon_Temple_Kenji = new ImageAsset("/Application/Assets/UI/Icons/temple_ico_kenji.png",false);
					Image_Icon_Temple_En = new ImageAsset("/Application/Assets/UI/Icons/temple_ico_en.png",false);
					Image_Icon_Forge_Kenji = new ImageAsset("/Application/Assets/UI/Icons/forge_ico_kenji.png",false);
					Image_Icon_Forge_En = new ImageAsset("/Application/Assets/UI/Icons/forge_ico_en.png",false);
					Image_Icon_Fort_Kenji = new ImageAsset("/Application/Assets/UI/Icons/fort_ico_kenji.png",false);
					Image_Icon_Fort_En = new ImageAsset("/Application/Assets/UI/Icons/fort_ico_en.png",false);
					Image_Icon_Forge_Neutral = new ImageAsset("/Application/Assets/UI/Icons/forge_ico_neutral.png",false);
					Image_Icon_Temple_Neutral = new ImageAsset("/Application/Assets/UI/Icons/temple_ico_neutral.png",false);
					
					Image_Icon_Farmer_Kenji = new ImageAsset("/Application/Assets/UI/Icons/farmer_ico_kenji.png",false);
					Image_Icon_Farmer_En = new ImageAsset("/Application/Assets/UI/Icons/farmer_ico_en.png",false);
					Image_Icon_Monk_Kenji = new ImageAsset("/Application/Assets/UI/Icons/monk_ico_kenji.png",false);
					Image_Icon_Monk_En = new ImageAsset("/Application/Assets/UI/Icons/monk_ico_en.png",false);
					Image_Icon_Samurai_Kenji = new ImageAsset("/Application/Assets/UI/Icons/samurai_ico_kenji.png",false);
					Image_Icon_Samurai_En = new ImageAsset("/Application/Assets/UI/Icons/samurai_ico_en.png",false);
								
					// Player 
					Image_Kenji_UI_Turn = new ImageAsset("/Application/Assets/UI/kenji_turn.png",false);
					Image_Gohzu_UI_Turn = new ImageAsset("/Application/Assets/UI/gohzu_turn.png",false);
					
					// Dialog Elements
					Image_Dialog_Overlay_Red = new ImageAsset("/Application/Assets/Dialog/char_overlay_red.png",false);
					Image_Dialog_Overlay_Blue = new ImageAsset("/Application/Assets/Dialog/char_overlay_blue.png",false);
					Image_Dialog_Kenji_1 = new ImageAsset("/Application/Assets/Dialog/char_kenji_1.png",false);
					Image_Dialog_Gohzu_1 = new ImageAsset("/Application/Assets/Dialog/char_gohzu_1.png",false);
					
					
					Loaded = true;
				} 
				catch (Exception e)
				{
					Console.WriteLine("Error creating a texture at Assets Manager - " + e.Message);
				}
			}
		}
		
		public void Dispose()
		{
			try
			{
				CursorTexture.Dispose();
				CursorTextureInfo.Dispose();
					
				UnitsTexture.Dispose();
				UnitsTextureInfo.Dispose();
				
				TerrainTexture.Dispose();
				TerrainTextureInfo.Dispose();
				
				BuildingsTexture.Dispose();
				BuildingsTextureInfo.Dispose();
				
				BattleTexture.Dispose();
				BattleTextureInfo.Dispose();	
				
				OverworldMapTexture.Dispose();
				OverworldMapTextureInfo.Dispose();
				
				AvatarTexture.Dispose();
				AvatarTextureInfo.Dispose();
				
				_Instance = null;
				
			} catch (Exception e){ Console.WriteLine(e.Message + " - Error disposing textures");}
		}
	}
}

