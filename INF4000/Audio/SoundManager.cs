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
		
		private static  BgmPlayer SongPlayer;
  		private static Bgm[] Songs;
		
		private Sound CursorMoved;
		private Sound CursorSelect;
		private Sound CursorCancel;
		private Sound TurnStart;
		private Sound UnitMarch;
		private Sound Combat;
		
		public SoundManager ()
		{
			Loaded = false;
		}
		
		public void LoadSounds()
		{
			if(!Loaded){
				try
				{
					Songs = new Bgm[4];
					Songs[1] = new Bgm("/Application/Audio/kenji_1.mp3");
					Songs[0] = new Bgm("/Application/Audio/kenji_2.mp3");
					Songs[2] = new Bgm("/Application/Audio/enemy_1.mp3");
					Songs[3] = new Bgm("/Application/Audio/enemy_2.mp3");				
					
					CursorMoved = new Sound("/Application/Audio/Sound/cursor_move.wav");
					CursorSelect = new Sound("/Application/Audio/Sound/selection_action.wav");
					CursorCancel = new Sound("/Application/Audio/Sound/cursor_cancel.wav");
					TurnStart = new Sound("/Application/Audio/Sound/coin_sound.wav");
					UnitMarch = new Sound("/Application/Audio/Sound/unit_march.wav");
					Combat = new Sound("/Application/Audio/Sound/combat.wav");
					
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
				}
				
				if(SoundEnabled){
					SoundPlayer = soundToPlay.CreatePlayer();
					SoundPlayer.Play();
				}
				
			} catch {}
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

