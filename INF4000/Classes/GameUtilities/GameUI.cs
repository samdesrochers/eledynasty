using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class GameUI
	{
		private Scene _UIScene;
		public Panel MainPanel;
		
		public ImageBox ActivePlayerIcon;
		public Button Button_EndTurn;
		
		public GameUI ()
		{
			// UI -----------------
			MainPanel = new Panel();
            MainPanel.Width = 960;
            MainPanel.Height = 544;
			
			ActivePlayerIcon = new ImageBox();
            
			ActivePlayerIcon.Width = 130;
			ActivePlayerIcon.Height = 80;
            ActivePlayerIcon.SetPosition(0.0f,0.0f);
			
			Button_EndTurn = new Button();
            Button_EndTurn.Name = "ButtonEndTurn";
            Button_EndTurn.Text = "End Turn";
            Button_EndTurn.Width = 150;
            Button_EndTurn.Height = 60;
            Button_EndTurn.Alpha = 0.8f;
			Button_EndTurn.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_EndTurn.SetPosition(800, 480);
            Button_EndTurn.TouchEventReceived += GameScene.Instance.EndActivePlayerTurn;
			
			MainPanel.AddChildLast(Button_EndTurn);
			MainPanel.AddChildLast(ActivePlayerIcon);
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
		}
	}
}

