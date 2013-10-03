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
		public List<Tile> ActiveTiles;
		
		public string Name;
		public int Width;
		public int Height;

		public TextureInfo TexInfo;	
		public SpriteList SpriteList;
		
		public Map (string mapFilePath)
		{
			try {
				if (!LoadMapFromFile (mapFilePath)) {
					Console.WriteLine ("Error reading Map file");
				}
		
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
		
		private void LoadTerrainGraphics ()
		{						
			// Generate Tiles Sprites 
			foreach (Tile t in Tiles) 
			{			
				t.TextureInfo = TexInfo;		
			
				List<Vector2i> indexes = TilesUtil.GetTileIndexesByType(t.TerrainType);
					
				t.AssignGraphics (indexes[0], indexes[1], indexes[2]);
				SpriteList.AddChild (t.SpriteTile);
			}
		}
		
		#region Load Methods
		public bool LoadMapFromFile (string fileName)
		{	
			List<string> MapTextFileContent = new List<string> (System.IO.File.ReadAllLines (fileName));			
			if (MapTextFileContent == null) {
				return false;
			}
				
			// Extract MapName, Width and Heigh of Map
			Name = MapTextFileContent [0];
			Int32.TryParse (MapTextFileContent [1], out Width);
			Int32.TryParse (MapTextFileContent [2], out Height);
			
			// Get Players Name for this level
			GameScene.Instance.Players[0].Name = MapTextFileContent [3];
			GameScene.Instance.Players[1].Name = MapTextFileContent [4];
			
			// Clean Map strings list to keep tiles only
			MapTextFileContent.RemoveRange (0, 5);
			MapTextFileContent.RemoveAll (StartsWithDelimiter);
			
			// Generate the Tiles Map
			Tiles = new Tile[Height, Width];
			int tileCounter = 0;
			
			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Width; j++) {
					string tileDefinition = MapTextFileContent [tileCounter];
					Tiles [i, j] = ExtractTileInfo (tileDefinition, j, i);
					tileCounter++;
				}
			}
			
			foreach(Tile t in Tiles)
				t.AssignAdjacentTiles(Tiles, Width, Height);
			
			return true;
		}
		
		public void LoadGraphics ()
		{
			// Load textures map
			TexInfo = AssetsManager.Instance.TerrainTextureInfo;
				
			// Create and fill SpriteList
			SpriteList = new SpriteList (TexInfo);				
			LoadTerrainGraphics ();
		}
		#endregion
		
		private Tile ExtractTileInfo (string info, int posx, int posy)
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
			Tile tile = new Tile (tileType, tileOwner, posx, posy);
			Tuple<int, string> tuple = TilesUtil.GetTileInfo(tile.TerrainType);
			tile.AssignInfo(tuple.Item1, tuple.Item2);
			
			// Create Unit according to extracted information
			if (tileUnit != Constants.UNIT_TYPE_NONE) {
				
				moves -= 10;		// NOTE: these -= 10 are to adjust to the tile format
				lifePoints -= 10;	// Ex : Input : 12 HP = 2 real HP (1 is simply a buffer)
				
				Unit unit = Unit.CreateByType(tileUnit, moves, lifePoints, posx, posy);
				
				// Assign the new unit to the correct Player
				Utilities.AssignUnitToPlayerByIndex (unit, tileOwner);
				
				unit.LoadGraphics();
				tile.CurrentUnit = unit;
			}
			
			// Create Building according to extracted information
			if (tileType >= 50 && tileType < 60) {
				
				List<int> stats = BuildingUtil.GetStatsByType(tileType);
				List<Vector2i> indexes = BuildingUtil.GetTileIndexesByType(tileType, tileOwner);
				Building build = new Building(tileType, posx, posy, stats[0], stats[1], stats[2], stats[3], BuildingUtil.GetNameByType(tileType));
				build.AssignGraphics(indexes[0], indexes[1], indexes[2]);
				tile.CurrentBuilding = build;
				
				// Assign the new building to the correct Player (if any)
				Utilities.AssignBuildingToPlayer (build, tileOwner);
				
				
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
				
				// All conditions to know that a unit is selectable and movable this turn
				if(t != null && t.CurrentUnit != null && t.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name && t.CurrentUnit.Move_RadiusLeft > 0)
				{
					SelectActiveTiles(t, t.CurrentUnit.Move_RadiusLeft);
					t.CurrentUnit.Select();
					return t.CurrentUnit;
				}
			}
			return null;
		}
		
		public Building SelectBuildingFromTile(Vector2i index)
		{
			if(index.X >= 0 && index.Y >= 0)
			{
				Tile t = Tiles[index.Y, index.X];
				
				// All conditions to know that a unit is selectable and movable this turn
				if(t != null && t.CurrentBuilding != null && t.CurrentBuilding.OwnerName == GameScene.Instance.ActivePlayer.Name && t.CurrentBuilding.CanProduceThisTurn)
				{
					return t.CurrentBuilding;
				}
			}
			return null;
		}
		
		public Tile SelectTileFromPosition(Vector2i index)
		{
			if(index.X >= 0 && index.Y >= 0)
			{
				Tile t = Tiles[index.Y, index.X];				
				return t;		
			}
			return null;
		}
		#endregion		
			
		#region Utilities
		private void SelectActiveTiles(Tile initialTile, int radius) // OBSOLETE
		{
			ActiveTiles = new List<Tile>();
			Vector2i initPos = initialTile.WorldPosition;
			
			for(int i = 0; i < 2*radius + 1; i++)
			{
				Vector2i p0 = new Vector2i(initPos.Y + radius - i, initPos.X);
				if(p0.X >= 0 && p0.Y >= 0 && p0.X < Height && p0.Y < Width)
				{
					Tile t = Tiles[p0.X, p0.Y];
					
					if(i <= radius)
						t.TintWeight = i;
					else
						t.TintWeight = 2*radius - i;
					ActiveTiles.Add(t);
				}
			}
			
			List<Tile> selectedTiles = new List<Tile>();
			foreach(Tile t in ActiveTiles)
			{
				Vector2i linkPos = t.WorldPosition;
				for(int i = 0; i <= t.TintWeight*2; i++)
				{
					int p1 = linkPos.X - t.TintWeight + i; // X and Y inverted for Tiles[]
					if(p1 >= 0 && p1 < Width && linkPos.Y < Height && linkPos.Y >= 0)
					{				
						Tile candidate = Tiles[linkPos.Y, p1];
						
						if(candidate.IsMoveValid) // Water Check
							selectedTiles.Add(candidate);
					}
				}
			}
			
			ActiveTiles.Clear();
			foreach(Tile mt in selectedTiles)
				ActiveTiles.Add(mt);
			
			foreach(Tile at in ActiveTiles)
				at.SetActive(true);
		}
		
		public void SetTargetedTiles(Tile origin)
		{
			foreach(Vector2i v in origin.AdjacentPositions){
				if(this.GetTile(v) != null)
				{
					Tile t = this.GetTile(v);
					if(t.CurrentUnit != null && t.CurrentUnit.OwnerName != GameScene.Instance.ActivePlayer.Name)
					{
						GameScene.Instance.ActivePlayer.TargetUnits.Add(t.CurrentUnit);
						t.SetTargeted(true);
					}
				}
			}
		}
		
		public Tile GetTile(Vector2i worldPos)
		{
			if(worldPos.X >= 0 && worldPos.Y >= 0 && worldPos.X < Width && worldPos.Y < Height)
				return Tiles[worldPos.Y, worldPos.X];
			return null;
		}
		
		public void ResetActiveTiles()
		{
			UnTintAllTiles();
			foreach(Tile t in ActiveTiles)
				t.SetActive(true);
		}
	
		public void UnTintAllTiles()
		{
			foreach(Tile t in Tiles)
			{
				t.TintWeight = 0;
				t.SetActive(false);
			}		
		}
		
		private static bool StartsWithDelimiter (string s)
		{
			return s.StartsWith ("#");
		}
		
		~Map ()
		{
			TexInfo.Dispose ();
		}
		#endregion
	}
}
