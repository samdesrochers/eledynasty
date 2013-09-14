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
		public Unit CurrentUnit;
		public Building CurrentBuilding;
		public int TileOwner;
		public int BuildingTurnsToProduce;
		public int Defense;
		public int TintWeight;
		
		public Vector2i WorldPosition;
		public SpriteTile SpriteTile;
		
		public Vector2i IdleStateIndex;
		public Vector2i ActiveStateIndex;
		public Vector2i TargetStateIndex;
		
		public List<Vector2i> AdjacentPosition;
		public Vector2i Adjacent_Left_Pos;
		public Vector2i Adjacent_Up_Pos;
		public Vector2i Adjacent_Right_Pos;
		public Vector2i Adjacent_Down_Pos;
		
		public string Label;

		public Tile (int terrain, int owner, int posX, int posY)
		{	
			TerrainType = terrain;			
			TileOwner = owner;
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void AssignGraphics(Vector2i idleIndex, Vector2i activeIndex, Vector2i targetIndex)
		{
			IdleStateIndex = idleIndex;
			ActiveStateIndex = activeIndex;
			TargetStateIndex = targetIndex;
			
			SpriteTile = new SpriteTile(this.TextureInfo, IdleStateIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
		
		public void AssignInfo(int defenseModifier, string info)
		{
			this.Defense = defenseModifier;
			this.Label = info;
		}
		
		public void Update()
		{
			// Update SpriteTile's position
			SpriteTile.Position = this.Position;			
		}
		
		public bool HasUnit()
		{
			if(CurrentUnit != null)
				return true;
			return false;
		}
		
		public void SetActive(bool active)
		{	
			if(active)
				SpriteTile.TileIndex2D = ActiveStateIndex;
			else
				SpriteTile.TileIndex2D = IdleStateIndex;
		}
		
		public void SetTargeted(bool target)
		{	
			if(target)
				SpriteTile.TileIndex2D = TargetStateIndex;
			else
				SpriteTile.TileIndex2D = IdleStateIndex;
		}
		
		public void AssignAdjacentTiles(Tile[,] tiles, int width, int height)
		{			
			AdjacentPosition = new List<Vector2i>();
			
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
			
			AdjacentPosition.Add(Adjacent_Left_Pos);
			AdjacentPosition.Add(Adjacent_Up_Pos);
			AdjacentPosition.Add(Adjacent_Right_Pos);
			AdjacentPosition.Add(Adjacent_Down_Pos);

		}
	}
}

