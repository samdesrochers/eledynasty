using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class CinematicDialogManager
	{
		public Queue<string> DialogSequences;
		public string CurrentSequence;
		
		private string CurrentText;
		private int CurrentTextIndex;
		
		private float DisplayTime;
		
		public CinematicDialogManager ()
		{
			DialogSequences = new Queue<string>();
			CurrentSequence = "";
			CurrentText = "";
			CurrentTextIndex = 0;
		}
		
		public void NextSequence()
		{
			if(DialogSequences.Count == 0)
				return;
			
			CurrentText = "";
			CurrentTextIndex = 0;
			CinematicScene.Instance.CUI.Label_Text.Text = "";
			CurrentSequence = DialogSequences.Dequeue();
			
		}		
		
		public void Update(float dt)
		{
			if(CurrentSequence != null && CurrentTextIndex < CurrentSequence.Length) {
				if(DisplayTime > 0.04) {
					CurrentText += CurrentSequence[CurrentTextIndex];
					CinematicScene.Instance.CUI.Label_Text.Text = CurrentText;
					CurrentTextIndex ++;
					DisplayTime = 0;
				}
			}
			DisplayTime += dt;
		}
	}
}

