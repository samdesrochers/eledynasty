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
		
		public ImageAsset Image_TurnSwitch_BG;
		public ImageAsset Image_Player_1_Icon;
		public ImageAsset Image_Action_Panel_BG;
		public ImageAsset Image_Panel_Attack_Icon;
		public ImageAsset Image_Panel_Wait_Icon;
		public ImageAsset Image_Panel_Cancel_Icon;
		public ImageAsset Image_Panel_OddsBubble;
		
		public ImageAsset Image_Stats_Panel_BG;
		public ImageAsset Image_Panel_Def_Icon;
		public ImageAsset Image_Panel_Dmg_Icon;
		public ImageAsset Image_Panel_HP_Icon;
		public ImageAsset Image_Panel_Gld_Icon;
		
		public ImageAsset Image_Kenji_UI_Turn;
		public ImageAsset Image_Gohzu_UI_Turn;
		
		public ImageAsset Image_Icon_Fort;
		public ImageAsset Image_Icon_Farm;
		public ImageAsset Image_Icon_Grass;
		public ImageAsset Image_Icon_Road;
		
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
					BattleTextureInfo = new TextureInfo (BattleTexture, new Vector2i (2, 2));
					
					// UI Elements
					Image_TurnSwitch_BG = new ImageAsset("/Application/Assets/UI/turnswitch_bg.png",false);
					Image_Player_1_Icon = new ImageAsset("/Application/Assets/Players/araki.png",false);
					Image_Action_Panel_BG = new ImageAsset("/Application/Assets/UI/panel_bg.png",false);
					Image_Panel_Wait_Icon = new ImageAsset("/Application/Assets/UI/wait_icon.png",false);
					Image_Panel_Attack_Icon = new ImageAsset("/Application/Assets/UI/attack_icon.png",false);
					Image_Panel_Cancel_Icon = new ImageAsset("/Application/Assets/UI/back_icon.png",false);
					Image_Panel_OddsBubble = new ImageAsset("/Application/Assets/UI/odds_bubble.png",false);
					
					Image_Stats_Panel_BG = new ImageAsset("/Application/Assets/UI/stats_panel.png",false);
					Image_Panel_Def_Icon = new ImageAsset("/Application/Assets/UI/def_ico.png",false);
					Image_Panel_Dmg_Icon = new ImageAsset("/Application/Assets/UI/dmg_ico.png",false);
					Image_Panel_HP_Icon = new ImageAsset("/Application/Assets/UI/hp_ico.png",false);
					Image_Panel_Gld_Icon = new ImageAsset("/Application/Assets/UI/gold_ico.png",false);
					
					Image_Icon_Fort = new ImageAsset("/Application/Assets/UI/Icons/fort_ico.png",false);
					Image_Icon_Farm = new ImageAsset("/Application/Assets/UI/Icons/farm_ico.png",false);
					Image_Icon_Grass = new ImageAsset("/Application/Assets/UI/Icons/grass_ico.png",false);
					Image_Icon_Road = new ImageAsset("/Application/Assets/UI/Icons/road_ico.png",false);
					
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
				_Instance = null;
				
			} catch (Exception e){ Console.WriteLine(e.Message + " - Error disposing textures");}
		}
	}
}

