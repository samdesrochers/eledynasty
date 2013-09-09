using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class ActionItem
	{
		public Panel ActionPanel;
		public Vector2 Position;
		private ImageBox Image_Action;
		public Label Text_Action;
		private string Text;
		
		public bool IsActive;
		public int Action;
		
		public ActionItem (string text, Vector2 pos, int action, ImageAsset image)
		{
			IsActive = false;
			Position = pos;
			Text = text;
			this.Action = action;
			
			ActionPanel = new Panel();
			ActionPanel.SetPosition(Position.X, Position.Y);		
            ActionPanel.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
            ActionPanel.Width = Constants.UI_ELEMENT_ACTIONBOX_WIDTH;
			
			Image_Action = new ImageBox();
			Image_Action.Width = 10 + Constants.TILE_SIZE/2;
			Image_Action.Height = 10 + Constants.TILE_SIZE/2;
			Image_Action.SetPosition(140, -10);
			Image_Action.Image = image;
			Image_Action.Visible = true;
		
			Text_Action = new Label();
			Text_Action.Text = Text;
			Text_Action.SetPosition(15, 0);
			Text_Action.Font = AssetsManager.Instance.PixelFont;
			Text_Action.Visible = true;
			
			ActionPanel.AddChildLast(Image_Action);
			ActionPanel.AddChildLast(Text_Action);
		}
		
		public void SetActive(bool active)
		{			
			IsActive = active;
			ActionPanel.Visible = IsActive;
		}
	}
}

