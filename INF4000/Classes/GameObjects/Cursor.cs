using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Cursor : SpriteUV
	{
		public Tile SelectedTile;
		public SpriteTile SpriteTile;
		public Vector2i WorldPosition;
		
		public Cursor (Tile tile)
		{
			SelectedTile = tile;
			
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = SelectedTile.Position;
			WorldPosition = SelectedTile.WorldPosition;
		}
		
		public void Update()
		{
			SpriteTile.Position = SelectedTile.Position;
			Position = SelectedTile.Position;
		}
		
		public void NavigateLeft()
		{
			if(SelectedTile.WorldPosition.X > 0)
			{
				Tile newSelectedTile = Map.Tiles[SelectedTile.WorldPosition.Y, SelectedTile.WorldPosition.X - 1];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void NavigateUp()
		{
			if(SelectedTile.WorldPosition.Y < Map.Height - 1)
			{
				Tile newSelectedTile = Map.Tiles[SelectedTile.WorldPosition.Y + 1, SelectedTile.WorldPosition.X];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void NavigateRight()
		{
			if(SelectedTile.WorldPosition.X < Map.Width - 1)
			{
				Tile newSelectedTile = Map.Tiles[SelectedTile.WorldPosition.Y, SelectedTile.WorldPosition.X + 1];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void NavigateDown()
		{
			if(SelectedTile.WorldPosition.Y > 0)
			{
				Tile newSelectedTile = Map.Tiles[SelectedTile.WorldPosition.Y - 1, SelectedTile.WorldPosition.X];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void CreateSpriteTile(Vector2i index)
		{
			SpriteTile = new SpriteTile(this.TextureInfo, index);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
	}
}

