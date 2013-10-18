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
		
        public MenuScene ()
        {
            this.Camera.SetViewFromViewport();
            Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
            dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
            dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
            
            ImageBox ImageBox = new ImageBox();
            ImageBox.Width = dialog.Width;
            ImageBox.Image = new ImageAsset("/Application/Assets/Title/title.png", false);
            ImageBox.Height = dialog.Height;
            ImageBox.SetPosition(0.0f,0.0f);
            
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
				if(OverworldScene.Instance != null)
                Director.Instance.ReplaceScene( OverworldScene.Instance );
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

