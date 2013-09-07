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
		
		public Label Label_TurnSwitchMessage;
		public Label Label_TurnSwitchPlayerName;
		
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
			Button_EndTurn.TextFont = AssetsManager.Instance.PixelFont;
            Button_EndTurn.SetPosition(800, 480);
            Button_EndTurn.TouchEventReceived += GameScene.Instance.EndActivePlayerTurn;
			
			Label_TurnSwitchMessage = new Label();
			Label_TurnSwitchMessage.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Label_TurnSwitchMessage.Width = 300;
			Label_TurnSwitchMessage.Height = 100;
			Label_TurnSwitchMessage.SetPosition(400, 100);
			Label_TurnSwitchMessage.Font = AssetsManager.Instance.XenoFont;
			Label_TurnSwitchMessage.Visible = false;
			
			MainPanel.AddChildLast(Label_TurnSwitchMessage);
			MainPanel.AddChildLast(Button_EndTurn);
			MainPanel.AddChildLast(ActivePlayerIcon);
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
		}
		
		public void SetSwitchitngTurn()
		{
			Label_TurnSwitchMessage.Visible = true;
			Label_TurnSwitchMessage.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Button_EndTurn.Visible = false;
		}
		
		public void SetPlaying()
		{
			Label_TurnSwitchMessage.Visible = false;
			Button_EndTurn.Visible = true;
		}
		
		public void SetPause()
		{
			
		}
		
		public void SetGameOver()
		{
			
		}
	}
}

