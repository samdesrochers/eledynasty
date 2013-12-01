using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class LoadingScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene _UIScene;
		private Sce.PlayStation.HighLevel.UI.Panel MainPanel;
		private ImageBox Background;
		
		bool cleanUp;
		bool loaded;
		
		public LoadingScene ()
		{
			this.Camera.SetViewFromViewport ();
			MainPanel = new Panel();
            MainPanel.Width = Director.Instance.GL.Context.GetViewport().Width;
            MainPanel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			Background = new ImageBox();
            Background.Width = MainPanel.Width;
            Background.Image = new ImageAsset("/Application/Assets/Title/loading.png", false);
            Background.Height = MainPanel.Height;
            Background.SetPosition(0.0f,0.0f);
			
			MainPanel.AddChildLast(Background);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
			
			cleanUp = false;
			loaded = false;
			
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
		}
		
		public override void Draw ()
        {
            base.Draw();
			UISystem.Render ();
        }
		
        float loadTime = 0.0f;
		
		public override void Update (float dt)
		{
			if(!cleanUp) {
				// CLEAN UP			
				AssetsManager.Instance.Dispose();
				SoundManager.Instance.Dispose();
				cleanUp = true;
			}
			
			if(cleanUp && !loaded) {
				AssetsManager.Instance.LoadAssets();
				SoundManager.Instance.LoadSounds();
				loaded = true;
			}
			
			if (loaded && cleanUp && loadTime > 3.0f) {
				Director.Instance.ReplaceScene( new OverworldScene() );
			}
			
			loadTime += dt;
		}
	}
}

