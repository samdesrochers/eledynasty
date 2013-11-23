using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class CinematicNode : SpriteUV
	{
		
		public Texture2D Texture;
		public TextureInfo TexInfo;
		public SpriteTile SpriteTile;
		
		const float ratio = 1.5f;
		
		public CinematicNode (string path)
		{
			Quad.S = new Vector2(ratio * 960 , ratio * 544);
			Position = new Vector2(0,0);		
			
			Texture2D tex2d = new Texture2D(path, false);
			TextureInfo tex = new TextureInfo(tex2d, new Vector2i(1, 1));
			
			SpriteTile = new SpriteTile(tex, new Vector2i(0,0));
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
	}
}

