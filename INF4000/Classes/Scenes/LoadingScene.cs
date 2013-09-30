using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class LoadingScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		Texture2D texture;
		TextureInfo ti;
		
		public LoadingScene ()
		{
			// Load title screen image and prepare camera
			this.Camera.SetViewFromViewport ();
			
            texture = new Texture2D("/Application/Assets/Title/loading.png", false);
			ti = new TextureInfo(texture);
			SpriteUV sprite = new SpriteUV(ti);
			sprite.Quad.S = new Vector2(960, 544);
			sprite.Position = new Sce.PlayStation.Core.Vector2(0,0);
			this.AddChild(sprite);
			Scheduler.Instance.ScheduleUpdateForTarget (this, 0, false);
		}
		
		public override void Draw ()
        {
            base.Draw();
        }
		
        float loadTime = 0.0f;
		
		public override void Update (float dt)
		{
			// Wait for any input to load Main Menu
			base.Update (dt);
			loadTime += dt;
			
			if (loadTime >= 8.0f) {
				Director.Instance.ReplaceScene( new GameScene() );
			}
		}
		
		~LoadingScene ()
		{
			ti.Dispose();
			texture.Dispose();
		}
	}
}

