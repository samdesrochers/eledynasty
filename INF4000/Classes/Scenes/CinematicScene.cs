using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

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
		public CinematicUI CUI;
		
		private CinematicDialogManager DialogManager;
		
		private CinematicNode CurrentCinematic;
		private int CinematicIndex;
		
		private List<CinematicNode> Cinematics;
		private const float ImageRatio = 1.3f;
		
		private static BgmPlayer SongPlayer;
		private static Bgm Song;
		
		public CinematicScene ()
		{
			Console.WriteLine("CREATING CINEMATIC SCENE");
			_Instance = this;
			
			State = Constants.GN_STATE_ENTERING_SCENE;
						
			//this.Camera.SetViewFromViewport ();
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f, new Vector2(ImageRatio * 960/2, ImageRatio*544/2));
			
			CUI = new CinematicUI();
			
			DialogManager = new CinematicDialogManager();
			
			DialogManager.DialogSequences.Enqueue("This is a text yo, the village is all chill and nice, living with prosperity and shnizell");
			DialogManager.DialogSequences.Enqueue("But then the king dies he was so shit and ruled the kingdom with an iron fist while being just to its people");
			DialogManager.DialogSequences.Enqueue("Its now up to his daughter the whore to take his place and its no easy task mind you nigga");
			DialogManager.DialogSequences.Enqueue("The korama guys take this chance of finally getting back at Elisia and strom the shit out of the litte country that's where our hero Kenji enter the scene");
			
			Cinematics = new List<CinematicNode>();
			CinematicNode c1 = new CinematicNode("/Application/Assets/Cinematics/cine_elisia.png", Constants.CINE_TRAN_TO_CINE, Constants.CINE_EFFECT_PAN);
			CinematicNode c2 = new CinematicNode("/Application/Assets/Cinematics/cine_king.png", Constants.CINE_TRAN_TO_CINE, Constants.CINE_EFFECT_ZOOM);
			CinematicNode c3 = new CinematicNode("/Application/Assets/Cinematics/cine_throne.png", Constants.CINE_TRAN_TO_CINE, Constants.CINE_EFFECT_ZOOM);
			CinematicNode c4 = new CinematicNode("/Application/Assets/Cinematics/cine_fire.png", Constants.CINE_TRAN_TO_GAME, Constants.CINE_EFFECT_PAN);
			Cinematics.Add(c1);
			Cinematics.Add(c2);
			Cinematics.Add(c3);
			Cinematics.Add(c4);
			CinematicIndex = 0;
			
			SetCurrentCinematic();
			IsFadingOut = true;		
			
			try {
				Song = new Bgm("/Application/Audio/cine_intro.mp3");
				
				PlaySong();
			} catch (Exception e) { Console.WriteLine("Error creating Audio in Cinematic Scene. {0}", e.Message); }
			
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
				case Constants.GN_STATE_IDLE:
					UpdateRunning(dt);
					break;
				case Constants.GN_STATE_STARTING_GAME:
					UpdateStartingGame(dt);
					break;
				case Constants.GN_STATE_ENTERING_SCENE:
					UpdateEnteringFromMenu(dt);
					break;
				case Constants.GN_STATE_SWITCHING_LEVEL:
					UpdateSwitchingCinematic(dt);
					break;
			}		
			
			//ZoomCameraIn(dt);
		}
		
		private void SetCurrentCinematic()
		{
			CurrentCinematic = Cinematics[CinematicIndex];
			this.RemoveAllChildren(true);
			this.AddChild(CurrentCinematic.SpriteTile);
			
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f, new Vector2(ImageRatio * 960/2, ImageRatio*544/2));
			
			CUI.Label_Text.Alpha = 0;
			CUI.Label_Text.Text = "";
			DialogManager.NextSequence();
		}
		
		public void UpdateRunning(float dt)
		{		
			if(CurrentCinematic.Effect == Constants.CINE_EFFECT_PAN) {
				PanCameraLeftToRight(dt);	
			} else if (CurrentCinematic.Effect == Constants.CINE_EFFECT_ZOOM){
				ZoomCameraIn(dt);
			}
			
			DialogManager.Update(dt);
			
			if(EllapsedTime > 1.0f) {
				if(CurrentCinematic.Transition == Constants.CINE_TRAN_TO_CINE) {
					EllapsedTime = 0;
					AnimationTime = 0;
					State = Constants.GN_STATE_SWITCHING_LEVEL;
				} else {
					State = Constants.GN_STATE_STARTING_GAME;
				}			
			}
			EllapsedTime += dt;
			
		}

		public void UpdateStartingGame(float dt) 
		{
			CUI.UpdateGameStarting(dt);
			SongFadeOut(dt);
			
			if(CUI.WhiteTween.Alpha >= 1)
			{
				Dispose();
				Director.Instance.ReplaceScene( new OverworldScene() );
			}
		}
		
		public void UpdateEnteringFromMenu(float dt) 
		{
			CUI.UpdateSceneEntering(dt);
			SongFadeIn(dt);
			
			if(CUI.WhiteTween.Alpha <= 0)
				State = Constants.GN_STATE_IDLE;
		}
		
		public void UpdateSwitchingCinematic(float dt)
		{
			if(CUI.BlackTween.Alpha > 1 && IsFadingOut) {
				CinematicIndex ++;
				SetCurrentCinematic();
				IsFadingOut = false;
			}
			
			if(CUI.BlackTween.Alpha < 1 && IsFadingOut) {
				CUI.UpdateSwitchFadeOut(dt);
			} else if (CUI.BlackTween.Alpha >= 0 && !IsFadingOut) {
				CUI.UpdateSwitchFadeIn(dt);
			}
			
			if(CUI.BlackTween.Alpha <= 0 && !IsFadingOut) {
				State = Constants.GN_STATE_IDLE;
				IsFadingOut = true;
				CUI.BlackTween.Alpha = 0;
			}
		}
		
		#region Camera Effects
		
		private float EllapsedTime;
		private float AnimationTime;
		private bool IsFadingOut;
		
		public void PanCameraLeftToRight(float dt)
		{
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f, new Vector2(ImageRatio * 960/2 + AnimationTime, ImageRatio * 544/2));
			AnimationTime += 2*dt;
		}
		
		public void ZoomCameraIn(float dt)
		{		
			Camera2D camera = this.Camera as Camera2D;
			camera.SetViewFromWidthAndCenter(1200.0f - AnimationTime, new Vector2(ImageRatio * 960/2, ImageRatio * 544/2));
			AnimationTime += 8*dt;
		}
		
		#endregion
		
		#region Utility Methods	     
		public void Dispose()
		{
			this.Cleanup();
			this.RemoveAllChildren(true);
			
			if(SongPlayer != null) {
				SongPlayer.Stop();
				SongPlayer.Dispose();
			}
			if(Song != null) Song.Dispose();
			
			_Instance = null;
			Console.WriteLine("DELETING CINEMATIC SCENE");
		}
		
		public void PlaySong()
		{
			if(SongPlayer != null) SongPlayer.Dispose();
			SongPlayer = Song.CreatePlayer();
   			SongPlayer.Play();
			SongPlayer.Loop = true;	
			SongPlayer.Volume = 0;
		}
		
		public void SongFadeOut(float dt)
		{
			if(SongPlayer != null) SongPlayer.Volume -= 1.2f*dt;
			if(SongPlayer != null && SongPlayer.Volume < 0) SongPlayer.Volume = 0;
		}
		
		public void SongFadeIn(float dt)
		{
			if(SongPlayer != null) SongPlayer.Volume += 1.5f*dt;
			if(SongPlayer != null && SongPlayer.Volume > 1) SongPlayer.Volume = 1;
		}
		
				
		#endregion
	}
}

