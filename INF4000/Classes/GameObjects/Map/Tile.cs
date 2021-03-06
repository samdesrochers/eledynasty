using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Tile : SpriteUV
	{
		public int BuildingType;
		public int TerrainType;
		
		private Unit _CurrentUnit;
		public Unit CurrentUnit
		{
			get { return _CurrentUnit; }
			set 
			{ 
				if(value != null && _CurrentUnit != null && value.UniqueId != CurrentUnit.UniqueId)
					Console.WriteLine("WTF YO");
				_CurrentUnit = value;
			}
		}
		
		
		public Building CurrentBuilding;
		public int TileOwner;
		public int BuildingTurnsToProduce;
		public int Defense;
		public int TintWeight;
		public bool IsMoveValid;
		
		public Vector2i WorldPosition;
		
		public SpriteTile SpriteTile;
		public SpriteTile ActiveOverlay;
		public SpriteTile TargetOverlay;	
		public Vector2i IdleStateIndex;
		
		public List<Vector2i> AdjacentPositions;
		public Vector2i Adjacent_Left_Pos;
		public Vector2i Adjacent_Up_Pos;
		public Vector2i Adjacent_Right_Pos;
		public Vector2i Adjacent_Down_Pos;
		
		public string Label;

		public Tile (int terrain, int owner, int posX, int posY)
		{	
			TerrainType = terrain;		

			if(TerrainType < Constants.TILE_TYPE_WATER_MIDDLE || TerrainType > Constants.TILE_TYPE_WATER_MID_EARTH_UP_LF_R)
				IsMoveValid = true;
			else 
				IsMoveValid = false;
			
			TileOwner = owner;
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void AssignGraphics(Vector2i idleIndex)
		{
			IdleStateIndex = idleIndex;
			SpriteTile = new SpriteTile(this.TextureInfo, IdleStateIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
			
			ActiveOverlay = new SpriteTile(this.TextureInfo, new Vector2i(1, 3));
			ActiveOverlay.Quad = this.Quad;
			ActiveOverlay.Position = this.Position;
			ActiveOverlay.Color = new Vector4(1,1,1,0.4f);
			
			TargetOverlay = new SpriteTile(this.TextureInfo, new Vector2i(2, 3));
			TargetOverlay.Quad = this.Quad;
			TargetOverlay.Position = this.Position;
			TargetOverlay.Color = new Vector4(1,1,1,0.4f);
		}
		
		public void AssignInfo(int defenseModifier, string info)
		{
			this.Defense = defenseModifier;
			this.Label = info;
		}
		
		public void Update()
		{
			
		}
		
		public bool HasUnit()
		{
			if(CurrentUnit != null)
				return true;
			return false;
		}
		
		public void SetActive(bool active)
		{	
			GameScene.Instance.RemoveChild(GameScene.Instance.Cursor.SpriteTile, false);
			if(active) {
				GameScene.Instance.AddChild(ActiveOverlay);
			} else {
				GameScene.Instance.RemoveChild(ActiveOverlay, false);
			}
			GameScene.Instance.AddChild(GameScene.Instance.Cursor.SpriteTile);
		}
		
		public void SetTargeted(bool target)
		{	
			GameScene.Instance.RemoveChild(GameScene.Instance.Cursor.SpriteTile, false);
			if(target) {
				GameScene.Instance.AddChild(TargetOverlay);
			} else {
				GameScene.Instance.RemoveChild(TargetOverlay, false);
			}
			GameScene.Instance.AddChild(GameScene.Instance.Cursor.SpriteTile);
		}
		
		public void AssignAdjacentTiles(Tile[,] tiles, int width, int height)
		{			
			AdjacentPositions = new List<Vector2i>();
			
			//Assign Left
			if(WorldPosition.X - 1 >= 0)
				Adjacent_Left_Pos = (tiles[WorldPosition.Y, WorldPosition.X - 1].WorldPosition);
			else
				Adjacent_Left_Pos = new Vector2i(-1,-1);
			
			//Assign Up
			if(WorldPosition.Y + 1 < height)
				Adjacent_Up_Pos = (tiles[WorldPosition.Y + 1, WorldPosition.X].WorldPosition);
			else
				Adjacent_Up_Pos = new Vector2i(-1,-1);
			
			//Assign Right
			if(WorldPosition.X + 1 < width)
				Adjacent_Right_Pos = (tiles[WorldPosition.Y, WorldPosition.X + 1].WorldPosition);
			else
				Adjacent_Right_Pos = new Vector2i(-1,-1);
			
			// Assign Down
			if(WorldPosition.Y - 1 >= 0)
				Adjacent_Down_Pos = (tiles[WorldPosition.Y - 1, WorldPosition.X].WorldPosition);
			else
				Adjacent_Down_Pos = new Vector2i(-1,-1);
			
			AdjacentPositions.Add(Adjacent_Left_Pos);
			AdjacentPositions.Add(Adjacent_Up_Pos);
			AdjacentPositions.Add(Adjacent_Right_Pos);
			AdjacentPositions.Add(Adjacent_Down_Pos);

		}
	}
}

