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
		public Tile[,] Tiles;
		public string Name;
		public int Width;
		public int Height;
		private Texture2D _Texture;
		private TextureInfo _TexInfo;
		public SpriteList _SpriteList;
		public Cursor Cursor;
		private const string AssetsPath = "/Application/Assets/Tiles/tilesMap1.png";
		
		public Map (string mapFilePath)
		{
			try {
				if (!LoadMapFromFile (mapFilePath)) {
					Console.WriteLine ("Error reading Map file");
				}
				
				Cursor = new Cursor (Tiles [7, 3]);			
				LoadGraphics ();
				
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}
		
		public void Draw ()
		{
			foreach (Tile t in Tiles)
				t.Draw ();
		}
		
		public void Update ()
		{
			foreach (Tile t in Tiles)
				t.Update ();
		}
		
		private void AssignSpriteToTiles ()
		{			
			Vector2i index = new Vector2i (0, 0);
			
			// Generate Tiles Sprites 
			foreach (Tile t in Tiles) {			
				t.TextureInfo = _TexInfo;		
			
				switch (t.TerrainType) {
				case Constants.TILE_TYPE_GRASS_MIDDLE:
					index = new Vector2i (1, 3);
					break;
				case Constants.TILE_TYPE_WATER_MIDDLE:
					index = new Vector2i (3, 0);
					break;
				case Constants.TILE_TYPE_BUILD_FORT:
					index = new Vector2i (2, 3);
					break;
				case Constants.TILE_TYPE_BUILD_FARM:
					index = new Vector2i (3, 3);
					break;
				}
					
				t.CreateSpriteTile (index);
				_SpriteList.AddChild (t.SpriteTile);
			}
			
			// Generate Units Sprites -- WILL BE MOVED!!!!!!!!!!!!!!!!!!!!
			foreach(Player p in GameScene.Instance.Players)
			{
				foreach(Unit u in p.Units)
				{
					u.TextureInfo = _TexInfo;
					switch (u.Type) {
						case Constants.UNIT_TYPE_FARMER:
							index = new Vector2i (0, 3);
							break;
					}
					u.CreateSpriteTile (index);
					_SpriteList.AddChild (u.SpriteTile);
				}
			}
			
			// Create Cursor's texture and add it to the SpriteList -- WILL BE MOVED!!!!!!!!!!!!!!!!!!!!
			Cursor.TextureInfo = _TexInfo;
			Cursor.CreateSpriteTile (new Vector2i (0, 3));
			_SpriteList.AddChild (Cursor.SpriteTile);
		}
		
		#region Load Methods
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
		
		public void LoadGraphics ()
		{
			// Load textures map
			_Texture = new Texture2D (AssetsPath, false);
			_TexInfo = new TextureInfo (_Texture, new Vector2i (4, 4));
				
			// Create and fill SpriteList
			_SpriteList = new SpriteList (_TexInfo);
			_SpriteList.EnableLocalTransform = true;				
				
			AssignSpriteToTiles ();
		}
		#endregion
		
		private Tile ExtractTileFromString (string info, int posx, int posy)
		{
			int tileType;
			int tileUnit;
			int tileOwner;
			int turns;
			int moves;
			int lifePoints;
			
			Int32.TryParse (info.Substring (0, 2), out tileType);
			Int32.TryParse (info.Substring (2, 2), out tileUnit);
			Int32.TryParse (info.Substring (4, 1), out tileOwner);
			Int32.TryParse (info.Substring (5, 1), out turns);
			Int32.TryParse (info.Substring (6, 2), out moves);
			Int32.TryParse (info.Substring (8, 2), out lifePoints);
			
			// Create Tile according to extracted information
			Tile tile = new Tile (tileType, tileOwner, turns, posx, posy);
			
			// Create Unit according to extracted information
			if (tileUnit != 10) {
				
				moves -= 10;		// NOTE: these -= 10 are to adjust to the tile format
				lifePoints -= 10;	// Ex : Input : 12 HP = 2 real HP (1 is simply a buffer)
				
				Unit unit = Unit.CreateByType(tileUnit, moves, lifePoints, posx, posy);
				tile.CurrentUnit = unit;
				
				// Assign the new unit to the correct Player
				Utilities.AssignUnitToPlayer (unit, tileOwner);
			}			                   
			return tile;
		}
		
		public bool SaveMapToFile (string fileName)
		{
			return true;
		}
		
		#region Game Action Methods
		public Unit SelectUnitFromTile(Vector2i index)
		{
			if(index.X >= 0 && index.Y >= 0)
			{
				Tile t = Tiles[index.Y, index.X];
				if(t != null && t.CurrentUnit != null && t.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name)
				{
					return t.CurrentUnit;
				}
			}
			return null;
		}
		#endregion		
			
		#region Utilities
		private static bool StartsWithDelimiter (string s)
		{
			return s.StartsWith ("#");
		}
		
		~Map ()
		{
			_Texture.Dispose ();
			_TexInfo.Dispose ();
		}
		#endregion
	}
}
