using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Avatar : SpriteUV
	{
		public SpriteTile SpriteTile;
		public Vector2 Destination;
		
		/* Animation fields*/
		private Vector2i faceIndex;
		private Vector2i leftIndex;
		private Vector2i rightIndex;
		public int Direction;
		private int currentFrameIndexX = 0;
		private int nbFrames = 3;
		private float FrameTime = 0.3f;		
		private float AnimationTime;
		
		public Avatar ()
		{
			Quad.S = new Vector2(48, 48);
			Position = new Vector2(670,315);
			Destination = Position;
			faceIndex = new Vector2i(0, 3);
			leftIndex = new Vector2i(0, 2);
			rightIndex = new Vector2i(0, 1);
			Direction = Constants.AVATAR_DIRECTION_UP;
			
			SpriteTile = new SpriteTile(AssetsManager.Instance.AvatarTextureInfo, faceIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
		
		public override void Update(float dt)
		{
			base.Update(dt);
			Animate(dt);
			
			if(Destination.X == Position.X && Destination.Y == Position.Y)
				Direction = Constants.AVATAR_DIRECTION_UP;
			
		}
		
		public void Animate(float dt)
		{
			AnimationTime += dt;
			if(AnimationTime >= FrameTime) {
				currentFrameIndexX = (currentFrameIndexX + 1) % nbFrames;
				
				int indexY = 0;
				if(Direction == Constants.AVATAR_DIRECTION_UP)
					indexY = faceIndex.Y;
				else if(Direction == Constants.AVATAR_DIRECTION_LEFT)
					indexY = leftIndex.Y;
				else if(Direction == Constants.AVATAR_DIRECTION_RIGHT)
					indexY = rightIndex.Y;
				
				SpriteTile.TileIndex2D = new Vector2i(currentFrameIndexX, indexY);
				AnimationTime = 0;
			}
		}
	}
}

