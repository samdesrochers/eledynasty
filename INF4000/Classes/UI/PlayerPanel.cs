using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class PlayerPanel
	{
		public Panel Panel;
		private Label Label_Gold;
		private Label Label_Focus;
		private ImageBox Image_Player;

		public Vector2 Position;
		public Vector2 Right_Anchor;
		public Vector2 Left_Anchor;
		
		public PlayerPanel (Vector2 pos)
		{
			this.Position = pos;
			this.Left_Anchor = pos;
			
			Panel = new Panel();
			Panel.Width = 256;
			Panel.Height = 92;
			Panel.SetPosition(Position.X, Position.Y);
			Panel.Visible = false;
			
			Image_Player = new ImageBox();
			Image_Player.Width = Panel.Width;
			Image_Player.Height = Panel.Height;
			Image_Player.Image = AssetsManager.Instance.Image_Stats_Panel_BG;
			Image_Player.Visible = true;
			
			Label_Gold = new Label();
			Label_Gold.TextColor = new UIColor(1,0.4f,0.1f,1);
			Label_Gold.SetPosition(140, 53);
			Label_Gold.Font = AssetsManager.Instance.PixelFont_18;
			Label_Gold.Alpha = 1.0f;
			Label_Gold.Visible = true;
			
			Panel.AddChildLast(Image_Player);
			Panel.AddChildLast(Label_Gold);
		}
		
		public void SetCurrentPlayerData(ImageAsset img, string gold, string focus)
		{
			Image_Player.Image = img;
			Label_Gold.Text = gold;
		}
		
		public void SetGold(string gold)
		{
			Label_Gold.Text = gold;
		}

		public void SetActive(bool visible)
		{
			Panel.Visible = visible;
		}
		
		public void SetToRightOfScreen()
		{
			Panel.SetPosition(Right_Anchor.X, Panel.Y);
		}
		
		public void SetToLeftOfScreen()
		{
			Panel.SetPosition(Left_Anchor.X, Panel.Y);
		}
		
		public void SetTop()
		{
			Panel.SetPosition(Panel.X, 20);
		}
		
		public void SetBottom()
		{
			Panel.SetPosition(Panel.X, 430);
		}
	}
}

