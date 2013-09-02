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
		
		public TextImage (string text, Vector2 initialPosition)
		{
			IsHidden = false;
			_Text = text;
			
			try
			{
				Font = new Font("/Application/Assets/Fonts/font.ttf", 18, FontStyle.Bold);
				
				Image img = new Image (ImageMode.Rgba, new ImageSize (50, 50), new ImageColor (255, 255, 255, 0));
				img.DrawText (_Text, new ImageColor (0, 0, 0, 255), Font, new ImagePosition (0, 0));
	  
				Texture2D texture = new Texture2D (50, 50, false, PixelFormat.Rgba);
				texture.SetPixels (0, img.ToBuffer ());
				img.Dispose ();                                  
	   
				TextureInfo ti = new TextureInfo ();
				ti.Texture = texture;
	
				this.TextureInfo = ti;
	   
				this.Quad.S = ti.TextureSizef;	
				this.Position = new Vector2(initialPosition.X + 40,initialPosition.Y - 100);	
			} 
			catch(Exception e)
			{
				Console.WriteLine(e.Message);				
				Image img = new Image (ImageMode.Rgba, new ImageSize (50, 50), new ImageColor (255, 255, 255, 0));
				img.DrawText (_Text, new ImageColor (0, 0, 0, 255), new Font (FontAlias.System, 18, FontStyle.Regular), new ImagePosition (0, 0));
	  
				Texture2D texture = new Texture2D (50, 50, false, PixelFormat.Rgba);
				texture.SetPixels (0, img.ToBuffer ());
				img.Dispose ();                                  
	   
				TextureInfo ti = new TextureInfo ();
				ti.Texture = texture;
	
				this.TextureInfo = ti;
	   
				this.Quad.S = ti.TextureSizef;	
				this.Position = new Vector2(initialPosition.X + 40,initialPosition.Y - 100);
			}
		}

		public void UpdateText(Vector2 pos)
		{
			Image img = new Image (ImageMode.Rgba, new ImageSize (50, 50), new ImageColor (255, 255, 255, 0));
			img.DrawText (_Text, new ImageColor (255, 255, 255, 255), Font, new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (50, 50, false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
			this.Position = new Vector2(pos.X ,pos.Y);
		}
		
		public void AssignRelativePosition(Vector2 pos)
		{
			this.Position = new Vector2(pos.X + 38, pos.Y - 21);
		}
	}
}



