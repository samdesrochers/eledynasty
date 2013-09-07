using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace INF4000
{
	public class TitleScene : Scene
	{
		private TextureInfo _TexInfo;
		private Texture2D _Texture;
		private Bgm bgm;
		private BgmPlayer bgmPlayer;
		
		public TitleScene ()
		{
			// Load title screen image and prepare camera
			this.Camera.SetViewFromViewport ();
			_Texture = new Texture2D ("Application/Assets/Title/title.png", false);
			_TexInfo = new TextureInfo (_Texture);			
			SpriteUV titleScreen = new SpriteUV (_TexInfo);
			
			Vector2 titleSize = _TexInfo.TextureSizef;			
			titleScreen.Scale = titleSize;
			
			titleScreen.Pivot = new Vector2 (0.5f, 0.5f);
			titleScreen.Position = new Vector2 (Director.Instance.GL.Context.GetViewport ().Width / 2, Director.Instance.GL.Context.GetViewport ().Height / 2);
			this.AddChild (titleScreen);
			
			// Fancy "fade-in" animation
			Vector4 origColor = titleScreen.Color;
			titleScreen.Color = new Vector4 (0, 0, 0, 0);
			var tintAction = new TintTo (origColor, 10.0f);
			ActionManager.Instance.AddAction (tintAction, titleScreen);
			tintAction.Run ();
			
			// Load and prepare title song
			bgm = new Bgm ("/Application/Audio/menu.mp3");
			
			if (bgmPlayer != null)
				bgmPlayer.Dispose ();
			bgmPlayer = bgm.CreatePlayer ();
			
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
			Touch.GetData (0).Clear ();
		}
		
		public override void OnEnter ()
		{
			bgmPlayer.Loop = true;
			bgmPlayer.Play ();
		}
		
		public override void OnExit ()
		{
			base.OnExit ();
			bgmPlayer.Stop ();
			bgmPlayer.Dispose ();
			bgmPlayer = null;
		}
        
		public override void Update (float dt)
		{
			// Wait for any input to load Main Menu
			base.Update (dt);
			var touches = Touch.GetData (0).ToArray ();
			if ((touches.Length > 0 && touches [0].Status == TouchStatus.Down) || Input2.GamePad0.Cross.Press) {
				Director.Instance.ReplaceScene (new MenuScene ());
			}
		}
		
		~TitleScene ()
		{
			_Texture.Dispose ();
			_TexInfo.Dispose ();
		}
	}
}

