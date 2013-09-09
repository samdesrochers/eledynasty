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
		public TextImage TextImage; // Debug text
		
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
			
			// Create the Players
			Players = new List<Player> ();
			Player player1 = new HumanPlayer ();
			player1.IsActive = true;
			Players.Add (player1);
			
			Player player2 = new HumanPlayer ();
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
			
			// Add Debug help text
			TextImage = new TextImage ("", new Vector2(30,30));
			this.AddChild (TextImage);
			
			UI = new GameUI();
			UI.ActivePlayerIcon.Image = ActivePlayer.Icon;
			
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
				case Constants.GLOBAL_STATE_PAUSE:
					//UpdateGameRunning();
					break;
				case Constants.GLOBAL_STATE_GAMEOVER:
					//UpdateGameRunning();
					break;
			}
		}
		
		#region Update Methods
		private void UpdateGameRunning()
		{
			UpdateCameraPosition ();
			
			if (ActivePlayer is HumanPlayer) 
			{
				if(UI.ActionPanel.IsActive())
				{
					UpdatePanelSelection();
				} else {
					UpdateCursorPosition ();
				}
					
				CheckUserInput ();
			}
		
			UpdateUnits ();			
			UpdateMap ();	
			CheckIsGameOver ();		
			ExecuteTurn ();
			
			if(CheckIfTurnIsOver())
			{
				// Turn is done, switch to next player
				SwitchToNextPlayer();
			}	
		}
		
		private void UpdateGameSwitchingTurn(float dt)
		{
			SwitchTurnTime += dt;			
			UI.AnimateSwitchitngTurn(dt);

			if(SwitchTurnTime >= 2.0f || Input2.GamePad0.Cross.Release) // wait 2 seconds before switching turn
			{
				CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
				UI.SetPlaying();
				SwitchTurnTime = 0.0f;
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
		}
		
		public void UpdateCameraPositionByCursor ()
		{
			Camera2D camera = this.Camera as Camera2D;
			Vector2 pos = Cursor.Position;

			if(pos.X >= 960/2 && pos.X <= CurrentMap.Width * 64 - (960/2) + 64) {
				camera.Center = new Vector2 (Cursor.Position.X - 32, camera.Center.Y);
			} 
			
			if(pos.Y >= 544/2 && pos.Y <= CurrentMap.Height * 64 - (544/2) + 64) {
				camera.Center = new Vector2 (camera.Center.X, Cursor.Position.Y  - 32);
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
		
		private void UpdatePanelSelection()
		{
			if (Input2.GamePad.GetData (0).Up.Release) {
				UI.ActionPanel.FocusNextItemUp();
			} else if(Input2.GamePad.GetData (0).Down.Release) {
				UI.ActionPanel.FocusNextItemDown();
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
		#endregion
		
		#region Game Loop Methods
		private void SwitchToNextPlayer()
		{
			// Finish current player's turn
			ActivePlayer.ResetUnits();
			if(ActivePlayer.ActiveUnit != null)
			{
				ActivePlayer.ActiveUnit.RevertMove();
				ActivePlayer.ActiveUnit.Unselect ();
				ActivePlayer.ActiveUnit = null;
				CurrentMap.UnTintAllTiles();
			}
			ActivePlayer.ActiveUnit = null;
			
			// Switch Player and add a turn to the counter
			ActivePlayerIndex++;		
			ActivePlayer = Players [ ActivePlayerIndex % Players.Count ];
			CurrentTurnCount ++;
			
			// Move Cursor to new player's first unit
			Cursor.MoveToTileByWorldPosition(ActivePlayer.Units[0].WorldPosition);
			UpdateCameraPositionByCursor();
			
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
					return true;
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
			TextImage.Text = ActivePlayer.Name;
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
			else if (this.CurrentGameState == Constants.GAME_STATE_SELECTION_ACTIVE) 
			{				
				// User selects a destination Tile and Presses "X"
				if (Input2.GamePad.GetData (0).Cross.Release) 
				{
					CrossPressed_LastStateSelectionActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release) 
				{
					CirclePressed_LastStateActive();
				}
			} 
			else if(this.CurrentGameState == Constants.GAME_STATE_ACTIONPANEL_ACTIVE)
			{
				// User selects an option in the Action panel Presses "X"
				if (Input2.GamePad.GetData (0).Cross.Release) 
				{
					CrossPressed_LastStateActionPanelActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release) 
				{
					CirclePressed_LastStateActionPanelActive();
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
		
		private void CrossPressed_LastStateIdle ()
		{
			if (Input2.GamePad.GetData (0).Cross.Release) 
			{ 
				Unit selectedUnit = CurrentMap.SelectUnitFromTile (Cursor.WorldPosition);
					
				if (selectedUnit != null)  // Actual Unit was found on tile
				{
					CurrentGameState = Constants.GAME_STATE_SELECTION_ACTIVE;
					ActivePlayer.ActiveUnit = selectedUnit;
					ActivePlayer.ActiveTile = CurrentMap.SelectTileFromPosition (Cursor.WorldPosition);
				}
			}	
		}
		
		private void CrossPressed_LastStateSelectionActive()
		{
			Path path = new Path ();				
			int action = path.GetDestinationAction (Cursor.WorldPosition);
					
			if (action == Constants.ACTION_MOVE) {		
				
				// Prepare the Action Panel with Move and Cancel actions only
				GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL);
				CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
				Utilities.AssignMovePathToActiveUnit(); // Concrete move action
						
			} else if (action == Constants.ACTION_CANCEL) {
				
				CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
					
				// Unselect unit and remove tint from tiles
				ActivePlayer.ActiveUnit.Unselect ();
				ActivePlayer.ActiveUnit = null;
				CurrentMap.UnTintAllTiles ();
			}

			Cursor.TintToWhite();
		}
		
		private void CrossPressed_LastStateActionPanelActive()
		{
			int action = UI.ActionPanel.GetSelectedAction();
			
			if(action == Constants.UI_ELEMENT_ACTION_TYPE_ATTACK)
			{
				
			}
			else if(action == Constants.UI_ELEMENT_ACTION_TYPE_WAIT)
			{
				// Unit is moved concretly, finalize the action and hide panel
				CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
				UI.ActionPanel.SetActive(false);
				Utilities.FinalizeMoveAction();
				
			}
			else if(action == Constants.UI_ELEMENT_ACTION_TYPE_CANCEL)
			{
				// Move is canceled, hide panel and reset unit to origin position
				CurrentGameState = Constants.GAME_STATE_SELECTION_ACTIVE;
				UI.ActionPanel.SetActive(false);
				
				ActivePlayer.ActiveUnit.RevertMove();
				Cursor.MoveToTileByWorldPosition(ActivePlayer.ActiveUnit.WorldPosition);
			}
		}
		
		private void CirclePressed_LastStateActive()
		{
			CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
					
			// Unselect unit and remove tint from tiles
			ActivePlayer.ActiveUnit.Unselect ();
			ActivePlayer.ActiveUnit = null;
			CurrentMap.UnTintAllTiles();
		}
		
		private void CirclePressed_LastStateActionPanelActive()
		{
			CurrentGameState = Constants.GAME_STATE_SELECTION_ACTIVE;
			UI.ActionPanel.SetActive(false);
			
			// Unselect unit and remove tint from tiles
			ActivePlayer.ActiveUnit.RevertMove();
			Cursor.MoveToTileByWorldPosition(ActivePlayer.ActiveUnit.WorldPosition);
		}
		
		#endregion
		
		#region Utilities
        
		~GameScene ()
		{
			//_SoundPlayer.Dispose ();
		}
		#endregion
	}
}

