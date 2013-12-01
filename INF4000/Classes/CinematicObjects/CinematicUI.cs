using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class CinematicUI
	{		
		private Scene _UIScene;
		public Panel MainPanel;
		
		public ImageBox WhiteTween;	
		public ImageBox BlackTween;
		public ImageBox Image_Background;
		public Label Label_Text;
		
		public CinematicUI ()
		{
			MainPanel = new Panel();
            MainPanel.Width = 960;
            MainPanel.Height = 544;
			
			BlackTween = new ImageBox();
			BlackTween.Image = new ImageAsset("/Application/Assets/UI/blackbg.png",false);
			BlackTween.Width = 960;
			BlackTween.Height = 544;
			BlackTween.Alpha = 0;
			
			WhiteTween = new ImageBox();
			WhiteTween.Image = new ImageAsset("/Application/Assets/UI/whitebg.png",false);
			WhiteTween.Width = 960;
			WhiteTween.Height = 544;
			WhiteTween.Alpha = 1;
			
			Image_Background = new ImageBox();
			Image_Background.Width = 960;
			Image_Background.Height = 200;
			Image_Background.SetPosition(0.0f, 544 - 220.0f);
			Image_Background.Image = new ImageAsset("/Application/Assets/UI/turnswitch_bg.png",false);			
			Image_Background.Visible = true;
			Image_Background.Alpha = 0;
			
			// 50 char per line before \n allowed
			Label_Text = new Label();
			Label_Text.Text = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcdab\nabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcdab\nabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcdab\nbcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcdab\n";
			Label_Text.Font = new UIFont("/Application/Assets/fonts/half_bold_pixel-7.ttf", 24, FontStyle.Bold);
			Label_Text.SetPosition(35, 300);
			Label_Text.SetSize(890, 220);
			
			MainPanel.AddChildLast(Image_Background);
			MainPanel.AddChildLast(Label_Text);
			MainPanel.AddChildLast(BlackTween);
			MainPanel.AddChildLast(WhiteTween);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
		}
		
		public void Show()
		{
			UISystem.SetScene(_UIScene);
		}	
		
		public void UpdateGameStarting(float dt)
		{
			WhiteTween.Alpha += 1.2f*dt;
			Image_Background.Alpha -= dt;
			Label_Text.Alpha -= dt;
		}
		
		public void UpdateSceneEntering(float dt)
		{
			WhiteTween.Alpha -= dt/2;
			Image_Background.Alpha += dt/3;
			Label_Text.Alpha += dt;
		}
		
		public void UpdateSwitchFadeIn(float dt)
		{
			BlackTween.Alpha -= 1.5f*dt;
			Image_Background.Alpha += dt;
			Label_Text.Alpha += 1.5f*dt;
		}
		
		public void UpdateSwitchFadeOut(float dt)
		{
			BlackTween.Alpha += 1.5f*dt;
			Image_Background.Alpha -= dt;
			Label_Text.Alpha -= 1.5f*dt;
		}
	}
}

