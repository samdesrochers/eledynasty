using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

namespace INF4000
{
	public class TextImage : SpriteUV
	{
		public bool IsHidden;
		
		private string _Text;
		public string Text
		{
			get { return _Text; }
			set { _Text = value; }
		}
		
		public TextImage()
		{
			IsHidden = true;
		}
		
		private Font Font;
		
		public SpriteTile UnitHP;
		private const string AssetsName = "/Application/Assets/fonts/fontsheet.png";
		
		
		public TextImage (Vector2 initialPosition)
		{
			IsHidden = false;
			
			try
			{
				Quad.S = new Vector2(Constants.TEXT_SIZE, Constants.TEXT_SIZE);
				
				Texture2D tex = new Texture2D (AssetsName, false);
				this.TextureInfo = new TextureInfo (tex, new Vector2i (10, 1));
				
				// Assign part of that texture as SpriteTile for the object
				UnitHP = new SpriteTile(this.TextureInfo, new Vector2i(3, 0));
				
				UnitHP.Quad = this.Quad;			
				UnitHP.Position = initialPosition;
				UnitHP.Scale = new Vector2(0.75f, 0.75f);
			} 
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		
		public void UpdatePosition(Vector2 pos)
		{
			this.Position = new Vector2(pos.X + 40, pos.Y - 8);
			UnitHP.Position = this.Position;
		}
	}
}



