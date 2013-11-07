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
		
		public float MoveTick;
		
		public Cursor (Tile tile)
		{
			SelectedTile = tile;
			
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = SelectedTile.Position;
			WorldPosition = SelectedTile.WorldPosition;
						
			this.LoadGraphics();
			MoveTick = 0.0f;
		}
		
		public void LoadGraphics()
		{		
			SpriteTile = new SpriteTile(AssetsManager.Instance.CursorTextureInfo, new Vector2i (1, 1));
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}

		public void Update()
		{
			SpriteTile.Position = SelectedTile.Position;
			Position = SelectedTile.Position;
			WorldPosition = SelectedTile.WorldPosition;
		}
		
		public void MoveToTileByWorldPosition(Vector2i newPos)
		{
			Tile t = GameScene.Instance.CurrentMap.Tiles[newPos.Y, newPos.X];
			this.SelectedTile = t;
			
			this.Update();
		}
		
		public void NavigateLeft()
		{
			if(SelectedTile.WorldPosition.X > 0)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y, SelectedTile.WorldPosition.X - 1];
				SelectedTile = newSelectedTile;
				
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_MOVE);
			}
		}
		
		public void NavigateUp()
		{
			if(SelectedTile.WorldPosition.Y < GameScene.Instance.CurrentMap.Height - 1)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y + 1, SelectedTile.WorldPosition.X];
				SelectedTile = newSelectedTile;
				
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_MOVE);
			}
		}
		
		public void NavigateRight()
		{
			if(SelectedTile.WorldPosition.X < GameScene.Instance.CurrentMap.Width - 1)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y, SelectedTile.WorldPosition.X + 1];
				SelectedTile = newSelectedTile;
				
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_MOVE);
			}
		}
		
		public void NavigateDown()
		{
			if(SelectedTile.WorldPosition.Y > 0)
			{
				Tile newSelectedTile = GameScene.Instance.CurrentMap.Tiles[SelectedTile.WorldPosition.Y - 1, SelectedTile.WorldPosition.X];
				SelectedTile = newSelectedTile;
				
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_MOVE);
			}
		}
		
		public void MoveToFirstUnit()
		{
			if(GameScene.Instance.ActivePlayer.Units[0] != null)
				MoveToTileByWorldPosition(GameScene.Instance.ActivePlayer.Units[0].WorldPosition);
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
		
		public void TintToRed()
		{
			SpriteTile.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Red, 1f), 0.3f));
		}
		#endregion
	}
}

