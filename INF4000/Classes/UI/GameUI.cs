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
		public Label Label_TurnSwitchMessage_Gold;
		public Label Label_Loading;
		
		public Button Button_EndTurn;
		public ActionPanel ActionPanel;
		
		public AttackDamagePanel OddsPanel;
		public StatsPanel TileStatsPanel;
		public StatsPanel UnitStatsPanel;
		
		public GameUI ()
		{
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
			Label_TurnSwitchMessage.SetPosition(380, 200);
			Label_TurnSwitchMessage.Font = AssetsManager.Instance.XenoFont;
			Label_TurnSwitchMessage.Visible = false;
			
			Label_TurnSwitchMessage_Gold = new Label();
			Label_TurnSwitchMessage_Gold.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Label_TurnSwitchMessage_Gold.Width = 300;
			Label_TurnSwitchMessage_Gold.Height = 40;
			Label_TurnSwitchMessage_Gold.SetPosition(470, 280);
			Label_TurnSwitchMessage_Gold.Font = AssetsManager.Instance.XenoFont_24;
			Label_TurnSwitchMessage_Gold.Alpha = 0.0f;
			Label_TurnSwitchMessage_Gold.TextColor = new UIColor(1, 0.4f, 0, 1);
			Label_TurnSwitchMessage_Gold.Visible = false;
			
			ActionPanel = new ActionPanel(new Vector2(10, 10));
			OddsPanel = new AttackDamagePanel(new Vector2(800,130));
			TileStatsPanel = new StatsPanel(new Vector2(800, 20));
			UnitStatsPanel = new StatsPanel(new Vector2(665, 20));
			
			MainPanel.AddChildLast(Image_TurnSwitchBG);
			MainPanel.AddChildLast(Label_TurnSwitchMessage);
			MainPanel.AddChildLast(Label_TurnSwitchMessage_Gold);
			MainPanel.AddChildLast(Button_EndTurn);
			MainPanel.AddChildLast(ActivePlayerIcon);
			MainPanel.AddChildLast(ActionPanel.Panel);
			MainPanel.AddChildLast(OddsPanel.Panel);
			MainPanel.AddChildLast(TileStatsPanel.Panel);
			MainPanel.AddChildLast(UnitStatsPanel.Panel);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
		}
		
		private void SetNoneVisible()
		{
			Button_EndTurn.Visible = false;
			Label_TurnSwitchMessage.Visible = false;
			Label_TurnSwitchMessage_Gold.Visible = false;
			Image_TurnSwitchBG.Visible = false;
			ActionPanel.Panel.Visible = false;
			OddsPanel.Panel.Visible = false;
			TileStatsPanel.Panel.Visible = false;
			UnitStatsPanel.Panel.Visible = false;
		}
		
		public void AnimateSwitchitngTurn(float dt)
		{
			SetNoneVisible();
			
			// TEMP - ANIMATION
			if(Image_TurnSwitchBG.Alpha < 1)
				Image_TurnSwitchBG.Alpha += dt*3;
			
			if(Label_TurnSwitchMessage_Gold.Alpha < 1)
				Label_TurnSwitchMessage_Gold.Alpha += 1.2f*dt;
			
			Label_TurnSwitchMessage.Visible = true;
			Label_TurnSwitchMessage_Gold.Visible = true;
			Image_TurnSwitchBG.Visible = true;		
			Label_TurnSwitchMessage.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Label_TurnSwitchMessage_Gold.Text = "+ " + GameScene.Instance.ActivePlayer.GoldEarnedThisTurn.ToString() + " Gold";
		}
		
		public void SetPlaying()
		{
			Image_TurnSwitchBG.Alpha = 0;
			Label_TurnSwitchMessage_Gold.Alpha = 0;
			SetNoneVisible();
			
			TileStatsPanel.Panel.Visible = true;
			Button_EndTurn.Visible = true;
		}
		
		public void SetBattleAnimation()
		{
			SetNoneVisible();
		}
		
		public void SetPause()
		{
			
		}
		
		public void SetGameOver()
		{
			
		}
		
		public void AnimateGameOver(float dt)
		{
			SetNoneVisible();
			
			// TEMP - ANIMATION
			if(Image_TurnSwitchBG.Alpha < 1)
				Image_TurnSwitchBG.Alpha += dt*2;
			
			Label_TurnSwitchMessage.Visible = true;
			Label_TurnSwitchMessage_Gold.Visible = true;
			Image_TurnSwitchBG.Visible = true;		
			Label_TurnSwitchMessage.Text = GameScene.Instance.WinnerName + " Won The Game!";
			
			Label_TurnSwitchMessage.Width = 800;
			Label_TurnSwitchMessage.Height = 100;
			Label_TurnSwitchMessage.SetPosition(100, 200);
		}
	}
}

