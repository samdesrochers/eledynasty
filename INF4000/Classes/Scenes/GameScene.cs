using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class GameScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		// Singleton Class
		private static GameScene _Instance;
		public static GameScene Instance {
			get {
				if (_Instance == null) {
					_Instance = new GameScene ();
				}
				return _Instance;
			}
		}
		
		/*******************************
		 * FIELDS
		 *******************************/
		public GameUI GameUI;
		
		public DialogUI DialogUI;
		public DialogManager DialogManager;
		
		public int CurrentGlobalState;
		public int CurrentGameState;
		
		public Map CurrentMap;
		public Cursor Cursor;
		
		public Player ActivePlayer;
		public int ActivePlayerIndex;	
		
		public List<Player> Players;
		public string WinnerName;
		
		public BattleViewer BattleViewer;
		
		private float SwitchTurnTime;
		private bool TurnSwitchInit = true;
		public int CurrentTurnCount = 1;
		
		public GameScene ()
		{
			Console.WriteLine("CREATING GAME SCENE");
			_Instance = this;
			this.CurrentGlobalState = Constants.GLOBAL_STATE_STARTING_GAME;
			this.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			
			this.Camera.SetViewFromViewport ();

			// Load the Assets
			AssetsManager.Instance.LoadAssets();
			
			// Load the Sounds
			SoundManager.Instance.LoadSounds();
			SoundManager.Instance.PlayIntroMapSong();
			
			// Create the Players
			Players = new List<Player> ();
			Player player1 = new HumanPlayer ();
			player1.IsActive = true;
			Players.Add (player1);
			
			Player player2 = new AIPlayer ();
			player2.Icon = AssetsManager.Instance.Image_Gohzu_UI_Turn;
			Players.Add (player2);				
			
			// Create the selected Map and its assets
			CurrentMap = new Map ( OverworldScene.Instance.SelectedLevel.GameLevelPath );   
			this.AddChild (CurrentMap.SpriteList);	
			
			// Add the units assets to the Main SpriteList
			Utilities.LoadAllSpritesFromPlayer (Players, this);
			
			// Create Cursor
			Cursor = new Cursor (CurrentMap.Tiles [3, 3]);
			this.AddChild (Cursor.SpriteTile);		
			
			// Select the initial Active Player
			ActivePlayer = SelectActivePlayer ();
			ActivePlayer.CollectGoldFromBuildings();

			// Create the battle viewer for animations
			BattleViewer = new BattleViewer();
			
			// Set the Buildings stack for capture animations
			BuildingUtil.LastCapturedBuildings = new Stack<Building>();
			
			GameUI = new GameUI();
			GameUI.PlayerPanel.SetCurrentPlayerData(ActivePlayer.Icon, ActivePlayer.Gold.ToString(), "");
			
			DialogUI = new DialogUI();
			DialogManager.SetNextSequence(); // Created in the Map, when the dialog are extracted;
			
			ActivePlayer.IsStartingTurn = true;
			SwitchTurnTime = 0.0f;	
			CurrentTurnCount = 1;
									
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
		}
		
		public override void Draw ()
        {
            base.Draw();
            UISystem.Render ();
        }

		public override void Update (float dt)
		{
			base.Update (dt);
			switch(this.CurrentGlobalState)
			{
				case Constants.GLOBAL_STATE_STARTING_GAME:
					UpdateStartingGame(dt);
					break;
				case Constants.GLOBAL_STATE_PLAYING_TURN:
					UpdateGameRunning(dt);
					break;
				case Constants.GLOBAL_STATE_SWITCHING_TURN:
					UpdateGameSwitchingTurn(dt);
					break;
				case Constants.GLOBAL_STATE_BATTLE_ANIMATION:
					UpdateBattleAnimation(dt);
					break;
				case Constants.GLOBAL_STATE_CAPTURE_ANIMATION:
					UpdateCaptureAnimation(dt);
					break;
				case Constants.GLOBAL_STATE_DIALOG:
					UpdateDialog(dt);
					break;
				case Constants.GLOBAL_STATE_PAUSE:
					//UpdateGameRunning();
					break;
				case Constants.GLOBAL_STATE_GAMEOVER:
					UpdateGameOver(dt);
					break;
			}
		}
		
		#region Update Methods
		private void UpdateGameRunning(float dt)
		{
			if(ActivePlayer.IsStartingTurn)
			{
				InitPlayerTurn();
				if(ActivePlayer.IsHuman) {
					Cursor.SpriteTile.Visible = true;
					Utilities.ShowStatsPanels();
				} else {
					ActivePlayer.Reset();
					Cursor.SpriteTile.Visible = false;
					Utilities.HideStatsPanels();
				}
			}
			
			UpdateCameraPosition ();
			
			// Human or AI behavior
			if (ActivePlayer is HumanPlayer) {
				UpdateGameRunningHuman(dt);
			} else {
				UpdateGameRunningAI(dt);
			}
		
			UpdateUnits ();			
			UpdateMap ();	
			
			CheckIsGameOver ();				
			if(CheckIfTurnIsOver())
			{
				// Turn is done, switch to next player
				SwitchToNextPlayer();
			}	
		}
		
		private void UpdateGameRunningHuman(float dt)
		{
			if(CurrentGameState == Constants.GAME_STATE_ACTIONPANEL_ACTIVE)
				UpdateActionPanelSelection();
			else if(CurrentGameState == Constants.GAME_STATE_ATTACKPANEL_ACTIVE)
				UpdateAttackPanelSelection();
			else 
				UpdateCursorPosition (dt);
				
			CheckUserInput ();
			
			if(CurrentGameState == Constants.GAME_STATE_SELECTION_INACTIVE || CurrentGameState == Constants.GAME_STATE_ATTACKPANEL_ACTIVE){
				Utilities.ShowStatsPanels();
				Utilities.ShowEndTurnButton();
				UpdateStatsPanel();
			} else {
				Utilities.HideStatsPanels();
				Utilities.HideEndTurnButton();
			}
		}
		
		private void UpdateGameRunningAI(float dt)
		{
			// Will update the AI player's update
			ActivePlayer.Update(dt);
		}
		
		private void UpdateGameSwitchingTurn(float dt)
		{
			if(TurnSwitchInit)
			{
				TurnSwitchInit = false;
				SoundManager.Instance.PlaySound(Constants.SOUND_TURN_START);
			}
			
			SwitchTurnTime += dt;			
			GameUI.AnimateSwitchitngTurn(dt);

			if(SwitchTurnTime >= 2.0f || Input2.GamePad0.Cross.Release) // wait 2 seconds before switching turn
			{
				CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
				GameUI.SetPlaying();
				SwitchTurnTime = 0.0f;
			}
		}
		
		private void UpdateBattleAnimation(float dt)
		{
			BattleViewer.Update(dt);
		}
		
		private void UpdateCaptureAnimation(float dt)
		{
			BuildingUtil.Update(dt);
		}
		
		private void UpdateDialog(float dt)
		{
			DialogManager.Update(dt);
		}
		
		private void UpdateGameOver(float dt)
		{
			SwitchTurnTime += dt;			
			GameUI.AnimateGameOver(dt);
			SoundManager.Instance.FadeOut(dt);

			if(SwitchTurnTime >= 5.0f || Input2.GamePad0.Cross.Release) // wait 2 seconds before switching turn
			{
				Dispose();			
				
				
				OverworldScene.Instance.Reset();
				Director.Instance.ReplaceScene( OverworldScene.Instance );		
			}
		}
		
		private void UpdateStartingGame(float dt)
		{
			GameUI.AnimateStartingGame(dt);		
			if(GameUI.Image_StartTurnBG.Alpha <= 0) {
				//Utilities.ShowDialogUI();
				CurrentGlobalState = Constants.GLOBAL_STATE_SWITCHING_TURN;
			}
		}
		
		#region Camera Update methods
		private void ResetCamera()
		{
			Console.WriteLine("Reseting Camera 1");
			Camera2D camera = this.Camera as Camera2D;
			if(this.ActivePlayer.Units[0] != null && ActivePlayer.IsHuman) {
				float topLimitY = CurrentMap.Height * 64;
				float topCameraY = camera.Center.Y + 544/2;
				float leftCameraX = camera.Center.X - 960/2;
				Vector2 uPos = this.ActivePlayer.Units[0].Position;
				
				float diffTopY = uPos.Y - topCameraY;
				if(diffTopY <= 0) {
					camera.Center = new Vector2(camera.Center.X, topLimitY + diffTopY - 64);
				}
				if(uPos.X <= leftCameraX && leftCameraX <= 960/2) {
					camera.Center = new Vector2(960/2, camera.Center.Y);
				}
				if(topCameraY >= topLimitY)
					camera.Center = new Vector2(camera.Center.X, topLimitY - 544/2);
			}			
		}
		
		private void UpdateCameraPosition ()
		{	
			Utilities.AdjustUIPanelsLocation();
		}
		
		public void UpdateCameraCycle()
		{
			Console.WriteLine("Cycling Camera 2");
			if(ActivePlayer.ActiveUnit == null)
				return;
			
			Camera2D camera = this.Camera as Camera2D;	
			Vector2 unitPos = ActivePlayer.ActiveUnit.Position;		
			Vector2 newCamCenter = unitPos;
			
			float mapWidth = CurrentMap.Width * 64;
			float mapHeigth = CurrentMap.Height * 64;
			
			float delimiterRight = newCamCenter.X + 960/2;
			float delimiterLeft = newCamCenter.X - 960/2;
			float delimiterUp = newCamCenter.Y + 544/2;
			float delimiterDown = newCamCenter.Y - 544/2;
			
			if(delimiterRight > mapWidth) {
				float diff = mapWidth - delimiterRight;
				camera.Center = new Vector2(newCamCenter.X + diff, camera.Center.Y);
			}
			
			if(newCamCenter.X < 480) {
				camera.Center = new Vector2(480, camera.Center.Y);
			}
			
			if(delimiterUp >= mapHeigth) {
				camera.Center = new Vector2(camera.Center.X, mapHeigth - 544/2);
			}
			
			if(newCamCenter.Y < 544/2) {
				camera.Center = new Vector2(camera.Center.X, 544/2);
			}
		}
		
		public void UpdateCameraPositionByCursor ()
		{
			//Console.WriteLine("Updating By Cursor Camera 3");
			Camera2D camera = this.Camera as Camera2D;
			Vector2 pos = new Vector2(Cursor.WorldPosition.X * 64, Cursor.WorldPosition.Y * 64);

			if(pos.X >= 960/2 && pos.X <= CurrentMap.Width * 64 - (960/2) + 64) {
				camera.Center = new Vector2 (Cursor.Position.X - 32, camera.Center.Y);
			} 
			
			if(pos.Y >= 544/2 && pos.Y <= CurrentMap.Height * 64 - (544/2) + 64) {
				camera.Center = new Vector2 (camera.Center.X, Cursor.Position.Y  - 16);
			} else if(pos.Y < 544/2) {
				camera.Center = new Vector2 (camera.Center.X, 544/2);
			}
		}
		
		public void UpdateCameraPositionBySelectedUnit ()
		{
			Console.WriteLine("Updating Camera By Selected Unit 1");
			if(ActivePlayer.ActiveUnit == null)
				return;
			
			Camera2D camera = this.Camera as Camera2D;
			Vector2 pos = ActivePlayer.ActiveUnit.Position;

			if(pos.X >= 960/2 && pos.X <= CurrentMap.Width * 64 - (960/2) + 64) {
				camera.Center = new Vector2 (ActivePlayer.ActiveUnit.Position.X - 32, camera.Center.Y);
			} 
			
			if(pos.Y >= 544/2 && pos.Y <= CurrentMap.Height * 64 - (544/2) + 64) {
				camera.Center = new Vector2 (camera.Center.X, ActivePlayer.ActiveUnit.Position.Y  - 16);
			} else if(pos.Y < 544/2) {
				camera.Center = new Vector2 (camera.Center.X, 544/2);
			} else if(pos.Y > 544/2) {
				if(pos.X >= 960/2)
					camera.Center = new Vector2 (camera.Center.X, CurrentMap.Height*64 - 544/2);
				else
					camera.Center = new Vector2 (960/2, CurrentMap.Height*64 - 544/2);
			}
			
			if(pos.X > (CurrentMap.Width*64) - 960/2)
				camera.Center = new Vector2 ((CurrentMap.Width*64) - 960/2, camera.Center.Y);
				
		}
		
		#endregion

		private void UpdateCursorPosition (float dt)
		{
			// "On" Section
			if(Input2.GamePad.GetData(0).Right.Press) {
				Cursor.NavigateRight();
			} else if(Input2.GamePad.GetData(0).Left.Press) {
				Cursor.NavigateLeft();
			} else if(Input2.GamePad.GetData(0).Up.Press) {
				Cursor.NavigateUp();
			} else if(Input2.GamePad.GetData(0).Down.Press) {
				Cursor.NavigateDown();	
			} else if (Input2.GamePad.GetData (0).Up.Press && Input2.GamePad.GetData (0).Right.Press) { // Up + Right
				Cursor.NavigateUp ();
				Cursor.NavigateRight ();
			} else if (Input2.GamePad.GetData (0).Up.Press && Input2.GamePad.GetData (0).Right.Press) { // Up + Left
				Cursor.NavigateUp ();
				Cursor.NavigateLeft ();
			} else if (Input2.GamePad.GetData (0).Down.Press && Input2.GamePad.GetData (0).Right.Press) { // Down + Right
				Cursor.NavigateDown ();
				Cursor.NavigateRight ();
			} else if (Input2.GamePad.GetData (0).Down.Press && Input2.GamePad.GetData (0).Right.Press) { // Down + Left
				Cursor.NavigateDown ();
				Cursor.NavigateLeft ();
			}

			
			// "Down" Section
			if(Input2.GamePad.GetData(0).Right.Down) {
				Cursor.MoveTick += dt;		
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateRight();
					Cursor.MoveTick = 0;
				}		
			} else if(Input2.GamePad.GetData(0).Left.Down) {
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateLeft();
					Cursor.MoveTick = 0;
				}
			} else if(Input2.GamePad.GetData(0).Up.Down) {
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateUp();
					Cursor.MoveTick = 0;
				}
			} else if(Input2.GamePad.GetData(0).Down.Down) {
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateDown();
					Cursor.MoveTick = 0;
				}
			} else if (Input2.GamePad.GetData (0).Up.Release && Input2.GamePad.GetData (0).Right.Release) { // Up + Right
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateUp ();
					Cursor.NavigateRight ();
					Cursor.MoveTick = 0;
				}
			} else if (Input2.GamePad.GetData (0).Up.Release && Input2.GamePad.GetData (0).Right.Release) { // Up + Left
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateUp ();
					Cursor.NavigateLeft ();
					Cursor.MoveTick = 0;
				}
			} else if (Input2.GamePad.GetData (0).Down.Release && Input2.GamePad.GetData (0).Right.Release) { // Down + Right
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateDown ();
					Cursor.NavigateRight ();
					Cursor.MoveTick = 0;
				}
			} else if (Input2.GamePad.GetData (0).Down.Release && Input2.GamePad.GetData (0).Right.Release) { // Down + Left
				Cursor.MoveTick += dt;
				if(Cursor.MoveTick >= Constants.CURSOR_TICK_TIME) {
					Cursor.NavigateDown ();
					Cursor.NavigateLeft ();
					Cursor.MoveTick = 0;
				}
			}
			
			// "Release Section"
			if (Input2.GamePad.GetData (0).Right.Release || Input2.GamePad.GetData (0).Left.Release || Input2.GamePad.GetData (0).Up.Release || Input2.GamePad.GetData (0).Down.Release) {
				Cursor.MoveTick = 0;
			}
			
			Cursor.Update ();	
			UpdateCameraPositionByCursor();
		}
		
		private void UpdateActionPanelSelection()
		{
			if (Input2.GamePad.GetData (0).Up.Release) {
				GameUI.ActionPanel.FocusNextItemUp();
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			} else if(Input2.GamePad.GetData (0).Down.Release) {
				GameUI.ActionPanel.FocusNextItemDown();
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}
		}
		
		private void UpdateAttackPanelSelection()
		{
			if (Input2.GamePad.GetData (0).Up.Release || Input2.GamePad.GetData (0).Right.Release) 
			{
				Utilities.CycleEnemyUnitsRight();
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			} 
			else if(Input2.GamePad.GetData (0).Down.Release || Input2.GamePad.GetData (0).Left.Release) 
			{
				Utilities.CycleEnemyUnitsRight();
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}
		}
		
		private void UpdateMap ()
		{
			CurrentMap.Update ();
			if(BuildingUtil.LastCapturedBuildings.Count > 0 && CurrentGlobalState != Constants.GLOBAL_STATE_GAMEOVER)
			{
				CurrentGlobalState = Constants.GLOBAL_STATE_CAPTURE_ANIMATION;
			}
		}
		
		private void UpdateUnits ()
		{
			foreach (Player p in Players) {
				foreach (Unit u in p.Units) {
					u.Update ();
				}
			}
		}
		
		private void UpdateStatsPanel()
		{
			Tile t = Cursor.SelectedTile;
			if(t != null)
			{
				GameUI.TileStatsPanel.SetElements(t.Defense, 0, 0, 0, t.Label, t.TerrainType, null, 0);
				GameUI.TileStatsPanel.SetConfiguration(Constants.UI_ELEMENT_CONFIG_STATS_TERRAIN);
				
				if(t.CurrentBuilding != null)
				{
					GameUI.TileStatsPanel.SetElements(t.CurrentBuilding.Defense, 0, 0, t.CurrentBuilding.GoldPerTurn, t.CurrentBuilding.Label, t.CurrentBuilding.Type, t.CurrentBuilding.OwnerName, t.CurrentBuilding.PointsToCapture);
					GameUI.TileStatsPanel.SetConfiguration(Constants.UI_ELEMENT_CONFIG_STATS_BUILDING);
				} 
				
				if(t.CurrentUnit != null)
				{
					GameUI.UnitStatsPanel.SetElements(t.CurrentUnit.Armor, t.CurrentUnit.AttackDamage, t.CurrentUnit.LifePoints, 0, t.CurrentUnit.Label, t.CurrentUnit.Type, t.CurrentUnit.OwnerName, 0);
					GameUI.UnitStatsPanel.SetConfiguration(Constants.UI_ELEMENT_CONFIG_STATS_UNIT);
					GameUI.UnitStatsPanel.IsActive = true;
				} else {
					GameUI.UnitStatsPanel.IsActive = false;
				}
			}
		}
		#endregion
		
		#region Game Loop Methods
		private void InitPlayerTurn()
		{
			TurnSwitchInit = true;
			
			// Move Cursor to new player's first unit
			Cursor.MoveToFirstUnit();						
			UpdateCameraPositionByCursor();
			ResetCamera();
			
			ActivePlayer.IsStartingTurn = false;
		}
		
		private void SwitchToNextPlayer()
		{
			// Finish current player's turn
			ActivePlayer.ResetUnits();
			ActivePlayer.ResetBuildings();
			ActivePlayer.CollectGoldFromBuildings(); // for next turn
			
			if(ActivePlayer.ActiveUnit != null && ActivePlayer.IsHuman)
			{
				if(ActivePlayer.ActiveUnit.Path.RadiusUsed > 0)
					ActivePlayer.ActiveUnit.RevertMove();
				
				ActivePlayer.ActiveUnit.Unselect ();
				CurrentMap.UnTintAllTiles();
				Cursor.TintToWhite();
			}
			ActivePlayer.ActiveUnit = null;
			ActivePlayer.LastAction = -1; //void last action
			
			// Switch Player and add a turn to the counter
			ActivePlayerIndex++;		
			ActivePlayer = Players [ ActivePlayerIndex % Players.Count ];
			ActivePlayer.IsStartingTurn = true;
			CurrentTurnCount ++;	
			
			// Reset States
			CurrentGlobalState = Constants.GLOBAL_STATE_SWITCHING_TURN;
			CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			
			//Reset UI
			GameUI.ActionPanel.SetActive(false);		
		}
		
		private bool CheckIsGameOver ()
		{
			foreach (Player p in this.Players) {
				if (p.Units.Count == 0)
				{
					CurrentGlobalState = Constants.GLOBAL_STATE_GAMEOVER;
					WinnerName = Utilities.GetWinner();
				}
			}
			return false;
		}
		
		private Player SelectActivePlayer ()
		{
			int index = 0;
			foreach (Player p in this.Players) {
				if (p.IsActive) {
					ActivePlayerIndex = index;
					return p;
				}
				ActivePlayerIndex++;
			}
			return null;
		}
			
		private void ExecuteTurn ()
		{
			
		}
		
		private bool CheckIfTurnIsOver()
		{
			if(!ActivePlayer.HasMovableUnits() && CurrentGameState == Constants.GAME_STATE_SELECTION_INACTIVE && ActivePlayer.IsHuman)
				return true;
			else if (!ActivePlayer.IsHuman && ((AIPlayer)ActivePlayer).IsTurnOver )
				return true;
			
			return false;
		}
		
		public void EndActivePlayerTurn(object sender, TouchEventArgs e)
		{
			if(CurrentGlobalState == Constants.GLOBAL_STATE_PLAYING_TURN)
			{
				GameUI.Button_EndTurn.Visible = false;
				SwitchToNextPlayer();
			}
		}
		#endregion
		
		#region User Input
		private void CheckUserInput ()
		{
			if (this.CurrentGameState == Constants.GAME_STATE_SELECTION_INACTIVE) // "X" Pressed, No units selected at the moment
			{ 
				CrossPressed_LastStateIdle();
			} 
			else if (this.CurrentGameState == Constants.GAME_STATE_UNIT_SELECTION_ACTIVE) 
			{				
				// User selects a destination Tile and Presses "X"
				if (Input2.GamePad.GetData (0).Cross.Release) 
				{
					CrossPressed_LastStateUnitSelectionActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release) 
				{
					CirclePressed_LastStateActive();
					SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_CANCEL);
				}
			}
			else if(this.CurrentGameState == Constants.GAME_STATE_ACTIONPANEL_ACTIVE)
			{
				// User selects an option in the Action panel Presses "X"
				if (Input2.GamePad.GetData (0).Cross.Release && GameUI.ActionPanel.IsActive()) 
				{
					CrossPressed_LastStateActionPanelActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release && GameUI.ActionPanel.IsActive()) 
				{
					CirclePressed_LastStateActionPanelActive();
				}
			}
			else if(this.CurrentGameState == Constants.GAME_STATE_ATTACKPANEL_ACTIVE)
			{
				// User selects an option in the Action panel Presses "X"
				if (Input2.GamePad.GetData (0).Cross.Release) 
				{
					CrossPressed_LastStateAttackPanelActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release) 
				{
					CirclePressed_LastStateAttackPanelActive();
				}
			}
			
			// Right Bumper Pressed
			if (Input2.GamePad.GetData (0).R.Release) 
			{   
				if(this.CurrentGameState == Constants.GAME_STATE_SELECTION_INACTIVE)
					Utilities.CycleThroughUnits();
			}
			
			// Sqaure Pressed
			if (Input2.GamePad.GetData (0).Square.Release) 
			{ 
				Utilities.ShowGameUI();
				//UI.ActionSelectionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_MOVE_CANCEL_ATTACK);
			}
			
			// Triangle Pressed
			if (Input2.GamePad.GetData (0).Triangle.Release) 
			{ 
				Utilities.ShowDialogUI();
				SoundManager.Instance.PlayIntroMapSong();
				CurrentGlobalState = Constants.GLOBAL_STATE_DIALOG;
				//UI.ActionSelectionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_MOVE_CANCEL);
			}
		}
		
		#region Cross Pressed Actions
		private void CrossPressed_LastStateIdle ()
		{
			if (Input2.GamePad.GetData (0).Cross.Release) 
			{ 
				Unit selectedUnit = CurrentMap.SelectUnitFromTile (Cursor.WorldPosition);
				Building selectedBuild = CurrentMap.SelectBuildingFromTile (Cursor.WorldPosition);
					
				if (selectedUnit != null)  // Actual Unit was found on tile
				{			
					CurrentGameState = Constants.GAME_STATE_UNIT_SELECTION_ACTIVE;
					ActivePlayer.ActiveUnit = selectedUnit;
					ActivePlayer.ActiveBuilding = selectedBuild;
					ActivePlayer.ActiveTile = CurrentMap.SelectTileFromPosition (Cursor.WorldPosition);
					Cursor.TintToBlue();
				} 
				else if(selectedBuild != null && selectedUnit == null && selectedBuild.Type != Constants.BUILD_FORT)
				{
					CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
					GameUI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_CANCEL_PRODUCE);
					Utilities.ShowActionPanel();
					
					ActivePlayer.ActiveBuilding = selectedBuild;
					Cursor.TintToBlue();
				}
				
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}	
		}
		
		private void CrossPressed_LastStateUnitSelectionActive()
		{
			if(Cursor.WorldPosition != ActivePlayer.ActiveUnit.WorldPosition)  //User wants to move the unit
			{
				Path path = new Path ();				
				int action = path.GetDestinationAction (Cursor.WorldPosition, ActivePlayer.ActiveUnit.WorldPosition);
				ActivePlayer.LastAction = action;
				
				if (action == Constants.ACTION_MOVE) {						
					GameActions.PrepareUnitMove(path);							
				} else if (action == Constants.ACTION_CANCEL) {				
					GameActions.CancelUnitMove();					
				} else if (action == Constants.ACTION_ATTACK) {
					GameActions.PrepareUnitAttack(path);
				}
				GameUI.ActionPanel.TryAddCaptureItem(Cursor.WorldPosition);
			}
			else // User selected same tile as ActiveUnit's tile, which may or may not have a building
			{
				int action = Utilities.GetUnitActionsChoices(Cursor.WorldPosition, ActivePlayer.ActiveBuilding );
				ActivePlayer.LastAction = action;
				
				if (action == Constants.ACTION_SLEEP) {		
					GameActions.PrepareUnitSleep();				
				} else if (action == Constants.ACTION_NOMOVE_ATTACK) {
					GameActions.PrepareUnitAttackFromOrigin();
				} else if (action == Constants.ACTION_PRODUCE) {		
					GameActions.PrepareUnitSleepOrProduce();			
				} else if (action == Constants.ACTION_PRODUCE_ATTACK) {
					GameActions.PrepareUnitAttackFromOriginOrProduce();
				}
			}
			SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
		}
		
		private void CrossPressed_LastStateActionPanelActive()
		{
			int UIAction = GameUI.ActionPanel.GetSelectedAction();
			
			if(UIAction == Constants.UI_ELEMENT_ACTION_TYPE_ATTACK) // ATTACK Button pressed
			{
				// Unit is attacking from its original position
				if(ActivePlayer.LastAction == Constants.ACTION_NOMOVE_ATTACK)
				{
					GameActions.TargetEnemyUnits();
				} else {// Unit just moved and wants to attack 
					GameActions.TargetEnemyUnits();
				}
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}
			else if(UIAction == Constants.UI_ELEMENT_ACTION_TYPE_WAIT) // WAIT Button pressed
			{
				if(ActivePlayer.LastAction == Constants.ACTION_SLEEP || ActivePlayer.LastAction == Constants.ACTION_NOMOVE_ATTACK
				   || ActivePlayer.LastAction == Constants.ACTION_PRODUCE || ActivePlayer.LastAction == Constants.ACTION_PRODUCE_ATTACK)
				{				
					GameActions.SleepSelectedUnit();
				} else {	
					GameActions.MoveSelectedUnit();
				}
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}
			else if(UIAction == Constants.UI_ELEMENT_ACTION_TYPE_PRODUCE) // PRODUCE button pressed
			{
				GameActions.ProduceUnit(Cursor.WorldPosition, ActivePlayer.Name);
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			}
			else if(UIAction == Constants.UI_ELEMENT_ACTION_TYPE_CANCEL) // CANCEL button pressed
			{
				if(ActivePlayer.LastAction == Constants.ACTION_SLEEP || ActivePlayer.LastAction == Constants.ACTION_NOMOVE_ATTACK 
				   || ActivePlayer.LastAction == Constants.ACTION_PRODUCE || ActivePlayer.LastAction == Constants.ACTION_PRODUCE_ATTACK) 
				{			
					GameUI.ActionPanel.SetActive(false);
					CirclePressed_LastStateActive();		
				} else if(ActivePlayer.ActiveUnit != null){
					GameActions.MoveBackToOriginSelectedUnit();
				} else {
					CirclePressed_LastStateProducePanelOnly();
				}
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_CANCEL);
			}
		}
		
		private void CrossPressed_LastStateAttackPanelActive() // This activates combat
		{
			GameActions.AttackUnit(ActivePlayer.ActiveUnit, ActivePlayer.TargetUnit, 
	                       		  CurrentMap.GetTile(Cursor.WorldPosition),
	                       		  CurrentMap.GetTile(ActivePlayer.ActiveUnit.WorldPosition));
		}
		
		#endregion
		
		#region Circle Pressed Actions
		private void CirclePressed_LastStateActive()
		{
			CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
					
			// Unselect unit and remove tint from tiles
			Utilities.HideActionPanel();
			if(ActivePlayer.ActiveUnit != null) {
				ActivePlayer.ActiveUnit.Unselect ();			
				ActivePlayer.ActiveUnit = null;
			}
			
			Cursor.TintToWhite();
			CurrentMap.UnTintAllTiles();
		}
		
		private void CirclePressed_LastStateActionPanelActive()
		{
			if(ActivePlayer.LastAction == Constants.ACTION_SLEEP || ActivePlayer.LastAction == Constants.ACTION_NOMOVE_ATTACK 
				   || ActivePlayer.LastAction == Constants.ACTION_PRODUCE || ActivePlayer.LastAction == Constants.ACTION_PRODUCE_ATTACK) // Unit on same origin tile
			{
				CirclePressed_LastStateActive();
			}
			else if(ActivePlayer.ActiveUnit != null)
			{
				GameActions.MoveBackToOriginSelectedUnit();
			} 
			else if(ActivePlayer.ActiveUnit == null) // Only building selected
			{
				CirclePressed_LastStateProducePanelOnly();
			}
			SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_CANCEL);
		} 
		
		private void CirclePressed_LastStateAttackPanelActive()
		{
			// Clear targeted tiles and revert to Action Panel selection
			CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			CurrentMap.ResetActiveTiles();

			Cursor.MoveToTileByWorldPosition(ActivePlayer.ActiveUnit.WorldPosition);
			Cursor.TintToBlue();
			
			Utilities.HideAttackPanel();
		}
			
		private void CirclePressed_LastStateProducePanelOnly()
		{
			CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			Utilities.HideActionPanel();
			Cursor.TintToWhite();
		}
		#endregion
		
		#region Utilities
        
		public void Dispose()
		{
			this.Cleanup();
			
			CurrentMap.SpriteList.RemoveAllChildren(true);
			this.RemoveAllChildren(true);
			
			//AssetsManager.Instance.Dispose();
			//SoundManager.Instance.Dispose();
			
			_Instance = null;
			Console.WriteLine("DELETING GAME SCENE");
		}
		
		#endregion
		#endregion
	}
}

