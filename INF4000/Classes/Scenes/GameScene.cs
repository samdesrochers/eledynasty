using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Input;

namespace INF4000
{
	public class GameScene : Scene
	{
		private Map CurrentMap;
		
		//private SoundPlayer _SoundPlayer;
		//private Sound _Sound;
		
		private Vector2 LastTouch;
              
		public GameScene ()
		{
			this.Camera.SetViewFromViewport ();
			
			CurrentMap = new Map();   
			this.AddChild(CurrentMap._SpriteList);
			
			//Now load the sound fx and create a player
			//_Sound = new Sound ("/Application/audio/pongblip.wav");
			//_SoundPlayer = _Sound.CreatePlayer ();
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
		}
        
		public override void Update (float dt)
		{
			base.Update (dt);    			
			
			UpdateCameraPosition();
			UpdateCursorPosition();
			
			UpdateMap();
		}
		
		private void UpdateCameraPosition()
		{	
			Camera2D camera = this.Camera as Camera2D;
			
			// Adjust Camera accoring to Right Analog Stick
			GamePadData data = GamePad.GetData (0);
			camera.Center = new Vector2 (camera.Center.X + 5 * data.AnalogRightX, camera.Center.Y - 5 * data.AnalogRightY);
			
			// Adjust Camera according to Touch Input
			foreach (var touchData in Touch.GetData(0)) {
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
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Up.Release && Input2.GamePad.GetData(0).Right.Release) // Up + Left
			{
				CurrentMap.Cursor.NavigateUp();
				CurrentMap.Cursor.NavigateLeft();
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Down.Release && Input2.GamePad.GetData(0).Right.Release) // Down + Right
			{
				CurrentMap.Cursor.NavigateDown();
				CurrentMap.Cursor.NavigateRight();
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Down.Release && Input2.GamePad.GetData(0).Right.Release) // Down + Left
			{
				CurrentMap.Cursor.NavigateDown();
				CurrentMap.Cursor.NavigateLeft();
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Right.Release)
			{
				CurrentMap.Cursor.NavigateRight();
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Left.Release)
			{
				CurrentMap.Cursor.NavigateLeft();
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Up.Release)
			{
				CurrentMap.Cursor.NavigateUp();
				UpdateCameraPositionByCursor();
			}
			else if(Input2.GamePad.GetData(0).Down.Release)
			{
				CurrentMap.Cursor.NavigateDown();
				UpdateCameraPositionByCursor();
			}
			
			CurrentMap.Cursor.Update();
		}
		
		private void UpdateMap()
		{
			CurrentMap.Update();
		}
        
		~GameScene ()
		{
			//_SoundPlayer.Dispose ();
		}
	}
}

