using System;
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
		public int TileOwner;
		public int BuildingTurnsToProduce;
		public int TerrainModifier;
		public int TintWeight;
		
		public Vector2i WorldPosition;
		public SpriteTile SpriteTile;
		
		public Vector2i IdleStateIndex;
		public Vector2i ActiveStateIndex;
		
		public Tile (int terrain, int owner, int posX, int posY)
		{	
			TerrainType = terrain;			
			TileOwner = owner;
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void AssignGraphics(Vector2i idleIndex, Vector2i activeIndex)
		{
			IdleStateIndex = idleIndex;
			ActiveStateIndex = activeIndex;
			
			SpriteTile = new SpriteTile(this.TextureInfo, IdleStateIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
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
	}
}

