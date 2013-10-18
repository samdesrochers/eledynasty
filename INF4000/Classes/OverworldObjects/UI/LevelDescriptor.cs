using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class LevelDescriptor
	{
		public Panel Panel;
		public Vector2 Position;
		
		public Label LevelName;
		public Label LevelDesc;
		public Label EnemyName;
		public ImageBox EnemyAvatar;
		public ImageBox Background;
		public ImageBox Overlay1;
		public ImageBox Overlay2;
		public ImageBox Overlay3;
		
		public LevelDescriptor (Vector2 pos)
		{
			Position = pos;
			
			Panel = new Panel();
			Panel.Width = 370;
			Panel.Height = 290;
			Panel.SetPosition(Position.X, Position.Y);
			
			Background = new ImageBox();
			Background.Width = Panel.Width;
			Background.Height = Panel.Height;
			Background.Image = AssetsManager.Instance.Image_Stats_Panel_BG;
			Background.Alpha = 0.8f;
			
			Overlay1 = new ImageBox();
			Overlay1.Width = 170;
			Overlay1.Height = 200;
			Overlay1.Image = AssetsManager.Instance.Image_Black_BG;
			Overlay1.SetPosition(10, 80);
			Overlay1.Alpha = 0.4f;
			
			Overlay2 = new ImageBox();
			Overlay2.Width = 170;
			Overlay2.Height = 200;
			Overlay2.Image = AssetsManager.Instance.Image_Black_BG;
			Overlay2.SetPosition(190, 80);
			Overlay2.Alpha = 0.4f;
			
			Overlay3 = new ImageBox();
			Overlay3.Width = 350;
			Overlay3.Height = 60;
			Overlay3.Image = AssetsManager.Instance.Image_Black_BG;
			Overlay3.SetPosition(10, 10);
			Overlay3.Alpha = 0.4f;
			
			EnemyAvatar = new ImageBox();
			EnemyAvatar.Width = 120;
			EnemyAvatar.Height = 120;
			EnemyAvatar.SetPosition(40, 140);
			
			// Shadow for text
			TextShadowSettings tss = new TextShadowSettings();
			tss.HorizontalOffset = 0.5f;
			tss.Color = new UIColor(0,0,0,1);
			tss.VerticalOffset = 3;
			
			LevelName = new Label();
			LevelName.Width = Panel.Width;
			LevelName.Height = 50;
			LevelName.SetPosition(20, 15);
			LevelName.Font = AssetsManager.Instance.XenoFont_36;
			LevelName.TextShadow = tss;
			
			LevelDesc = new Label();
			LevelDesc.Width = 155;
			LevelDesc.Height = 180;
			LevelDesc.SetPosition(200,40);
			LevelDesc.Font = AssetsManager.Instance.PixelFont_18;
			
			EnemyName = new Label();
			EnemyName.Width = 300;
			EnemyName.Height = 70;
			EnemyName.SetPosition(20, 80);
			EnemyName.Font = AssetsManager.Instance.PixelFont_18;
			EnemyName.TextShadow = tss;
			
			Panel.AddChildLast(Background);
			Panel.AddChildLast(Overlay1);
			Panel.AddChildLast(Overlay2);
			Panel.AddChildLast(Overlay3);
			Panel.AddChildLast(EnemyAvatar);
			Panel.AddChildLast(LevelName);
			Panel.AddChildLast(LevelDesc);
			Panel.AddChildLast(EnemyName);
		}
		
		public void SetLevelInfo(ImageAsset image, string levelName, string levelDesc, string enemyName)
		{
			EnemyAvatar.Image = image;
			LevelName.Text = levelName;
			LevelDesc.Text = levelDesc;
			EnemyName.Text = "Opponent - \n"+enemyName;
		}
	}
}

