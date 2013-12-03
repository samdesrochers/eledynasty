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
	public class CreditsScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene _UIScene;
		private Sce.PlayStation.HighLevel.UI.Panel MainPanel;
		private ImageBox Background;
		private Sce.PlayStation.HighLevel.UI.Label Credit_1;
		private Sce.PlayStation.HighLevel.UI.Label Credit_2;
		private Sce.PlayStation.HighLevel.UI.Label Credit_3;
				
        public CreditsScene ()
        {
            this.Camera.SetViewFromViewport();
            MainPanel = new Panel();
            MainPanel.Width = Director.Instance.GL.Context.GetViewport().Width;
            MainPanel.Height = Director.Instance.GL.Context.GetViewport().Height;
			            
            Background = new ImageBox();
            Background.Width = MainPanel.Width;
            Background.Image = new ImageAsset("/Application/Assets/Title/title.png", false);
            Background.Height = MainPanel.Height;
            Background.SetPosition(0.0f,0.0f);
			Background.Alpha = 0.2f;
			
			UIFont font = new UIFont("/Application/Assets/fonts/half_bold_pixel-7.ttf", 24, FontStyle.Bold);
			float offset = 250;
			
			Credit_1 = new Sce.PlayStation.HighLevel.UI.Label();
			Credit_1.Font = font;
			Credit_1.SetPosition(MainPanel.Width/2 - offset, MainPanel.Height/2 - 30 - 50);
			Credit_1.SetSize(600, 30);
			Credit_1.Text = "Created by : Samuel Des Rochers";
			
			Credit_2 = new Sce.PlayStation.HighLevel.UI.Label();
			Credit_2.Font = font;
			Credit_2.SetPosition(MainPanel.Width/2 - offset, MainPanel.Height/2 - 50);
			Credit_2.SetSize(600, 150);
			Credit_2.Text = "Graphics by :  \n- Samuel Des Rochers \n- Philipp Lenssen \n- Tenbed from DevianArt";
			
			Credit_3 = new Sce.PlayStation.HighLevel.UI.Label();
			Credit_3.Font = font;
			Credit_3.SetPosition(MainPanel.Width/2 - offset, MainPanel.Height/2 + 130 - 50);
			Credit_3.SetSize(600, 100);
			Credit_3.Text = "Music by : \n- Phlogiston \n- BoxCat \n- Alex Mauer";
			
			
            MainPanel.AddChildLast(Background);
			MainPanel.AddChildLast(Credit_1);
			MainPanel.AddChildLast(Credit_2);
			MainPanel.AddChildLast(Credit_3);
			
            _UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
            UISystem.SetScene(_UIScene);
			
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
        }
		
		float AnimationTime = 0;
        public override void Update (float dt)
        {
            base.Update (dt);
            UISystem.Update(Touch.GetData(0));
			
			AnimationTime += dt;
			
			Credit_1.SetPosition(Credit_1.X, Credit_1.Y + 2*dt);
			Credit_2.SetPosition(Credit_1.X, Credit_2.Y + 2*dt);
			Credit_3.SetPosition(Credit_1.X, Credit_3.Y + 2*dt);
			
			
			if(AnimationTime > 10.0f)
			{
				Director.Instance.ReplaceScene(new MenuScene());
			}
			
			if(Input2.GamePad0.Cross.Release)
			{
				Director.Instance.ReplaceScene(new MenuScene());
			}
        }
        
        public override void Draw ()
        {
            base.Draw();
            UISystem.Render ();
        }      
	}
}

