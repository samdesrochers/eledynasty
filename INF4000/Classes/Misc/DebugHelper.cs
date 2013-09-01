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
	public class DebugHelper : SpriteUV
	{
		public bool IsHidden;
		
		private string _DebugText;
		public string Text
		{
			get { return _DebugText; }
			set 
			{ 
				_DebugText = value; 
				this.UpdateText();
			}
		}
		
		public DebugHelper()
		{
			IsHidden = true;
		}
		
		public DebugHelper (string text)
		{
			IsHidden = false;
			_DebugText = text;
			
			Image img = new Image (ImageMode.Rgba, new ImageSize (300, 40), new ImageColor (255, 0, 0, 0));
			img.DrawText (_DebugText, new ImageColor (255, 0, 0, 255), new Font (FontAlias.System, 24, FontStyle.Regular), new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (300, 40, false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
   
			this.Quad.S = ti.TextureSizef;
			//this.CenterSprite();		
			this.Position = new Vector2(100,40);				
		}
		
		public void UpdateText()
		{
			Image img = new Image (ImageMode.Rgba, new ImageSize (300, 40), new ImageColor (255, 0, 0, 0));
			img.DrawText (_DebugText, new ImageColor (255, 0, 0, 255), new Font (FontAlias.System, 24, FontStyle.Regular), new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (300, 40, false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
		}
	}
}

