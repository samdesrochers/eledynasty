using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class StatItem
	{
		public Panel Panel;
		public Vector2 Position;
		private ImageBox Image_StatIcon;
		public Label Text_Stat;
		private string Text;
		
		public StatItem (string text, Vector2 pos, ImageAsset image)
		{
			Position = pos;
			Text = text;
			
			Panel = new Panel();
			Panel.SetPosition(Position.X, Position.Y);		
            Panel.Height = 40;
            Panel.Width = 50;
			
			Image_StatIcon = new ImageBox();
			Image_StatIcon.Width = Constants.TILE_SIZE/2;
			Image_StatIcon.Height = Constants.TILE_SIZE/2;
			Image_StatIcon.Image = image;
			Image_StatIcon.SetPosition(0,0);
			Image_StatIcon.Visible = true;
		
			Text_Stat = new Label();
			Text_Stat.Text = Text;
			Text_Stat.SetPosition(35,4);
			Text_Stat.Font = AssetsManager.Instance.PixelFont_18;
			Text_Stat.Visible = true;
			
			Panel.AddChildLast(Image_StatIcon);
			Panel.AddChildLast(Text_Stat);
		}
	}
}

