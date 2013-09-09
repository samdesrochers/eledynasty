using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class ActionPanel
	{
		public int Configuration;
		public Panel Panel;
		private ImageBox Image_Background;
		private ImageBox Image_Selector;
		
		public List<ActionItem> ActionItems;
		public ActionItem AttackItem;
		public ActionItem MoveItem;
		public ActionItem CancelItem;
		
		public Vector2 Position;
		public int SelectedIndex;
		private int ItemCount;
		
		public ActionPanel (Vector2 pos)
		{
			Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL;
			SelectedIndex = 0;
			this.Position = pos;
			
			Panel = new Panel();
			Panel.Width = Constants.UI_ELEMENT_ACTIONBOX_WIDTH;
			Panel.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
			Panel.SetPosition(this.Position.X, this.Position.Y);
			
			Vector2 pos_att = new Vector2(Position.X , Position.Y + 15);
			Vector2 pos_move = new Vector2(Position.X , Position.Y + 40);
			Vector2 pos_cancel = new Vector2(Position.X, Position.Y + 75);
			
			AttackItem = new ActionItem("ATTACK", pos_att, Constants.UI_ELEMENT_ACTION_TYPE_ATTACK, AssetsManager.Instance.Image_Panel_Attack_Icon);
			MoveItem = new ActionItem("WAIT ", pos_move, Constants.UI_ELEMENT_ACTION_TYPE_WAIT, AssetsManager.Instance.Image_Panel_Wait_Icon);
			CancelItem = new ActionItem("CANCEL", pos_cancel, Constants.UI_ELEMENT_ACTION_TYPE_CANCEL, AssetsManager.Instance.Image_Panel_Cancel_Icon);
			
			ActionItems = new List<ActionItem>();
			ActionItems.Add(MoveItem);
			ActionItems.Add(CancelItem);
			
			ItemCount = ActionItems.Count;
			
			Image_Background = new ImageBox();
			Image_Background.Width = Constants.UI_ELEMENT_ACTIONBOX_WIDTH;
			Image_Background.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
			Image_Background.SetPosition(this.Position.X, this.Position.Y);
			Image_Background.Image = AssetsManager.Instance.Image_Panel_BG;			
			Image_Background.Visible = true;

			
			// Initial State is Move/Cancel only, adjust in consquence
			UpdateUIElementsPositions();
			
			Panel.AddChildLast(Image_Background);
			Panel.AddChildLast(MoveItem.ActionPanel);
			Panel.AddChildLast(CancelItem.ActionPanel);

			Panel.Visible = false;
		}
		
		public void ResetDefault()
		{
			SelectedIndex = 0;
			
			foreach(ActionItem a in ActionItems)
				a.Text_Action.TextColor = new UIColor(1,1,1,1);

		}
		
		public bool IsActive()
		{
			return Panel.Visible;
		}
		
		public void SetActive(bool active)
		{
			Panel.Visible = active;
			if(!active)
			{
				ResetDefault();
			}
		}
		
		public void SetActiveConfiguration(int type)
		{
			ResetDefault();
			
			if(type == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL && Configuration != type)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL;
				Panel.RemoveChild(AttackItem.ActionPanel);
				ActionItems.Remove(AttackItem);
				ItemCount = ActionItems.Count;		
				
				this.UpdateUIElementsPositions();
			}
			else if(type == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK && Configuration != type)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK;
				Panel.RemoveChild(Image_Background);
				Panel.AddChildFirst(AttackItem.ActionPanel);
				Panel.AddChildFirst(Image_Background);
				ActionItems.Insert(0, AttackItem);
				ItemCount = ActionItems.Count;	
				
				this.UpdateUIElementsPositions();
			}
			FocusByIndex();
		}
		
		private void UpdateUIElementsPositions()
		{
			if(this.Configuration == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL)
			{
				MoveItem.ActionPanel.SetPosition(MoveItem.Position.X, MoveItem.Position.Y - 25);
				CancelItem.ActionPanel.SetPosition(CancelItem.Position.X, CancelItem.Position.Y - 25);
				
				Panel.Height -= 30;
				Image_Background.Height -= 30;
				
			}
			else if(this.Configuration == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK)
			{
				MoveItem.ActionPanel.SetPosition(MoveItem.Position.X, MoveItem.Position.Y + 10);
				CancelItem.ActionPanel.SetPosition(CancelItem.Position.X, CancelItem.Position.Y + 10);
				
				Panel.Height += 30;
				Image_Background.Height += 30;
			}
		}
		
		public void FocusNextItemUp()
		{
			SelectedIndex = Math.Abs( (SelectedIndex - 1) % this.ItemCount );
			FocusByIndex();
		}
		
		public void FocusNextItemDown()
		{
			SelectedIndex = Math.Abs ( (SelectedIndex + 1) % this.ItemCount );
			FocusByIndex();
		}
		
		private void FocusByIndex()
		{
			// Resets all color
			foreach(ActionItem a in ActionItems)
				a.Text_Action.TextColor = new UIColor(1,1,1,1);
			
			// Find next item and change its color
			int count = 0;
			foreach(ActionItem a in ActionItems)
			{			
				if(SelectedIndex == count)
					a.Text_Action.TextColor = new UIColor(1, 0.7f, 0, 1);
				
				count ++;
			}
		}
		
		public int GetSelectedAction()
		{
			// Find next item and return its action
			int count = 0;
			foreach(ActionItem a in ActionItems)
			{			
				if(SelectedIndex == count)
					return a.Action;
				
				count ++;
			}
			return -1; // error occured
		}
	}
}

