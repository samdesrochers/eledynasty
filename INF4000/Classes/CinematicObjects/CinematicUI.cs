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
			WhiteTween.Alpha += 1.8f*dt;
		}
		
		public void UpdateSceneEntering(float dt)
		{
			WhiteTween.Alpha -= dt/4;
		}
	}
}

