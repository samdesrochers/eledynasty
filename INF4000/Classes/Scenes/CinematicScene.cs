using System;
using System.Threading;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class CinematicScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		// Singleton Class
		private static CinematicScene _Instance;
		public static CinematicScene Instance {
			get {
				if (_Instance == null) {
					_Instance = new CinematicScene ();
				}
				return _Instance;
			}
		}
		
		/*******************************
		 * FIELDS
		 *******************************/
		
		public int State;
		
		private CinematicUI CUI;
		private List<CinematicNode> Cinematics;
		
		public CinematicScene ()
		{
			Console.WriteLine("CREATING CINEMATIC SCENE");
			_Instance = this;
			
			State = Constants.OV_STATE_ENTERING_SCENE;
						
			//this.Camera.SetViewFromViewport ();
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f, new Vector2(1.5f * 960/2 + time, 1.5f*544/2));
			
			Cinematics = new List<CinematicNode>();
			CinematicNode c1 = new CinematicNode("/Application/Assets/Map/overworld.png");		
			Cinematics.Add(c1);
			
			this.AddChild(c1.SpriteTile);
			
			CUI = new CinematicUI();
			CUI.Show();
			
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
			
			switch(State) {
				case Constants.OV_STATE_IDLE:
					UpdateIdle(dt);
					break;
				case Constants.OV_STATE_STARTING_GAME:
					UpdateStartingGame(dt);
					break;
				case Constants.OV_STATE_ENTERING_SCENE:
					UpdateEnteringGame(dt);
					break;
			}		
			
			//ZoomCameraIn(dt);
		}
		
		public void UpdateIdle(float dt)
		{
			Console.WriteLine("{0}", time);
			ZoomCameraIn(dt);
			if(time > 20.0f) {
				State = Constants.OV_STATE_STARTING_GAME;
			}
		}

		public void UpdateStartingGame(float dt) 
		{
			CUI.UpdateGameStarting(dt);
			
			if(CUI.WhiteTween.Alpha >= 1)
			{
				Director.Instance.ReplaceScene( new OverworldScene() );
			}
		}
		
		public void UpdateEnteringGame(float dt) 
		{
			CUI.UpdateSceneEntering(dt);
			if(CUI.WhiteTween.Alpha <= 0)
				State = Constants.OV_STATE_IDLE;
		}
		
		#region Camera Effects
		private float time;
		public void PanCameraLeftToRight(float dt)
		{
			time += 5*dt;
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f, new Vector2(1.5f * 960/2 + time, 1.5f*544/2));
		}
		
		public void ZoomCameraIn(float dt)
		{
			time += 5*dt;
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f - time, new Vector2(1.5f * 960/2, 1.5f*544/2));
		}
		
		#endregion
		
		#region Utility Methods	     
		public void Dispose()
		{
			this.Cleanup();
			this.RemoveAllChildren(true);
			
			foreach(CinematicNode c in Cinematics)
			{
				c.Texture.Dispose();
				c.TexInfo.Dispose();
			}
			
			_Instance = null;
			Console.WriteLine("DELETING CINEMATIC SCENE");
		}
				
		#endregion
	}
}

