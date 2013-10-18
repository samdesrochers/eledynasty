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
		
		/* Animation fields*/
		private Vector2i faceIndex;
		private int currentFrameIndexX = 0;
		private int nbFrames = 3;
		private float FrameTime = 0.4f;		
		private float AnimationTime;
		
		public Avatar ()
		{
			Quad.S = new Vector2(48, 48);
			Position = new Vector2(670,315);					
			faceIndex = new Vector2i(0, 3);
			
			SpriteTile = new SpriteTile(AssetsManager.Instance.AvatarTextureInfo, faceIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
		
		public override void Update(float dt)
		{
			base.Update(dt);
			
			AnimationTime += dt;
			
			if(AnimationTime >= FrameTime) {
				currentFrameIndexX = (currentFrameIndexX + 1) % nbFrames;
				SpriteTile.TileIndex2D = new Vector2i(currentFrameIndexX, faceIndex.Y);
				AnimationTime = 0;
			}
		}
	}
}

