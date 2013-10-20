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
	public class MenuScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene _UIScene;
		private Sce.PlayStation.HighLevel.UI.Panel dialog;
		
		private int State;
		private ImageBox Tween;
		
        public MenuScene ()
        {
            this.Camera.SetViewFromViewport();
            dialog = new Panel();
            dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
            dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			State = Constants.OV_STATE_IDLE;
            
            ImageBox ImageBox = new ImageBox();
            ImageBox.Width = dialog.Width;
            ImageBox.Image = new ImageAsset("/Application/Assets/Title/title.png", false);
            ImageBox.Height = dialog.Height;
            ImageBox.SetPosition(0.0f,0.0f);
			
			Tween = new ImageBox();
			Tween.Width = 960;
			Tween.Height = 544;
			Tween.SetPosition(0,0);
			Tween.Image = new ImageAsset("/Application/Assets/UI/whitebg.png", false);
			Tween.Alpha = 0;
            
            Button Button_Play = new Button();
            Button_Play.Name = "buttonPlay";
            Button_Play.Text = "Launch Game";
            Button_Play.Width = 500;
            Button_Play.Height = 100;
            Button_Play.Alpha = 0.8f;
			Button_Play.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_Play.SetPosition(dialog.Width/2 - Button_Play.Width/2, 300.0f);
            Button_Play.TouchEventReceived += (sender, e) => 
			{
				dialog.AddChildLast(Tween);
				State = Constants.OV_STATE_STARTING_GAME;
            };
            
            Button Button_Title = new Button();
            Button_Title.Name = "buttonMenu";
            Button_Title.Text = "Main Menu";
            Button_Title.Width = 500;
            Button_Title.Height = 50;
            Button_Title.Alpha = 0.8f;
            Button_Title.SetPosition(dialog.Width/2 - Button_Play.Width/2, 450.0f);
            Button_Title.TouchEventReceived += (sender, e) => 
			{
            	//Director.Instance.ReplaceScene(new GameScene());
            };        
                
            dialog.AddChildLast(ImageBox);
            dialog.AddChildLast(Button_Play);
            dialog.AddChildLast(Button_Title);
			
            _UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(dialog);
            UISystem.SetScene(_UIScene);
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
        }
		
        public override void Update (float dt)
        {
            base.Update (dt);
            UISystem.Update(Touch.GetData(0));
			
			if(State == Constants.OV_STATE_STARTING_GAME)
			{
				Tween.Alpha += dt;
				if(Tween.Alpha >= 1) {
					State = Constants.OV_STATE_IDLE;
					if(OverworldScene.Instance != null)
                		Director.Instance.ReplaceScene( OverworldScene.Instance );
				}			
			}
        }
        
        public override void Draw ()
        {
            base.Draw();
            UISystem.Render ();
        }
		
		public override void OnExit ()
		{
			
		}
        
        ~MenuScene()
        {

        }
	}
}

