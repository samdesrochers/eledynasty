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
		
		
		public TextImage (string text, Vector2 initialPosition)
		{
			IsHidden = false;
			_Text = text;
			
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
			this.Position = new Vector2(pos.X + 40, pos.Y - 8);
			UnitHP.Position = this.Position;
		}
	}
}



