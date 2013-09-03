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
		
		public Tile (int terrain, int owner, int turns, int posX, int posY)
		{	
			TerrainType = terrain;			
			TileOwner = owner;
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void AssignGraphics(Vector2i index)
		{
			SpriteTile = new SpriteTile(this.TextureInfo, index);
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
		
		public void TintToBlue()
		{
			SpriteTile.Color = new Vector4(0.2f,0.2f,0.2f,0.2f);
			//SpriteTile.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Red, 0.3f), 0.25f));
		}
		
		public void TintBackToNormal()
		{
			SpriteTile.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.White, 1f), 0.25f));
		}
	}
}

