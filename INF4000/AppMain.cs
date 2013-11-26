using System;
using Sce.PlayStation.HighLevel.UI; 
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core.Input;


namespace INF4000
{
	public class AppMain
	{
        public static void Main (string[] args)
        {
            Director.Initialize();
            UISystem.Initialize(Director.Instance.GL.Context);
            Director.Instance.RunWithScene(new MenuScene(), true); // Use that TitleScene whenever   
			
			while( true )
			{
				Sce.PlayStation.Core.Environment.SystemEvents.CheckEvents();
			
				//Tick();
						
				Director.Instance.Update();
				Director.Instance.Render();
			
				UISystem.Update(Touch.GetData(0));
				UISystem.Render();
						
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap(); // you must call this after SwapBuffers
			}
        }
	}
}
