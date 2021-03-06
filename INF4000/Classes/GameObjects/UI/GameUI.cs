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
		const int PlayerIconPos_X_Right = 690;
		
		private Scene _UIScene;
		public Panel MainPanel;
	
		public ImageBox Image_TurnSwitchBG;
		public ImageBox Image_StartTurnBG;
		
		public Label Label_TurnSwitchMessage;
		public Label Label_TurnSwitchMessage_Gold;
		public Label Label_Pause;
		
		public Button Button_EndTurn;
		public ActionPanel ActionPanel;
		
		public AttackDamagePanel OddsPanel;
		public StatsPanel TileStatsPanel;
		public StatsPanel UnitStatsPanel;
		
		public PlayerPanel PlayerPanel;
		private bool Starting_SwitchingTurn;
		
		public GameUI ()
		{
			MainPanel = new Panel();
            MainPanel.Width = 960;
            MainPanel.Height = 544;
			
			Starting_SwitchingTurn = true;
			
			Image_TurnSwitchBG = new ImageBox();
			Image_TurnSwitchBG.Width = 960;
			Image_TurnSwitchBG.Height = 200;
			Image_TurnSwitchBG.SetPosition(0.0f, 155.0f);
			Image_TurnSwitchBG.Image = AssetsManager.Instance.Image_TurnSwitch_BG;			
			Image_TurnSwitchBG.Alpha = 0.0f;
			Image_TurnSwitchBG.Visible = false;
			
			Image_StartTurnBG = new ImageBox();
			Image_StartTurnBG.Width = 960;
			Image_StartTurnBG.Height = 544;
			Image_StartTurnBG.Image = AssetsManager.Instance.Image_Black_BG;			
			Image_StartTurnBG.Alpha = 1.0f;
			
			Button_EndTurn = new Button();
            Button_EndTurn.IconImage = AssetsManager.Instance.Image_Button_EndTurn;
			Button_EndTurn.Width = 107;
            Button_EndTurn.Height = 53;
            Button_EndTurn.Alpha = 0.9f;
            Button_EndTurn.TouchEventReceived += GameScene.Instance.EndActivePlayerTurn;
			Button_EndTurn.Visible = false;
			
			Label_TurnSwitchMessage = new Label();
			Label_TurnSwitchMessage.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Label_TurnSwitchMessage.Width = 300;
			Label_TurnSwitchMessage.Height = 100;
			Label_TurnSwitchMessage.SetPosition(380, 200);
			Label_TurnSwitchMessage.Font = AssetsManager.Instance.XenoFont;
			Label_TurnSwitchMessage.Visible = false;
			
			Label_Pause = new Label();
			Label_Pause.Text = "Game Paused";
			Label_Pause.Width = 600;
			Label_Pause.Height = 60;
			Label_Pause.SetPosition(230, 220);
			Label_Pause.Font = AssetsManager.Instance.XenoFont;
			Label_Pause.Visible = false;
			
			Label_TurnSwitchMessage_Gold = new Label();
			Label_TurnSwitchMessage_Gold.Text = "Turn " + GameScene.Instance.CurrentTurnCount;
			Label_TurnSwitchMessage_Gold.Width = 300;
			Label_TurnSwitchMessage_Gold.Height = 40;
			Label_TurnSwitchMessage_Gold.SetPosition(470, 280);
			Label_TurnSwitchMessage_Gold.Font = AssetsManager.Instance.XenoFont_24;
			Label_TurnSwitchMessage_Gold.Alpha = 0.0f;
			Label_TurnSwitchMessage_Gold.TextColor = new UIColor(1, 0.4f, 0, 1);
			Label_TurnSwitchMessage_Gold.Visible = false;
			
			ActionPanel = new ActionPanel(new Vector2(23, 109));
			ActionPanel.Right_Anchor = new Vector2(PlayerIconPos_X_Right + 3, 109);
			
			OddsPanel = new AttackDamagePanel(new Vector2(800,130));
			
			PlayerPanel = new PlayerPanel(new Vector2(20, 20));
			PlayerPanel.Right_Anchor = new Vector2(PlayerIconPos_X_Right, 20);
			
			TileStatsPanel = new StatsPanel(new Vector2(800, 20));
			TileStatsPanel.Left_Anchor = new Vector2(20, 20);
			
			UnitStatsPanel = new StatsPanel(new Vector2(665, 20));
			UnitStatsPanel.Left_Anchor = new Vector2(155, 20);
			
			MainPanel.AddChildLast(Image_TurnSwitchBG);
			MainPanel.AddChildLast(Label_TurnSwitchMessage);
			MainPanel.AddChildLast(Label_TurnSwitchMessage_Gold);
			MainPanel.AddChildLast(Button_EndTurn);
			MainPanel.AddChildLast(PlayerPanel.Panel);
			MainPanel.AddChildLast(ActionPanel.Panel);
			MainPanel.AddChildLast(OddsPanel.Panel);
			MainPanel.AddChildLast(TileStatsPanel.Panel);
			MainPanel.AddChildLast(UnitStatsPanel.Panel);
			MainPanel.AddChildLast(Image_StartTurnBG);
			MainPanel.AddChildLast(Label_Pause);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
		}
		
		public void SetNoneVisible()
		{
			Button_EndTurn.Visible = false;
			PlayerPanel.Panel.Visible = false;
			Label_TurnSwitchMessage.Visible = false;
			Label_TurnSwitchMessage_Gold.Visible = false;
			Image_TurnSwitchBG.Visible = false;
			ActionPanel.Panel.Visible = false;
			OddsPanel.Panel.Visible = false;
			TileStatsPanel.Panel.Visible = false;
			UnitStatsPanel.Panel.Visible = false;
			Label_Pause.Visible = false;
		}
		
		public void AnimateSwitchitngTurn(float dt)
		{
			SetNoneVisible();
			
			if(Starting_SwitchingTurn)
			{
				PlayerPanel.SetCurrentPlayerData(GameScene.Instance.ActivePlayer.Icon, GameScene.Instance.ActivePlayer.Gold.ToString(), "");
				
				if(GameScene.Instance.ActivePlayer.Name == Constants.CHAR_KENJI) {
					SoundManager.Instance.PlayKenjiGameSong();
				} else {
					SoundManager.Instance.PlayEnemyGameSong();
				}
				Starting_SwitchingTurn = false;
			}
			
			// ANIMATION
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
		
		public void AnimateStartingGame(float dt)
		{
			Image_StartTurnBG.Alpha -= 2*(dt/3);
			
			if(Image_StartTurnBG.Alpha <= 0)
				MainPanel.RemoveChild(Image_StartTurnBG);
		}
		
		public void SetPlaying()
		{
			Image_TurnSwitchBG.Alpha = 0;
			Label_TurnSwitchMessage_Gold.Alpha = 0;
			SetNoneVisible();
			
			PlayerPanel.Panel.Visible = true;
			TileStatsPanel.Panel.Visible = true;
			Button_EndTurn.Visible = true;
			
			Starting_SwitchingTurn = true;
		}
		
		public void SetBattleAnimation()
		{
			SetNoneVisible();
		}
		
		public void SetPause()
		{
			SetNoneVisible();
			Image_TurnSwitchBG.Visible = true;
			Image_TurnSwitchBG.Alpha = 1.0f;
			Label_Pause.Visible = true;
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
		
		public void SetActive()
		{
			UISystem.SetScene(_UIScene);
		}
		
		public void SetEndTurn_Left()
		{
			Button_EndTurn.SetPosition(20, 492);
		}
		
		public void SetEndTurn_Right()
		{
			Button_EndTurn.SetPosition(830, 492);
		}
	}
}

