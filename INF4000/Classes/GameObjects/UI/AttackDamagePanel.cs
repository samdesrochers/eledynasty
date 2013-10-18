using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class AttackDamagePanel
	{
		public Panel Panel;
		private Label Label_Odds;
		private ImageBox Image_Background;
		
		public bool IsActive;
		public Vector2 Position;
		
		public AttackDamagePanel (Vector2 pos)
		{
			IsActive = false;
			Position = pos;
			
			Panel = new Panel();
			Panel.Width = Constants.UI_ELEMENT_STATSBOX_WIDTH;;
			Panel.Height = 30;
			Panel.SetPosition(Position.X, Position.Y);
			Panel.Visible = false;
			
			Image_Background = new ImageBox();
			Image_Background.Width = Panel.Width;
			Image_Background.Height = Panel.Height;
			//Image_Background.SetPosition(Position.X, Position.Y);
			Image_Background.Image = AssetsManager.Instance.Image_Stats_Panel_BG;
			Image_Background.Alpha = 0.85f;
			Image_Background.Visible = true;
			
			Label_Odds = new Label();
			Label_Odds.Text = "Damage:99%"; // never go full 100 bro
			Label_Odds.TextColor = new UIColor(1,1,1,1);
			Label_Odds.SetPosition(5, 3);
			Label_Odds.Font = AssetsManager.Instance.PixelFont_18;
			Label_Odds.Alpha = 1.0f;
			Label_Odds.Visible = true;
			
			Panel.AddChildLast(Image_Background);
			Panel.AddChildLast(Label_Odds);
		}
		
		public void SetOdds(string odds)
		{
			Label_Odds.Text = "Damage:" + odds + "%";
		}
		
		public void SetActive(bool visible)
		{
			Panel.Visible = visible;
		}
		
		public void SetPosition(Vector2 pos)
		{
			this.Position = pos;
			if(pos.Y <= 0)
				pos = new Vector2(pos.X, pos.Y + 128);
			else if (pos.Y >= 544)
				pos = new Vector2(pos.X, pos.Y - 128);
			
			Panel.SetPosition(pos.X, pos.Y);
		}
	}
}

