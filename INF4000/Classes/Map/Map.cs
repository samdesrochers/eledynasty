using System;
using System.IO;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Map
	{		
		public static Tile[,] Tiles;
	
		public static string Name;
		public static int Width;
		public static int Height;
		
		private Texture2D _Texture;
		private TextureInfo _TexInfo;
		public SpriteList _SpriteList;
		
		public Cursor Cursor;
		
		public Map ()
		{
			try {
				if (!LoadMapFromFile (@"/Application/MapFiles/defaultMap.txt")) {
					Console.WriteLine ("Error reading Map file");
				}
				
				Cursor = new Cursor(Tiles[7,3]);
				
				// Load textures map
				_Texture = new Texture2D ("/Application/Assets/Tiles/tilesMap1.png", false);
				_TexInfo = new TextureInfo (_Texture, new Vector2i (4, 4));
				
				// Create and fill SpriteList
				_SpriteList = new SpriteList (_TexInfo);
				_SpriteList.EnableLocalTransform = true;				
				
				GenerateSpriteTiles ();
				
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}
		
		public void Draw ()
		{
			foreach (Tile t in Tiles)
				t.Draw ();
		}
		
		public void Update()
		{
			foreach (Tile t in Tiles)
				t.Update();
		}
		
		private void GenerateSpriteTiles ()
		{			
			foreach (Tile t in Tiles) {
				
				Vector2i index = new Vector2i (0, 0);
				t.TextureInfo = _TexInfo;		
			
				switch (t.TileType) {
				case Constants.TILE_TYPE_GRASS_MIDDLE:
					index = new Vector2i (1, 3);
					break;
				case Constants.TILE_TYPE_BUILD_FORT:
					index = new Vector2i (2, 3);
					break;
				case Constants.TILE_TYPE_BUILD_FARM:
					index = new Vector2i (3, 3);
					break;
				case Constants.TILE_TYPE_WATER_MIDDLE:
					index = new Vector2i (3, 0);
					break;					
				}
					
				t.CreateSpriteTile(index);
				_SpriteList.AddChild(t.SpriteTile);
			}
			
			// Create Cursor's texture and add it to the SpriteList
			Cursor.TextureInfo = _TexInfo;
			Cursor.CreateSpriteTile(new Vector2i(0,3));
			_SpriteList.AddChild(Cursor.SpriteTile);
		}
		
		public bool LoadMapFromFile (string fileName)
		{	
			List<string> MapTextFileContent = new List<string> (System.IO.File.ReadAllLines (fileName));			
			if (MapTextFileContent == null) {
				return false;
			}
				
			// Extract Name, Width and Heigh of Map
			Name = MapTextFileContent [0];
			Int32.TryParse (MapTextFileContent [1], out Width);
			Int32.TryParse (MapTextFileContent [2], out Height);
			
			// Clean Map strings list to keep tiles only
			MapTextFileContent.RemoveRange (0, 3);
			MapTextFileContent.RemoveAll (StartsWithDelimiter);
			
			// Generate the Tiles Map
			Tiles = new Tile[Height, Width];
			int tileCounter = 0;
			
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Width; j++) {
					string tileDefinition = MapTextFileContent [tileCounter];
					Tiles [i, j] = ExtractTileFromString (tileDefinition, j, i);
					tileCounter++;
				}
			}
			
			return true;
		}
		
		private Tile ExtractTileFromString (string info, int posx, int posy)
		{
			int isBuilding;
			int tileType;
			int tileUnit;
			int tileOwner;
			int turns;
			
			Int32.TryParse (info.Substring (0, 1), out isBuilding);
			Int32.TryParse (info.Substring (1, 2), out tileType);
			Int32.TryParse (info.Substring (3, 2), out tileUnit);
			Int32.TryParse (info.Substring (5, 1), out tileOwner);
			Int32.TryParse (info.Substring (6, 2), out turns);
	
			return new Tile (isBuilding, tileType, tileUnit, tileOwner, turns, posx, posy);
		}
		
		public bool SaveMapToFile (string fileName)
		{
			return true;
		}
			
		private static bool StartsWithDelimiter (string s)
		{
			return s.StartsWith ("#");
		}
		
		~Map ()
		{
			_Texture.Dispose ();
			_TexInfo.Dispose ();
		}
	}
}

