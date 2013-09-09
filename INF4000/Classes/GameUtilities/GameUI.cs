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
		public ImageBox Image_TurnSwitchBG;
		
		public Label Label_TurnSwitchMessage;
		public Label Label_Loading;
		
		public Button Button_EndTurn;
		public ActionPanel ActionPanel;
		
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
			ActivePlayerIcon.Visible = false;
			
			Image_TurnSwitchBG = new ImageBox();
			Image_TurnSwitchBG.Width = 960;
			Image_TurnSwitchBG.Height = 200;
			Image_TurnSwitchBG.SetPosition(0.0f, 155.0f);
			Image_TurnSwitchBG.Image = AssetsManager.Instance.Image_TurnSwitch_BG;			
			Image_TurnSwitchBG.Alpha = 0.0f;
			Image_TurnSwitchBG.Visible = false;
			
			Button_EndTurn = new Button();
            Button_EndTurn.Name = "ButtonEndTurn";
            Button_EndTurn.Text = "End Turn";
            Button_EndTurn.Width = 150;
            Button_EndTurn.Height = 60;
            Button_EndTurn.Alpha = 0.8f;
			Button_EndTurn.TextFont = AssetsManager.Instance.PixelFont;
            Button_EndTurn.SetPosition(800, 480);
            Button_EndTurn.TouchEventReceived += GameScene.Instance.EndActivePlayerTurn;
			Button_EndTurn.Visible = false;
			
			Label_TurnSwitchMessage = new Label();
			Label_TurnSwitchMessage.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Label_TurnSwitchMessage.Width = 300;
			Label_TurnSwitchMessage.Height = 100;
			Label_TurnSwitchMessage.SetPosition(400, 200);
			Label_TurnSwitchMessage.Font = AssetsManager.Instance.XenoFont;
			Label_TurnSwitchMessage.Visible = false;
			
			ActionPanel = new ActionPanel(new Vector2(150, 200));
			
			MainPanel.AddChildLast(Image_TurnSwitchBG);
			MainPanel.AddChildLast(Label_TurnSwitchMessage);
			MainPanel.AddChildLast(Button_EndTurn);
			MainPanel.AddChildLast(ActivePlayerIcon);
			MainPanel.AddChildLast(ActionPanel.Panel);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
		}
		
		private void SetNoneVisible()
		{
			Button_EndTurn.Visible = false;
			Label_TurnSwitchMessage.Visible = false;
			Image_TurnSwitchBG.Visible = false;
		}
		
		public void AnimateSwitchitngTurn(float dt)
		{
			SetNoneVisible();
			
			// TEMP - ANIMATION
			if(Image_TurnSwitchBG.Alpha < 1)
				Image_TurnSwitchBG.Alpha += dt*3;
			
			Label_TurnSwitchMessage.Visible = true;
			Image_TurnSwitchBG.Visible = true;		
			Label_TurnSwitchMessage.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
		}
		
		public void SetPlaying()
		{
			Image_TurnSwitchBG.Alpha = 0;
			SetNoneVisible();
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

