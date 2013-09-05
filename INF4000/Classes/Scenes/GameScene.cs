using System;
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
		private static GameScene _Instance;

		public static GameScene Instance {
			get {
				if (_Instance == null) {
					_Instance = new GameScene ();
				}
				return _Instance;
			}
		}
		
		public int CurrentState = 0;
		public Map CurrentMap;
		public Player ActivePlayer;
		public Cursor Cursor;
		public int ActivePlayerIndex;
		public List<Player> Players;
		public DebugHelper DebugHelp;
		
		public GameUI UI;
		
		public GameScene ()
		{
			_Instance = this;
			this.CurrentState = Constants.STATE_SELECT_IDLE;
			this.Camera.SetViewFromViewport ();
						
			// Create the Players
			Players = new List<Player> ();
			Player player1 = new HumanPlayer ("SAM");
			player1.IsActive = true;
			Players.Add (player1);
			
			Player player2 = new HumanPlayer ("GINO");
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
			DebugHelp = new DebugHelper ("");
			this.AddChild (DebugHelp);
			
			UI = new GameUI();
			UI.ActivePlayerIcon.Image = ActivePlayer.Icon;
			
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
			UpdateCameraPosition ();
			
			if (ActivePlayer is HumanPlayer) 
			{
				UpdateCursorPosition ();	
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
					
			// Game Loop Instance : Select Active Player,Is Game Over?, Are Units with move still avail?, Move Cursor-Select Uni,t Do Concrete Actions (Move or Attack), End Turn.
		}
		
		#region Game Loop Methods
		private void SwitchToNextPlayer()
		{
			// Finish current player's turn
			ActivePlayer.ResetUnits();
			
			// Switch Player
			ActivePlayerIndex++;		
			ActivePlayer = Players [ ActivePlayerIndex % Players.Count ];
			
			// Move Cursor to new player's first unit
			Cursor.MoveToTileByWorldPosition(ActivePlayer.Units[0].WorldPosition);
			UpdateCameraPositionByCursor();
			
			// Assign New UI Values (image, FocusPoints, place Cursor on first Unit found, etc.)
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
			DebugHelp.Text = ActivePlayer.Name;
		}
		
		private bool CheckIfTurnIsOver()
		{
			if(!ActivePlayer.HasMovableUnits())
				return true;
			
			return false;
		}
		
		public void EndActivePlayerTurn(object sender, TouchEventArgs e)
		{
			UI.Button_EndTurn.Visible = false;
			Console.WriteLine("WHATUP");
		}
		#endregion
		
		#region User Input
		private void CheckUserInput ()
		{
			if (this.CurrentState == Constants.STATE_SELECT_IDLE) // "X" Pressed, No units selected at the moment
			{ 
				CrossPressed_LastStateIdle();
			} 
			else if (this.CurrentState == Constants.STATE_SELECT_ACTIVE) 
			{				
				// User selects a destination Tile and Presses "X"
				if (Input2.GamePad.GetData (0).Cross.Release) 
				{
					CrossPressed_LastStateActive();
				}
				
				// User Presses "O"
				if (Input2.GamePad.GetData (0).Circle.Release) 
				{
					CirclePressed_LastStateActive();
				}
			}
			
			// Right Bumper Pressed
			if (Input2.GamePad.GetData (0).R.Release) 
			{ 
				Utilities.CycleThroughUnits();
			}
		}
		
		private void CrossPressed_LastStateIdle ()
		{
			if (Input2.GamePad.GetData (0).Cross.Release) 
			{ 
				Unit selectedUnit = CurrentMap.SelectUnitFromTile (Cursor.WorldPosition);
					
				if (selectedUnit != null)  // Actual Unit was found on tile
				{
					CurrentState = Constants.STATE_SELECT_ACTIVE;
					ActivePlayer.ActiveUnit = selectedUnit;
					ActivePlayer.ActiveTile = CurrentMap.SelectTileFromPosition (Cursor.WorldPosition);
				}
			}	
		}
		
		private void CrossPressed_LastStateActive()
		{
			Path path = new Path ();				
			int action = path.GetDestinationAction (Cursor.WorldPosition);
					
			if (action == Constants.ACTION_MOVE) {
				
				// Build Unit Path for move action
				path.BuildMoveToSequence (ActivePlayer.ActiveUnit.WorldPosition, Cursor.WorldPosition);
				path.PathCompleted += ActivePlayer.ActiveUnit.Unit_PathCompleted;
				ActivePlayer.ActiveUnit.Path = path;
						
				// Remove unit from previous tile
				ActivePlayer.ActiveTile.CurrentUnit = null;
					
				CurrentState = Constants.STATE_SELECT_IDLE;
						
				// Unselect unit and remove tint from tiles
				ActivePlayer.ActiveUnit.Unselect();
				ActivePlayer.ActiveUnit = null;
				CurrentMap.UnTintAllTiles ();
						
			} else if (action == Constants.ACTION_CANCEL) {
				CurrentState = Constants.STATE_SELECT_IDLE;
						
				// Unselect unit and remove tint from tiles
				ActivePlayer.ActiveUnit.Unselect ();
				ActivePlayer.ActiveUnit = null;
				CurrentMap.UnTintAllTiles ();
			}

			Cursor.TintToWhite();
		}
		
		private void CirclePressed_LastStateActive()
		{
			CurrentState = Constants.STATE_SELECT_IDLE;
					
			// Unselect unit and remove tint from tiles
			ActivePlayer.ActiveUnit.Unselect ();
			ActivePlayer.ActiveUnit = null;
			CurrentMap.UnTintAllTiles();
		}
		
		#endregion
		
		#region Update Methods
		private void UpdateCameraPosition ()
		{	
			Camera2D camera = this.Camera as Camera2D;
			
			// Adjust Camera accoring to Right Analog Stick
			GamePadData data = GamePad.GetData (0);
			camera.Center = new Vector2 (camera.Center.X + 5 * data.AnalogRightX, camera.Center.Y - 5 * data.AnalogRightY);
			
			/*/ *******************************************************(Conflicts with UI)
			// Adjust Camera according to Touch Input 
			foreach (var touchData in Touch.GetData(0)) {
				if (touchData.Status == TouchStatus.Down || touchData.Status == TouchStatus.Move) {
					float pointX = touchData.X * 15;
					float pointY = touchData.Y * 15;									
					camera.Center = new Vector2 (camera.Center.X + pointX, camera.Center.Y - pointY);
				}
			}*/
		}
		
		public void UpdateCameraPositionByCursor ()
		{
			Camera2D camera = this.Camera as Camera2D;
			camera.Center = new Vector2 (Cursor.Position.X, Cursor.Position.Y);
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
		
		#region Utilities
        
		~GameScene ()
		{
			//_SoundPlayer.Dispose ();
		}
		#endregion
	}
}

