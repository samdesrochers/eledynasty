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
			set 
			{ 
				_Text = value; 
				this.UpdateText();
			}
		}
		
		public TextImage()
		{
			IsHidden = true;
		}
		
		public Font font;
		
		public TextImage (string text, Vector2 pos)
		{
			IsHidden = false;
			_Text = text;
			
			font = AssetsManager.Instance.PixelFont;
			
			Image img = new Image (ImageMode.Rgba, new ImageSize (300, 40), new ImageColor (255, 0, 0, 0));
			img.DrawText (_Text, new ImageColor (255, 255, 255, 255), font, new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (300, 40, false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
   
			this.Quad.S = ti.TextureSizef;
			//this.CenterSprite();		
			this.Position = new Vector2(pos.X, pos.Y);				
		}
		
		private void UpdateText()
		{
			Image img = new Image (ImageMode.Rgba, new ImageSize (300, 40), new ImageColor (255, 0, 0, 0));
			img.DrawText (_Text, new ImageColor (255, 255, 255, 255), font, new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (300, 40, false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
		}
		
		public void UpdatePosition(Vector2 pos)
		{
			this.Position = new Vector2(pos.X + 30, pos.Y - 20);
		}
	}
}

