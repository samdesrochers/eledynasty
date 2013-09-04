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
		
		public Texture2D Texture;
		
		private const string AssetsPath = "/Application/Assets/Items/gameItems.png";
		
		public Cursor (Tile tile)
		{
			SelectedTile = tile;
			
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = SelectedTile.Position;
			WorldPosition = SelectedTile.WorldPosition;
						
			this.LoadGraphics(new Vector2i(1,1));
		}
		
		public void LoadGraphics(Vector2i index)
		{
			// Create the actual texture object and specify its size
			Texture = new Texture2D (AssetsPath, false);
			this.TextureInfo = new TextureInfo (Texture, new Vector2i (2, 2));
			
			// Assign part of that texture as SpriteTile for the object
			SpriteTile = new SpriteTile(this.TextureInfo, index);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}

		public void Update()
		{
			SpriteTile.Position = SelectedTile.Position;
			Position = SelectedTile.Position;
			WorldPosition = SelectedTile.WorldPosition;
		}
		
		public void NavigateLeft()
		{
			if(SelectedTile.WorldPosition.X > 0)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y, SelectedTile.WorldPosition.X - 1];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void NavigateUp()
		{
			if(SelectedTile.WorldPosition.Y < GameScene.Instance.CurrentMap.Height - 1)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y + 1, SelectedTile.WorldPosition.X];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void NavigateRight()
		{
			if(SelectedTile.WorldPosition.X < GameScene.Instance.CurrentMap.Width - 1)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y, SelectedTile.WorldPosition.X + 1];
				SelectedTile = newSelectedTile;
			}
		}
		
		public void NavigateDown()
		{
			if(SelectedTile.WorldPosition.Y > 0)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y - 1, SelectedTile.WorldPosition.X];
				SelectedTile = newSelectedTile;
			}
		}
		
		#region Animations
		public void TintToWhite()
		{
			SpriteTile.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.White, 1f), 0.3f));
		}
		
		public void TintToBlue()
		{
			SpriteTile.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.LightBlue, 1f), 0.3f));
		}
		#endregion
	}
}

