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
	public class TutorialScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene _UIScene;
		private Sce.PlayStation.HighLevel.UI.Panel dialog;
		
		private ImageAsset GoldScreen;
		private ImageAsset StatsScreen;
		private ImageAsset FortScreen;
		
		private Button Button_Gold;
		private Button Button_Fort;
		private Button Button_Stats;

				
        public TutorialScene ()
        {
            this.Camera.SetViewFromViewport();
            dialog = new Panel();
            dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
            dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			            
            ImageBox PanelBG = new ImageBox();
            PanelBG.Width = dialog.Width;
            PanelBG.Image = new ImageAsset("/Application/Assets/UI/Tutorial/panel.png", false);
            PanelBG.Height = dialog.Height;
            PanelBG.SetPosition(0.0f,0.0f);
			
			GoldScreen = new ImageAsset("/Application/Assets/UI/Tutorial/gold_screen.png", false);
			StatsScreen = new ImageAsset("/Application/Assets/UI/Tutorial/stats_screen.png", false);
			FortScreen = new ImageAsset("/Application/Assets/UI/Tutorial/fort_screen.png", false);
			
			ImageBox PanelScreen = new ImageBox();
            PanelScreen.Width = 592;
			PanelScreen.Height = 496;
            PanelScreen.Image = GoldScreen;
            PanelScreen.SetPosition(343, 24);
            
            Button_Gold = new Button();
            Button_Gold.Width = 294;
            Button_Gold.Height = 70;
			Button_Gold.IconImage = new ImageAsset("/Application/Assets/UI/Tutorial/gold_but.png", false);
            Button_Gold.SetPosition(0, 30);
			Button_Gold.Alpha = 0.2f;
            Button_Gold.TouchEventReceived += (sender, e) => 
			{
				ResetAllAlpha();
				Button_Gold.Alpha = 0.2f;
				PanelScreen.Image = GoldScreen;
            };
			
			Button_Fort = new Button();
            Button_Fort.Width = 294;
            Button_Fort.Height = 70;
			Button_Fort.IconImage = new ImageAsset("/Application/Assets/UI/Tutorial/fort_but.png", false);
            Button_Fort.SetPosition(0, 130);
            Button_Fort.TouchEventReceived += (sender, e) => 
			{
				ResetAllAlpha();
				Button_Fort.Alpha = 0.2f;
				PanelScreen.Image = FortScreen;
            };
			
			Button_Stats = new Button();
            Button_Stats.Width = 294;
            Button_Stats.Height = 70;
			Button_Stats.IconImage = new ImageAsset("/Application/Assets/UI/Tutorial/stats_but.png", false);
            Button_Stats.SetPosition(0, 230);
            Button_Stats.TouchEventReceived += (sender, e) => 
			{
				ResetAllAlpha();
				Button_Stats.Alpha = 0.2f;
				PanelScreen.Image = StatsScreen;
            }; 
			
			Button Button_Back = new Button();
            Button_Back.Width = 294;
            Button_Back.Height = 70;
			Button_Back.IconImage = new ImageAsset("/Application/Assets/UI/Tutorial/back_but.png", false);
            Button_Back.SetPosition(0, 450);
            Button_Back.TouchEventReceived += (sender, e) => 
			{
				ResetAllAlpha();
				//Director.Instance.ReplaceScene( new MenuScene() );
				
				Director.Instance.ReplaceScene( new TransitionSolidFade( new MenuScene() )
   				{ Duration = 5.0f, Tween = (x) => Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.PowEaseOut( x, 3.0f )} );
			};
                
            dialog.AddChildLast(PanelBG);
			dialog.AddChildLast(PanelScreen);
            dialog.AddChildLast(Button_Gold);
			dialog.AddChildLast(Button_Fort);
			dialog.AddChildLast(Button_Stats);
			dialog.AddChildLast(Button_Back);
			
            _UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(dialog);
            UISystem.SetScene(_UIScene);
			
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
        }
		
		private void ResetAllAlpha()
		{
			Button_Fort.Alpha = 1;
			Button_Gold.Alpha = 1;
			Button_Stats.Alpha = 1;
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
        
        ~TutorialScene()
        {
			GoldScreen.Dispose();
			FortScreen.Dispose();
			StatsScreen.Dispose();
        }
	}
}

