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
		
		public Font PixelFont;
		public Font PixelFont_18;
		public Font XenoFont;
		public Font XenoFont_24;
		
		public Texture2D UnitsTexture;
		public TextureInfo UnitsTextureInfo;
		
		public Texture2D TerrainTexture;
		public TextureInfo TerrainTextureInfo;
		
		public Texture2D BuildingsTexture;
		public TextureInfo BuildingsTextureInfo;
		
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
		
		public ImageAsset Image_Icon_Fort;
		public ImageAsset Image_Icon_Farm;
		public ImageAsset Image_Icon_Grass;
		public ImageAsset Image_Icon_Road;
		
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
					PixelFont = new Font("/Application/Assets/fonts/half_bold_pixel-7.ttf", 24, FontStyle.Bold);
					PixelFont_18 = new Font("/Application/Assets/fonts/half_bold_pixel-7.ttf", 18, FontStyle.Bold);
					XenoFont = new Font("/Application/Assets/fonts/Xenogears_font.ttf", 72, FontStyle.Regular);
					XenoFont_24 = new Font("/Application/Assets/fonts/Xenogears_font.ttf", 24, FontStyle.Regular);
					
					//Units Textures
					UnitsTexture = new Texture2D ("/Application/Assets/Units/units.png", false);
					UnitsTextureInfo = new TextureInfo (UnitsTexture, new Vector2i (2, 2));
					
					// Terrain Textures
					TerrainTexture = new Texture2D ("/Application/Assets/Tiles/TilesMap_Final.png", false);
					TerrainTextureInfo = new TextureInfo (TerrainTexture, new Vector2i (8, 8));
					
					// Buildings Textures
					BuildingsTexture = new Texture2D ("/Application/Assets/Buildings/build_1.png", false);
					BuildingsTextureInfo = new TextureInfo (BuildingsTexture, new Vector2i (2, 2));
					
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
					
					Image_Icon_Fort = new ImageAsset("/Application/Assets/UI/Icons/fort_ico.png",false);
					Image_Icon_Farm = new ImageAsset("/Application/Assets/UI/Icons/farm_ico.png",false);
					Image_Icon_Grass = new ImageAsset("/Application/Assets/UI/Icons/grass_ico.png",false);
					Image_Icon_Road = new ImageAsset("/Application/Assets/UI/Icons/road_ico.png",false);
					
					Loaded = true;
				} 
				catch (Exception e)
				{
					Console.WriteLine("Error creating a texture at Assets Manager - " + e.Message);
				}
			}
		}
		
		~ AssetsManager()
		{
			UnitsTexture.Dispose();
			UnitsTextureInfo.Dispose();
			
			TerrainTexture.Dispose();
			TerrainTextureInfo.Dispose();
		}
	}
}

