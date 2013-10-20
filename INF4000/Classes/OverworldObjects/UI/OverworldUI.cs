using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class OverworldUI
	{		
		private Scene _UIScene;
		public Panel MainPanel;
		public LevelDescriptor LevelDesc;
		
		public ImageBox WhiteTween;	
		public ImageBox BlackTween;
		
		public OverworldUI ()
		{
			MainPanel = new Panel();
            MainPanel.Width = 960;
            MainPanel.Height = 544;
			
			BlackTween = new ImageBox();
			BlackTween.Image = AssetsManager.Instance.Image_Black_BG;
			BlackTween.Width = 960;
			BlackTween.Height = 544;
			BlackTween.Alpha = 0;
			
			WhiteTween = new ImageBox();
			WhiteTween.Image = AssetsManager.Instance.Image_White_BG;
			WhiteTween.Width = 960;
			WhiteTween.Height = 544;
			WhiteTween.Alpha = 1;
			
			LevelDesc = new LevelDescriptor(new Vector2(20,20));		
			MainPanel.AddChildLast(LevelDesc.Panel);
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
		
		public void SetLevelInfo(Level level)
		{
			BlackTween.Alpha = 0;
			LevelDesc.SetLevelInfo(level.EnemyAvatar, level.Name, level.Description,level.EnemyName);
		}	
		
		public void AdjustPosition(Level level)
		{
			if(level.Position.X > 960/2)
				LevelDesc.Panel.SetPosition(20, 20);
			else
				LevelDesc.Panel.SetPosition(560, 220);
		}
		
		public void UpdateGameStarting(float dt)
		{
			BlackTween.Alpha += 1.8f*dt;
		}
		
		public void UpdateSceneEntering(float dt)
		{
			WhiteTween.Alpha -= dt;
			if(WhiteTween.Alpha <= 0)
				MainPanel.RemoveChild(WhiteTween);
		}
	}
}

