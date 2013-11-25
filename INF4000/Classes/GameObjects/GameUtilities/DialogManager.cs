using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
namespace INF4000
{
	public class DialogManager
	{
		public float AnimationTime;
		
		public List<Tuple<string, string>> DialogSequences;
		public Tuple<string, string> CurrentDialogSequence;
		
		public int ActiveSequenceIndex;
		int CurrentCharIndex;
		string CurrentSequenceText;
		
		public DialogManager (List<Tuple<string, string>> dialog)
		{
			DialogSequences = dialog;
			ActiveSequenceIndex = -1;
			AnimationTime = 0.0f;
			CurrentCharIndex = 0;
		}
		
		public void SetNextSequence()
		{
			ActiveSequenceIndex ++;
			if(ActiveSequenceIndex < DialogSequences.Count)
			{
				CurrentDialogSequence = DialogSequences[ActiveSequenceIndex];
				if(CurrentDialogSequence != null)
					GameScene.Instance.DialogUI.SetSpeakerInfo(CurrentDialogSequence.Item1);
			}
			else
				CurrentDialogSequence = null;
		}
		
		public void Update(float dt)
		{		
			if(CurrentDialogSequence != null && CurrentCharIndex < CurrentDialogSequence.Item2.Length)
			{
				CurrentSequenceText += CurrentDialogSequence.Item2[CurrentCharIndex];
				GameScene.Instance.DialogUI.Label_Text.Text = CurrentSequenceText;
				
				if(AnimationTime > 0.15f){
					AnimationTime = 0;
					SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_MOVE);
				}
			}
			   
			CurrentCharIndex++;
			
			// Check for user input 
			if (Input2.GamePad.GetData (0).Cross.Release) 
			{
				SetNextSequence();
				CurrentSequenceText = "";
				CurrentCharIndex = 0;
				
				if(CurrentDialogSequence != null && CurrentDialogSequence.Item1 != Constants.DIALOG_EOF)
				{
					GameScene.Instance.DialogUI.Label_Text.Text = "";	
				}
				else if(CurrentDialogSequence != null && CurrentDialogSequence.Item1 == Constants.DIALOG_EOF) // Last sequence, state switch
				{			
					GameScene.Instance.DialogUI.Label_Text.Text = "";
					int nextState = Convert.ToInt32(CurrentDialogSequence.Item2);
					GameScene.Instance.CurrentGlobalState = nextState;
					Utilities.ShowGameUI();
					SetNextSequence();
				}
				else if(CurrentDialogSequence == null)
				{
					ClearDialogPanelDefault();
				}
			}
			
			if(CurrentDialogSequence == null)
			{
				ClearDialogPanelDefault();
			}
			
			AnimationTime += dt;
		}
		
		private void ClearDialogPanelDefault()
		{
			GameScene.Instance.DialogUI.Label_Text.Text = "";
			Utilities.ShowGameUI();
			GameScene.Instance.CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
		}
	}
}

