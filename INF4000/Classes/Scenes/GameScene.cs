using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Input;

namespace INF4000
{
	public class GameScene : Scene
	{
		private static GameScene _Instance;
		public static GameScene Instance
		{
			get
			{
				if(_Instance == null)
				{
					_Instance = new GameScene();
				}
				return _Instance;
			}
		}
		
		public int CurrentState = 0;
		
		public Map CurrentMap;
		public Player ActivePlayer;
		public int ActivePlayerIndex;
		public List<Player> Players;
		
		public DebugHelper DebugHelp;
		
		public GameScene ()
		{
			_Instance = this;
			this.CurrentState = Constants.STATE_SELECT_IDLE;
			this.Camera.SetViewFromViewport ();
			
			// Create the Players
			Players = new List<Player>();
			Player player1 = new HumanPlayer("SAM");
			player1.IsActive = true;
			Players.Add(player1);
			
			Player player2 = new HumanPlayer("GINO");
			Players.Add(player2);	
			
			// Create the selected Map and its assets
			CurrentMap = new Map(@"/Application/MapFiles/defaultMap.txt");   
			this.AddChild(CurrentMap._SpriteList);
			
			// Select the initial Active Player
			ActivePlayer = SelectActivePlayer();
			
			// Create the Assets manager
			DebugHelp = new DebugHelper("");
			this.AddChild(DebugHelp);
			
			//Now load the sound fx and create a player
			//_Sound = new Sound ("/Application/audio/pongblip.wav");
			//_SoundPlayer = _Sound.CreatePlayer ();
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
		}

		public override void Update (float dt)
		{
			base.Update (dt);    			
			
			UpdateCameraPosition();
			
			if(ActivePlayer is HumanPlayer)
			{
				UpdateCursorPosition();	
				CheckUserInput();
			}
			
			UpdateUnits();
				
			UpdateMap();
			
			CheckIsGameOver();
			
			ExecuteTurn();
					
			// Game Loop Instance : Select Active Player,Is Game Over?, Are Units with move still avail?, Move Cursor-Select Uni,t Do Concrete Actions (Move or Attack), End Turn.
		}
		
		#region Game Loop Methods
		private bool CheckIsGameOver()
		{
			foreach (Player p in this.Players)
			{
				if(p.Units.Count == 0)
					return true;
			}
			return false;
		}
		
		private Player SelectActivePlayer()
		{
			int index = 0;
			foreach (Player p in this.Players)
			{
				if(p.IsActive){
					ActivePlayerIndex = index;
					return p;
				}
				ActivePlayerIndex++;
			}
			return null;
		}
			
		private void ExecuteTurn()
		{			
			///////////////// UNIT TEST
			if(Input2.GamePad.GetData(0).Triangle.Release)
			{			
				ActivePlayer = Players[(ActivePlayerIndex + 1)%Players.Count];
				ActivePlayerIndex++;
				
				DebugHelp.Text = ActivePlayer.Name;				
			}
			///////////////////////////
			
			if(!ActivePlayer.HasMovableUnits())
			{
				//Switch to next player
				ActivePlayer = Players[(ActivePlayerIndex + 1)%Players.Count];
				ActivePlayerIndex++;
			}
		}
		
		private void CheckUserInput()
		{
			if(this.CurrentState == Constants.STATE_SELECT_IDLE) // No units selected at the moment
			{
				if(Input2.GamePad.GetData(0).Cross.Release) // User selects a Tile (with or without Unit) and Presses "X"
				{
					Unit selectedUnit = CurrentMap.SelectUnitFromTile(CurrentMap.Cursor.WorldPosition);
					
					if(selectedUnit != null) // Actual Unit was found on tile
					{
						CurrentState = Constants.STATE_SELECT_ACTIVE;
						ActivePlayer.ActiveUnit = selectedUnit;
						DebugHelp.Text = selectedUnit.Identifier + " of " + ActivePlayer.Name + " is selected";
						// Draw "Radius"
					}
				}
			} 
			else if(this.CurrentState == Constants.STATE_SELECT_ACTIVE)
			{
				// User selects a destination Tile and Presses "X"
				if(Input2.GamePad.GetData(0).Cross.Release) 
				{
					Path path = new Path();
					path.BuildSequenceToDestination(ActivePlayer.ActiveUnit.WorldPosition, CurrentMap.Cursor.WorldPosition);
					path.PathCompleted += ActivePlayer.ActiveUnit.AssignUnitToTile;
					ActivePlayer.ActiveUnit.Path = path;
					
					CurrentState = Constants.STATE_SELECT_IDLE;
					ActivePlayer.UnselectAllUnits();
					DebugHelp.Text = "Unselected all units";
					
				}
				
				// User Presses "O"
				if(Input2.GamePad.GetData(0).Circle.Release) 
				{
					CurrentState = Constants.STATE_SELECT_IDLE;
					ActivePlayer.UnselectAllUnits();
					DebugHelp.Text = "Unselected all units";
				}
			}
		}
		
		#endregion
		
		#region Update Methods
		private void UpdateCameraPosition()
		{	
			Camera2D camera = this.Camera as Camera2D;
			
			// Adjust Camera accoring to Right Analog Stick
			GamePadData data = GamePad.GetData (0);
			camera.Center = new Vector2 (camera.Center.X + 5 * data.AnalogRightX, camera.Center.Y - 5 * data.AnalogRightY);
			
			// Adjust Camera according to Touch Input
			foreach (var touchData in Touch.GetData(0)) 
			{
	            if (touchData.Status == TouchStatus.Down || touchData.Status == TouchStatus.Move) 
				{
	                float pointX = touchData.X * 15;
	                float pointY = touchData.Y * 15;									
					camera.Center = new Vector2 (camera.Center.X + pointX, camera.Center.Y - pointY);
					//LastTouch = new Vector2(pointX, pointY);
	            }
        	}
		}
		
		private void UpdateCameraPositionByCursor()
		{
			Camera2D camera = this.Camera as Camera2D;
			camera.Center = new Vector2 (CurrentMap.Cursor.Position.X, CurrentMap.Cursor.Position.Y);
		}
		
		private void UpdateCursorPosition()
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
			if(Input2.GamePad.GetData(0).Up.Release && Input2.GamePad.GetData(0).Right.Release) // Up + Right
			{
				CurrentMap.Cursor.NavigateUp();
				CurrentMap.Cursor.NavigateRight();
			}
			else if(Input2.GamePad.GetData(0).Up.Release && Input2.GamePad.GetData(0).Right.Release) // Up + Left
			{
				CurrentMap.Cursor.NavigateUp();
				CurrentMap.Cursor.NavigateLeft();
			}
			else if(Input2.GamePad.GetData(0).Down.Release && Input2.GamePad.GetData(0).Right.Release) // Down + Right
			{
				CurrentMap.Cursor.NavigateDown();
				CurrentMap.Cursor.NavigateRight();
			}
			else if(Input2.GamePad.GetData(0).Down.Release && Input2.GamePad.GetData(0).Right.Release) // Down + Left
			{
				CurrentMap.Cursor.NavigateDown();
				CurrentMap.Cursor.NavigateLeft();
			}
			else if(Input2.GamePad.GetData(0).Right.Release)
			{
				CurrentMap.Cursor.NavigateRight();
			}
			else if(Input2.GamePad.GetData(0).Left.Release)
			{
				CurrentMap.Cursor.NavigateLeft();
			}
			else if(Input2.GamePad.GetData(0).Up.Release)
			{
				CurrentMap.Cursor.NavigateUp();
			}
			else if(Input2.GamePad.GetData(0).Down.Release)
			{
				CurrentMap.Cursor.NavigateDown();
			}
			
			CurrentMap.Cursor.Update();
		}
		
		private void UpdateMap()
		{
			CurrentMap.Update();
		}
		
		private void UpdateUnits()
		{
			foreach(Player p in Players)
			{
				foreach(Unit u in p.Units)
				{
					u.Update();
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

