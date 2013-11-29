using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class SpellUI
	{
		public Scene _UIScene;
		public Panel Panel;
		
		private ImageBox Image_Background;
		private Label Title;
		private Label Spell_1;
		
		public Vector2 Position;
		public int SelectedIndex;
		
		public SpellUI ()
		{
			SelectedIndex = 0;
			
			Panel = new Panel();
			Panel.Width = 960;
			Panel.Height = 544;
			
			Image_Background = new ImageBox();
			Image_Background.SetPosition(30, 30);
			Image_Background.Width = Constants.UI_ELEMENT_ACTIONBOX_WIDTH + 40;
			Image_Background.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT + 30;
			Image_Background.Image = AssetsManager.Instance.Image_Action_Panel_BG;	
			
			Title = new Label();
			Title.SetPosition(80, 45);
			Title.Font = AssetsManager.Instance.PixelFont;
			Title.Text = "- SPELLS -";
			
			Spell_1 = new Label();
			Spell_1.SetPosition(45, 70);
			Spell_1.Font = AssetsManager.Instance.PixelFont_18;
			Spell_1.TextColor = new UIColor(1,0.4f,0.1f,1);
			Spell_1.Text = "MASS HEAL - 10 pts";
			Spell_1.SetSize(260, 40);
			
			Panel.AddChildLast(Image_Background);
			Panel.AddChildLast(Title);
			Panel.AddChildLast(Spell_1);
			
			_UIScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _UIScene.RootWidget.AddChildLast(Panel);
		}
		
		public void SetActive()
		{		
			UISystem.SetScene(_UIScene);
		}
		
		public void SetInactive()
		{
			GameScene.Instance.DialogUI.Label_Text.Text = "";
			Utilities.ShowGameUI();
			GameScene.Instance.CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
		}
	}
}

