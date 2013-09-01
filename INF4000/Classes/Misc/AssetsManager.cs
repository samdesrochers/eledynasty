using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class AssetsManager
	{
		public SpriteUV DebugSprite;
		public SpriteUV DebugSprite1;
		
		public List<SpriteUV> Sprites;
		
		public AssetsManager ()
		{
			Sprites = new List<SpriteUV>();
			
			DebugSprite = new SpriteUV();
			DebugSprite1 = new SpriteUV();
			
			Sprites.Add(DebugSprite);
			Sprites.Add(DebugSprite1);

		}
		
		public void Draw()
		{
			foreach(SpriteUV sp in Sprites)
			{
				if(sp.TextureInfo != null)
					sp.Draw();
			}		
		}
	}
}

