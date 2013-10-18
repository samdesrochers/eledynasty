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
	public class OverworldScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		// Singleton Class
		private static OverworldScene _Instance;
		public static OverworldScene Instance {
			get {
				if (_Instance == null) {
					_Instance = new OverworldScene ();
				}
				return _Instance;
			}
		}
		
		/*******************************
		 * FIELDS
		 *******************************/
		
		public int State;
		
		public OverworldMap OVMap;
		public OverworldUI OVUI;
		public Avatar Avatar;
		
		public List<Level> Levels;
		public Level SelectedLevel;
		public int LevelIndex;
		
		public OverworldScene ()
		{
			Console.WriteLine("CREATING OVERWORLD SCENE");
			_Instance = this;
						
			State = Constants.OV_STATE_IDLE;
			this.Camera.SetViewFromViewport ();

			// Load the Assets
			AssetsManager.Instance.LoadAssets();
			
			// Load the Sounds
			SoundManager.Instance.LoadSounds();
						
			OVMap = new OverworldMap();				
			Avatar = new Avatar();
			
			Levels = new List<Level>();
			Level level1 = new Level("Village of Haruma", "Kenji's home village is under attack by local Rebels. Even with litte troops and training, Kenji rushes in to aid his people.", "Gohzu Amari", 2, AssetsManager.Instance.Image_Dialog_Gohzu_1);
			level1.GameLevelPath = @"/Application/MapFiles/map1.txt";
			level1.Position = new Vector2(670,315);
			Levels.Add(level1);
			
			Level level2 = new Level("Korama's Frontier", "War wages south, near Korama's frontier. Kenji must travel here with his newly acquired army and push through the enemy troops massing there. Onward on finding out what happened to his father...", "Akari Kotsu", 1, AssetsManager.Instance.Image_Dialog_Gohzu_1);
			level2.GameLevelPath = @"/Application/MapFiles/map2.txt";
			level2.Position = new Vector2(300,315);
			Levels.Add(level2);
			
			LevelIndex = 0;
			SelectedLevel = Levels[ LevelIndex ];
			this.AddChild(OVMap.SpriteTile);
			this.AddChild(Avatar.SpriteTile);		
			
			OVUI = new OverworldUI();
			OVUI.SetLevelInfo(Levels[0]);
			
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
		}
		
		public void Reset()
		{
			OVUI.SceneOverlay.Alpha = 0;
			State = Constants.OV_STATE_IDLE;
			OVUI.Show();
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
				case Constants.OV_STATE_EXITING_GAME:
					UpdateExitingGame(dt);
					break;
			}		
		}
		
		public void UpdateIdle(float dt)
		{
			Avatar.Update(dt);
			
			if (Input2.GamePad.GetData (0).Cross.Release || Input2.GamePad.GetData (0).Start.Release ) {
				State = Constants.OV_STATE_STARTING_GAME;
			} else if(Input2.GamePad.GetData (0).Circle.Release ) {
				State = Constants.OV_STATE_EXITING_GAME;
			}
			
			if( Input2.GamePad.GetData (0).Left.Release ) {
				LevelIndex = (int)Sce.PlayStation.Core.FMath.Abs((LevelIndex - 1) % Levels.Count);
				SelectedLevel = Levels[LevelIndex];
				OVUI.SetLevelInfo(SelectedLevel);
			} else if ( Input2.GamePad.GetData (0).Right.Release ) {
				LevelIndex = (LevelIndex + 1) % Levels.Count;
				SelectedLevel = Levels[LevelIndex];
				OVUI.SetLevelInfo(SelectedLevel);
			}
		}

		public void UpdateStartingGame(float dt) 
		{
			OVUI.UpdateGameStarting(dt);
			if(OVUI.SceneOverlay.Alpha >= 1)
			{
				Director.Instance.ReplaceScene( new GameScene() );
			}
		}
		
		public void UpdateExitingGame(float dt) 
		{
			//Director.Instance.ReplaceScene( new MenuScene() );
		}
		
		#region Update Methods	     
		public void Dispose()
		{
			this.Cleanup();
			this.RemoveAllChildren(true);
			
			AssetsManager.Instance.Dispose();
			SoundManager.Instance.Dispose();
			
			_Instance = null;
			Console.WriteLine("DELETING OVERWORLD SCENE");
		}
		
		#endregion
	}
}

