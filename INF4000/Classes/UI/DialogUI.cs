using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class DialogUI
	{
		
		public Scene _UIScene;
		public Panel MainPanel;
	
		public ImageBox Image_Background;
		public ImageBox Image_Background_Speaker;
		public ImageBox Image_Speaker;
		public Label Label_SpeakerName;
		public Label Label_Text;
		
		
		public DialogUI ()
		{
			MainPanel = new Panel();
            MainPanel.Width = 960;
            MainPanel.Height = 544;
			
			Image_Background = new ImageBox();
			Image_Background.Width = 960;
			Image_Background.Height = 200;
			Image_Background.SetPosition(0.0f, 544 - 220.0f);
			Image_Background.Image = AssetsManager.Instance.Image_TurnSwitch_BG;			
			Image_Background.Visible = true;
			
			Image_Background_Speaker = new ImageBox();
			Image_Background_Speaker.Width = 200;
			Image_Background_Speaker.Height = 200;
			Image_Background_Speaker.SetPosition(20, 130);
			Image_Background_Speaker.Image = AssetsManager.Instance.Image_Dialog_Overlay_Blue;		
			Image_Background_Speaker.Visible = true;
			
			Image_Speaker = new ImageBox();
			Image_Speaker.Width = 200;
			Image_Speaker.Height = 200;
			Image_Speaker.SetPosition(20, 130);
			Image_Speaker.Image = AssetsManager.Instance.Image_Dialog_Kenji_1;		
			Image_Speaker.Visible = true;
			
			Label_SpeakerName = new Label();
			Label_SpeakerName.Text = "Kenji";
			Label_SpeakerName.Font = AssetsManager.Instance.PixelFont;
			Label_SpeakerName.SetPosition(55, 360);
			
			// 50 char per line before \n allowed
			Label_Text = new Label();
			Label_Text.Text = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcd\nabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcd\nabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()abcd\n";
			Label_Text.Font = AssetsManager.Instance.PixelFont;
			Label_Text.SetPosition(55, 350);
			Label_Text.SetSize(850, 180);
			
			MainPanel.AddChildLast(Image_Background);
			MainPanel.AddChildLast(Image_Background_Speaker);
			MainPanel.AddChildLast(Image_Speaker);
			MainPanel.AddChildLast(Label_SpeakerName);
			MainPanel.AddChildLast(Label_Text);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(MainPanel);
		}
		
		public void SetSpeakerInfo(string name)
		{
			if(name == Constants.CHAR_KENJI){
				Image_Background_Speaker.Image = AssetsManager.Instance.Image_Dialog_Overlay_Blue;	
				Image_Speaker.Image = AssetsManager.Instance.Image_Dialog_Kenji_1;
				Label_SpeakerName.Text = "-"+name;
			} else if (name == Constants.CHAR_GOHZU) {
				Image_Background_Speaker.Image = AssetsManager.Instance.Image_Dialog_Overlay_Red;	
				Image_Speaker.Image = AssetsManager.Instance.Image_Dialog_Gohzu_1;
				Label_SpeakerName.Text = "-"+name+"-";
			}
		}
		
		public void SetSpeakerText(Tuple<string, string> sequence)
		{
			// Set the speaker info
			SetSpeakerInfo(sequence.Item1);
		}
		
		// Will set the dialog as active UI (overwrite the GameUI for ex.)
		public void SetActive()
		{		
			UISystem.SetScene(_UIScene);
		}
	}
}



