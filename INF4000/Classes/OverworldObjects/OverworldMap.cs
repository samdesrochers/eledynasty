using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class OverworldMap : SpriteUV
	{
		//private string LevelsManifestPath;
		
		public SpriteTile SpriteTile;
		
		public OverworldMap ()
		{
			Quad.S = new Vector2(960, 544);
			Position = new Vector2(0,0);		
			
			SpriteTile = new SpriteTile(AssetsManager.Instance.OverworldMapTextureInfo, new Vector2i(0,0));
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
	}
}

