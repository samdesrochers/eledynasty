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
			
			DialogManager.DialogSequences.Enqueue("Once upon a time, the People of Elisia lived in harmony with a relative peace under the guidance of King Jakal Elisiu, the people's Defender.");
			DialogManager.DialogSequences.Enqueue("But one day, the King succombed to a strange illness, leaving his young daughter Himiko as Queen of Elisia.");
			DialogManager.DialogSequences.Enqueue("The Koramaian Empire, found to the south of Elisia, seeing their greatest opponent pass away, rallied their banners and marched straight into Elisia, still mouring their deceased King.");
			DialogManager.DialogSequences.Enqueue("With the Elisian army dispatched to defend the Kingdom, many small Elisian villages fell to opportunist rebels. You are thus called to help by your old friend, the Queen of Elisia, to help restore ordre in the Kingdom of Elisia.");
			
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
				SoundManager.Instance.PlayCinematicSong();
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
			
			if(EllapsedTime > 9.0f || Input2.GamePad0.Square.Press) {
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
				Director.Instance.ReplaceScene( new LoadingScene() );
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
			
			_Instance = null;
			Console.WriteLine("DELETING CINEMATIC SCENE");
		}
		
		public void SongFadeOut(float dt)
		{
			if(SoundManager.Instance.SongPlayer != null) SoundManager.Instance.SongPlayer.Volume -= 1.2f*dt;
			if(SoundManager.Instance.SongPlayer != null && SoundManager.Instance.SongPlayer.Volume < 0) SoundManager.Instance.SongPlayer.Volume = 0;
		}
		
		public void SongFadeIn(float dt)
		{
			if(SoundManager.Instance.SongPlayer != null) SoundManager.Instance.SongPlayer.Volume += 1.3f*dt;
			if(SoundManager.Instance.SongPlayer != null && SoundManager.Instance.SongPlayer.Volume > 1) SoundManager.Instance.SongPlayer.Volume = 1;
		}
		
				
		#endregion
	}
}

