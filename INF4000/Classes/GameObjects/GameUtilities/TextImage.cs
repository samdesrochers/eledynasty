using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

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
		
		public UIFont font;
		private List<int> Size;
		public float Alpha;
		
		public TextImage (string text, Vector2 pos)
		{
			IsHidden = false;
			_Text = text;
			Size = new List<int>();
			Size.Add(300);
			Size.Add(40);
			Alpha = 1.0f;
			
			font = AssetsManager.Instance.PixelFont_18;
			
			Image img = new Image (ImageMode.Rgba, new ImageSize (Size[0], Size[1]), new ImageColor (255, 0, 0, 0));
			
			img.DrawText (_Text, new ImageColor (255, 255, 255, 255), font, new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (Size[0], Size[1], false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
   
			this.Quad.S = ti.TextureSizef;
			this.Position = new Vector2(pos.X, pos.Y);				
		}
		
		public TextImage (string text, Vector2 pos, int size)
		{
			IsHidden = false;
			_Text = text;
			Size = new List<int>();
			Size.Add(size);
			Size.Add(size);
			Alpha = 1.0f;
			
			font = AssetsManager.Instance.PixelFont_48;
			
			Image img = new Image (ImageMode.Rgba, new ImageSize (Size[0], Size[1]), new ImageColor (255, 0, 0, 0));
			
			img.DrawRectangle(new ImageColor(0,0,0,1), new Sce.PlayStation.Core.Imaging.ImageRect((int)pos.X - size/2, (int)pos.Y + size/2, size, size));
			img.DrawText (_Text, new ImageColor (255, 255, 255, 255), font, new ImagePosition (0, 0));
  			
			Texture2D texture = new Texture2D (Size[0], Size[1], false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
   
			this.Quad.S = ti.TextureSizef;
			this.Position = new Vector2(pos.X, pos.Y);				
		}
		
		private void UpdateText()
		{
			Image img = new Image (ImageMode.Rgba, new ImageSize (Size[0], Size[1]), new ImageColor (255, 0, 0, 0));
			img.DrawText (_Text, new ImageColor (255, 255, 255, 255), font, new ImagePosition (0, 0));
  
			Texture2D texture = new Texture2D (Size[0], Size[1], false, PixelFormat.Rgba);
			texture.SetPixels (0, img.ToBuffer ());
			img.Dispose ();                                  
   
			TextureInfo ti = new TextureInfo ();
			ti.Texture = texture;

			this.TextureInfo = ti;
			this.Position = this.Position;
		}
		
		public void UpdatePositionUnit(Vector2 pos)
		{
			this.Position = new Vector2(pos.X + 30, pos.Y - 20);
		}
		
		public void UpdatePositionBattle(Vector2 pos)
		{
			this.Position = new Vector2(pos.X, pos.Y);
		}
	}
}

