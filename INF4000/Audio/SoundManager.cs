using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;

namespace INF4000
{
	public class SoundManager
	{
		private static SoundManager _Instance;

		public static SoundManager Instance {
			get {
				if (_Instance == null) {
					_Instance = new SoundManager ();
				}
				return _Instance;
			}
		}
		
		public bool Loaded;
		public bool SoundEnabled;
		
		public SoundPlayer SoundPlayer;
		
		public BgmPlayer SongPlayer;
  		private Bgm[] Songs;
		
		private Sound CursorMoved;
		private Sound CursorSelect;
		private Sound CursorCancel;
		private Sound TurnStart;
		private Sound UnitMarch;
		private Sound Combat;
		private Sound Capture;
		private Sound Heal;
		
		public SoundManager ()
		{
			Loaded = false;
		}
		
		public void LoadSounds()
		{
			if(!Loaded){
				try
				{
					Songs = new Bgm[9];
					Songs[1] = new Bgm("/Application/Audio/kenji_1.mp3");
					Songs[0] = new Bgm("/Application/Audio/kenji_1.mp3");
					Songs[2] = new Bgm("/Application/Audio/enemy_2.mp3");
					Songs[3] = new Bgm("/Application/Audio/enemy_2.mp3");
					Songs[4] = new Bgm("/Application/Audio/intro_map1.mp3");
					Songs[5] = new Bgm("/Application/Audio/overworld_1.mp3");
					Songs[6] = new Bgm("/Application/Audio/cine_intro.mp3");
					Songs[7] = new Bgm("/Application/Audio/menu.mp3");
					Songs[8] = new Bgm("/Application/Audio/kenji_victory.mp3");
					
					CursorMoved = new Sound("/Application/Audio/Sound/cursor_move.wav");
					CursorSelect = new Sound("/Application/Audio/Sound/selection_action.wav");
					CursorCancel = new Sound("/Application/Audio/Sound/cursor_cancel.wav");
					TurnStart = new Sound("/Application/Audio/Sound/coin_sound.wav");
					UnitMarch = new Sound("/Application/Audio/Sound/unit_march.wav");
					Combat = new Sound("/Application/Audio/Sound/combat.wav");
					Capture = new Sound("/Application/Audio/Sound/capture.wav");
					Heal = new Sound("/Application/Audio/Sound/heal.wav");
					
					SoundEnabled = true;
					
					Loaded = true;
				} 
				catch (Exception e)
				{
					Console.WriteLine("Error creating a sound at Sound Manager - " + e.Message);
				}
			}
		}
		
		public void PlaySound(int sound)
		{
			try 
			{	
				Sound soundToPlay = null;
				switch(sound)
				{
				case Constants.SOUND_CURSOR_MOVE:
					soundToPlay = CursorMoved;
					break;
				case Constants.SOUND_CURSOR_SELECT:
					soundToPlay = CursorSelect;
					break;
				case Constants.SOUND_CURSOR_CANCEL:
					soundToPlay = CursorCancel;
					break;
				case Constants.SOUND_TURN_START:
					soundToPlay = TurnStart;
					break;
				case Constants.SOUND_UNIT_MARCH:
					soundToPlay = UnitMarch;
					break;
				case Constants.SOUND_COMBAT:
					soundToPlay = Combat;
					break;
				case Constants.SOUND_CAPTURE:
					soundToPlay = Capture;
					break;
				case Constants.SOUND_HEAL:
					soundToPlay = Heal;
					break;
				}
				
				if(SoundEnabled){
					SoundPlayer = soundToPlay.CreatePlayer();
					SoundPlayer.Play();
				}
				
			} catch {}
		}
		
		public void PlayIntroMapSong()
		{
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[4].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
			}
		}
		
		public void PlayOverworldSong()
		{
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[5].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
			}
		}
		
		public void PlayCinematicSong()
		{
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[6].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
				SongPlayer.Volume = 0;
			}
		}
		
		public void PlayMenuSong()
		{
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[7].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
			}
		}
		
		public void PlayVictorySong()
		{
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[8].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
			}
		}
		
		public void PlayKenjiGameSong()
		{
			Random ran = new Random();
			int pick = ran.Next(0,10);
			pick = (pick > 3) ? 0 : 1;
			
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[pick].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
			}
		}
		
		public void PlayEnemyGameSong()
		{
			Random ran = new Random();
			int pick = ran.Next(0,10);
			pick = (pick > 5) ? 2 : 3;
			
			if(SongPlayer != null) SongPlayer.Dispose();
			if(SoundEnabled){
				SongPlayer = Songs[pick].CreatePlayer();
	   			SongPlayer.Play();
				SongPlayer.Loop = true;
			}
		}
		
		public void FadeOut(float dt)
		{
			if(SongPlayer != null) SongPlayer.Volume -= dt;
		}
		
		public void Dispose()
		{
			try{
			CursorMoved.Dispose();
			CursorCancel.Dispose();
			CursorSelect.Dispose();
			Combat.Dispose();
			TurnStart.Dispose();
			UnitMarch.Dispose();
			if(SoundPlayer != null) SoundPlayer.Dispose();
			
			Songs[0].Dispose();
			Songs[1].Dispose();
			Songs[2].Dispose();
			Songs[3].Dispose();
			
			if(SongPlayer != null) SongPlayer.Dispose();
			} catch {}
			_Instance = null;
		}
	}
}

