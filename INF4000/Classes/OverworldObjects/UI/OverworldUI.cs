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
		
		public ImageBox SceneOverlay;
		
		public OverworldUI ()
		{
			MainPanel = new Panel();
            MainPanel.Width = 960;
            MainPanel.Height = 544;
			
			SceneOverlay = new ImageBox();
			SceneOverlay.Image = AssetsManager.Instance.Image_Black_BG;
			SceneOverlay.Width = 960;
			SceneOverlay.Height = 544;
			SceneOverlay.Alpha = 0;
			
			LevelDesc = new LevelDescriptor(new Vector2(20,20));		
			MainPanel.AddChildLast(LevelDesc.Panel);
			MainPanel.AddChildLast(SceneOverlay);
			
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
			SceneOverlay.Alpha = 0;
			LevelDesc.SetLevelInfo(level.EnemyAvatar, level.Name, level.Description,level.EnemyName);
		}		
		
		public void UpdateGameStarting(float dt)
		{
			SceneOverlay.Alpha += dt;
		}
	}
}

