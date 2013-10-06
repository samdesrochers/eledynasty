using System;
using System.Threading;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
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
		public GameUI UI;
		
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
		public int CurrentTurnCount = 1;
		
		public GameScene ()
		{
			_Instance = this;
			this.CurrentGlobalState = Constants.GLOBAL_STATE_SWITCHING_TURN;
			this.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			
			this.Camera.SetViewFromViewport ();

			// Load the Assets
			AssetsManager.Instance.LoadAssets();
			
			// Load the Sounds
			SoundManager.Instance.LoadSounds();
			
			// Create the Players
			Players = new List<Player> ();
			Player player1 = new HumanPlayer ();
			player1.IsActive = true;
			Players.Add (player1);
			
			Player player2 = new AIPlayer ();
			player2.Icon = AssetsManager.Instance.Image_Gohzu_UI_Turn;
			Players.Add (player2);				
			
			// Create the selected Map and its assets
			CurrentMap = new Map (@"/Application/MapFiles/defaultMap.txt");   
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
			
			UI = new GameUI();
			UI.PlayerPanel.SetCurrentPlayerData(ActivePlayer.Icon, ActivePlayer.Gold.ToString(), "");	
			
			ActivePlayer.IsStartingTurn = true;
			SwitchTurnTime = 0.0f;	
			CurrentTurnCount = 1;
									
			//Now load the sound fx and create a player
			//_Sound = new Sound ("/Application/audio/pongblip.wav");
			//_SoundPlayer = _Sound.CreatePlayer ();
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
				case Constants.GLOBAL_STATE_PLAYING_TURN:
					UpdateGameRunning();
					break;
				case Constants.GLOBAL_STATE_SWITCHING_TURN:
					UpdateGameSwitchingTurn(dt);
					break;
				case Constants.GLOBAL_STATE_BATTLE_ANIMATION:
					UpdateBattleAnimation(dt);
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
		private void UpdateGameRunning()
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
				UpdateGameRunningHuman();
			} else {
				UpdateGameRunningAI();
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
		
		private void UpdateGameRunningHuman()
		{
			if(CurrentGameState == Constants.GAME_STATE_ACTIONPANEL_ACTIVE)
				UpdateActionPanelSelection();
			else if(CurrentGameState == Constants.GAME_STATE_ATTACKPANEL_ACTIVE)
				UpdateAttackPanelSelection();
			else 
				UpdateCursorPosition ();
				
			CheckUserInput ();
			
			if(CurrentGameState == Constants.GAME_STATE_SELECTION_INACTIVE || CurrentGameState == Constants.GAME_STATE_ATTACKPANEL_ACTIVE){
				Utilities.ShowStatsPanels();
				UpdateStatsPanel();
			} else {
				Utilities.HideStatsPanels();
			}
		}
		
		private void UpdateGameRunningAI()
		{
			ActivePlayer.Update();
		}
		
		private bool turnSwitchFirstPass = true;
		private void UpdateGameSwitchingTurn(float dt)
		{
			if(turnSwitchFirstPass)
			{
				turnSwitchFirstPass = false;
				SoundManager.Instance.PlaySound(Constants.SOUND_TURN_START);
			}
			
			SwitchTurnTime += dt;			
			UI.AnimateSwitchitngTurn(dt);

			if(SwitchTurnTime >= 2.0f || Input2.GamePad0.Cross.Release) // wait 2 seconds before switching turn
			{
				CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
				UI.SetPlaying();
				SwitchTurnTime = 0.0f;
			}
		}
		
		private void UpdateBattleAnimation(float dt)
		{
			BattleViewer.Update(dt);
		}
		
		private void UpdateGameOver(float dt)
		{
			SwitchTurnTime += dt;			
			UI.AnimateGameOver(dt);

			if(SwitchTurnTime >= 5.0f || Input2.GamePad0.Cross.Release) // wait 2 seconds before switching turn
			{
				Director.Instance.ReplaceScene(new MenuScene());
			}
		}
		
		private void UpdateCameraPosition ()
		{	
//			Camera2D camera = this.Camera as Camera2D;
//			
//			// Adjust Camera accoring to Right Analog Stick
//			GamePadData data = GamePad.GetData (0);
//			Vector2 pos = camera.Center;
//			
//			if(pos.X >= 960/2 && pos.X <= CurrentMap.Width * 64 - (960/2) + 64) {
//				camera.Center = new Vector2 (camera.Center.X + 5 * data.AnalogRightX, camera.Center.Y - 5 * data.AnalogRightY);
//			} else {
//				camera.Center = new Vector2 (camera.Center.X, camera.Center.Y - 5 * data.AnalogRightY);
//			}		
			Utilities.AdjustStatsPanelLocation();
		}
		
		public void UpdateCameraPositionByCursor ()
		{
			Camera2D camera = this.Camera as Camera2D;
			Vector2 pos = Cursor.Position;

			if(pos.X >= 960/2 && pos.X <= CurrentMap.Width * 64 - (960/2) + 64) {
				camera.Center = new Vector2 (Cursor.Position.X - 32, camera.Center.Y);
			} 
			
			if(pos.Y >= 544/2 && pos.Y <= CurrentMap.Height * 64 - (544/2) + 64) {
				camera.Center = new Vector2 (camera.Center.X, Cursor.Position.Y  - 16);
			} else if(pos.Y < 544/2) {
				camera.Center = new Vector2 (camera.Center.X, 544/2);
			}
		}
		
		private void UpdateCursorPosition ()
		{
			// "On" Section
//			if(Input2.GamePad.GetData(0).Right.On)
//			{
//				CurrentMap.Cursor.NavigateRight();
//				UpdateCameraPositionByCursor();
//			}
//			else if(Input2.GamePad.GetData(0).Left.On)
//			{
//				CurrentMap.Cursor.NavigateLeft();
//				UpdateCameraPositionByCursor();
//			}
//			else if(Input2.GamePad.GetData(0).Up.On)
//			{
//				CurrentMap.Cursor.NavigateUp();
//				UpdateCameraPositionByCursor();
//			}
//			else if(Input2.GamePad.GetData(0).Down.On)
//			{
//				CurrentMap.Cursor.NavigateDown();
//				UpdateCameraPositionByCursor();
//			}
			
			// "Release" Section		
			if (Input2.GamePad.GetData (0).Up.Release && Input2.GamePad.GetData (0).Right.Release) { // Up + Right
				Cursor.NavigateUp ();
				Cursor.NavigateRight ();
			} else if (Input2.GamePad.GetData (0).Up.Release && Input2.GamePad.GetData (0).Right.Release) { // Up + Left
				Cursor.NavigateUp ();
				Cursor.NavigateLeft ();
			} else if (Input2.GamePad.GetData (0).Down.Release && Input2.GamePad.GetData (0).Right.Release) { // Down + Right
				Cursor.NavigateDown ();
				Cursor.NavigateRight ();
			} else if (Input2.GamePad.GetData (0).Down.Release && Input2.GamePad.GetData (0).Right.Release) { // Down + Left
				Cursor.NavigateDown ();
				Cursor.NavigateLeft ();
			} else if (Input2.GamePad.GetData (0).Right.Release) {
				Cursor.NavigateRight ();
			} else if (Input2.GamePad.GetData (0).Left.Release) {
				Cursor.NavigateLeft ();
			} else if (Input2.GamePad.GetData (0).Up.Release) {
				Cursor.NavigateUp ();
			} else if (Input2.GamePad.GetData (0).Down.Release) {
				Cursor.NavigateDown ();
			}
			
			Cursor.Update ();	
			this.UpdateCameraPositionByCursor();
		}
		
		private void UpdateActionPanelSelection()
		{
			if (Input2.GamePad.GetData (0).Up.Release) {
				UI.ActionPanel.FocusNextItemUp();
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
			} else if(Input2.GamePad.GetData (0).Down.Release) {
				UI.ActionPanel.FocusNextItemDown();
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
				UI.TileStatsPanel.SetElements(t.Defense, 0, 0, 0, t.Label, t.TerrainType, null);
				UI.TileStatsPanel.SetConfiguration(Constants.UI_ELEMENT_CONFIG_STATS_TERRAIN);
				
				if(t.CurrentBuilding != null)
				{
					UI.TileStatsPanel.SetElements(t.CurrentBuilding.Defense, 0, 0, t.CurrentBuilding.GoldPerTurn, t.CurrentBuilding.Label, t.CurrentBuilding.Type, t.CurrentBuilding.OwnerName);
					UI.TileStatsPanel.SetConfiguration(Constants.UI_ELEMENT_CONFIG_STATS_BUILDING);
				} 
				
				if(t.CurrentUnit != null)
				{
					UI.UnitStatsPanel.SetElements(t.CurrentUnit.Armor, t.CurrentUnit.AttackDamage, t.CurrentUnit.LifePoints, 0, t.CurrentUnit.Label, t.CurrentUnit.Type, t.CurrentUnit.OwnerName);
					UI.UnitStatsPanel.SetConfiguration(Constants.UI_ELEMENT_CONFIG_STATS_UNIT);
					UI.UnitStatsPanel.IsActive = true;
				} else {
					UI.UnitStatsPanel.IsActive = false;
				}
			}
		}
		#endregion
		
		#region Game Loop Methods
		private void InitPlayerTurn()
		{
			turnSwitchFirstPass = true;
			
			// Move Cursor to new player's first unit
			Cursor.MoveToFirstUnit();
			UpdateCameraPositionByCursor();
			
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
			UI.ActionPanel.SetActive(false);		
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
			if(!ActivePlayer.HasMovableUnits() && CurrentGameState == Constants.GAME_STATE_SELECTION_INACTIVE)
				return true;
			
			return false;
		}
		
		public void EndActivePlayerTurn(object sender, TouchEventArgs e)
		{
			if(CurrentGlobalState == Constants.GLOBAL_STATE_PLAYING_TURN)
			{
				UI.Button_EndTurn.Visible = false;
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
				if (Input2.GamePad.GetData (0).Cross.Release && UI.ActionPanel.IsActive()) 
				{
					CrossPressed_LastStateActionPanelActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release && UI.ActionPanel.IsActive()) 
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
				Utilities.CycleThroughUnits();
			}
			
			// Sqaure Pressed
			if (Input2.GamePad.GetData (0).Square.Release) 
			{ 
				//UI.ActionSelectionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_MOVE_CANCEL_ATTACK);
			}
			
			// Triangle Pressed
			if (Input2.GamePad.GetData (0).Triangle.Release) 
			{ 
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
					UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_CANCEL_PRODUCE);
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
			int UIAction = UI.ActionPanel.GetSelectedAction();
			
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
					UI.ActionPanel.SetActive(false);
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
			// State attaque va survenir
			BattleViewer.PrepareBattleAnimation(ActivePlayer.ActiveUnit, ActivePlayer.TargetUnit, 
			                       		  CurrentMap.GetTile(Cursor.WorldPosition),
			                       		  CurrentMap.GetTile(ActivePlayer.ActiveUnit.WorldPosition));
			
			CurrentGlobalState = Constants.GLOBAL_STATE_BATTLE_ANIMATION;
			SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_SELECT);
		}
		
		#endregion
		
		#region Circle Pressed Actions
		private void CirclePressed_LastStateActive()
		{
			CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
					
			// Unselect unit and remove tint from tiles
			Utilities.HideActionPanel();
			ActivePlayer.ActiveUnit.Unselect ();			
			ActivePlayer.ActiveUnit = null;
			
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
        
		~GameScene ()
		{
			//_SoundPlayer.Dispose ();
		}
		#endregion
		#endregion
	}
}

