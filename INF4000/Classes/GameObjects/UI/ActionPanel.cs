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
		
		public List<ActionItem> ActionItems;
		public ActionItem AttackItem;
		public ActionItem ProduceItem;
		public ActionItem MoveItem;
		public ActionItem CancelItem;
		
		public Vector2 Position;
		public int SelectedIndex;
		private int ItemCount;
		
		public Vector2 Right_Anchor;
		public Vector2 Left_Anchor;
		
		public ActionPanel (Vector2 pos)
		{
			Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL;
			SelectedIndex = 0;
			this.Position = pos;
			Left_Anchor = pos;
			
			Panel = new Panel();
			Panel.Width = Constants.UI_ELEMENT_ACTIONBOX_WIDTH;
			Panel.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
			Panel.SetPosition(this.Position.X, this.Position.Y);
			
			Vector2 pos_att = new Vector2(0 , 0 + 15);
			Vector2 pos_move = new Vector2(0 , 0 + 40);
			Vector2 pos_cancel = new Vector2(0, 0 + 75);
			
			AttackItem = new ActionItem("ATTACK", pos_att, Constants.UI_ELEMENT_ACTION_TYPE_ATTACK, AssetsManager.Instance.Image_Panel_Attack_Icon);
			ProduceItem = new ActionItem("PRODUCE", pos_att, Constants.UI_ELEMENT_ACTION_TYPE_PRODUCE, AssetsManager.Instance.Image_Panel_Gld_Icon);
			MoveItem = new ActionItem("WAIT ", pos_move, Constants.UI_ELEMENT_ACTION_TYPE_WAIT, AssetsManager.Instance.Image_Panel_Wait_Icon);
			CancelItem = new ActionItem("CANCEL", pos_cancel, Constants.UI_ELEMENT_ACTION_TYPE_CANCEL, AssetsManager.Instance.Image_Panel_Cancel_Icon);
			
			ActionItems = new List<ActionItem>();
			ActionItems.Add(MoveItem);
			ActionItems.Add(CancelItem);
			
			ItemCount = ActionItems.Count;
			
			Image_Background = new ImageBox();
			Image_Background.Width = Constants.UI_ELEMENT_ACTIONBOX_WIDTH;
			Image_Background.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
			Image_Background.Image = AssetsManager.Instance.Image_Action_Panel_BG;		
			Image_Background.Visible = true;
			
			// Initial State is Move/Cancel only, adjust in consquence
			UpdateUIElementsPositions();
			
			Panel.AddChildLast(Image_Background);
			Panel.AddChildLast(MoveItem.Panel);
			Panel.AddChildLast(CancelItem.Panel);

			Panel.Visible = false;
		}
		
		public void ResetDefault()
		{
			SelectedIndex = 0;
			MoveItem.Text_Action.Text = "WAIT";
			ProduceItem.Text_Action.Text = "PRODUCE";
			
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
			
			// Clear panel
			Panel.RemoveChild(Image_Background);
			foreach(ActionItem a in ActionItems)
				Panel.RemoveChild(a.Panel);
			ActionItems.Clear();
			
			if(type == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL;			
				
				Panel.AddChildLast(Image_Background);
				Panel.AddChildLast(CancelItem.Panel);
				Panel.AddChildLast(MoveItem.Panel);
				
				ActionItems.Add(MoveItem);
				ActionItems.Add(CancelItem);
				
				ItemCount = ActionItems.Count;					
				this.UpdateUIElementsPositions();
			}
			else if(type == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK;
				
				Panel.AddChildLast(Image_Background);
				Panel.AddChildLast(CancelItem.Panel);
				Panel.AddChildLast(MoveItem.Panel);
				Panel.AddChildLast(AttackItem.Panel);
				
				ActionItems.Add(AttackItem);
				ActionItems.Add(MoveItem);
				ActionItems.Add(CancelItem);

				ItemCount = ActionItems.Count;					
				this.UpdateUIElementsPositions();
			}
			else if(type == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE_ATTACK)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE_ATTACK;				
				ProduceItem.Text_Action.Text = "BUILD [" +BuildingUtil.GetHighLightedBuildingGoldToProduce().ToString()+ "]";
				
				Panel.AddChildLast(Image_Background);
				Panel.AddChildLast(CancelItem.Panel);
				Panel.AddChildLast(MoveItem.Panel);
				Panel.AddChildLast(AttackItem.Panel);
				Panel.AddChildLast(ProduceItem.Panel);
				
				ActionItems.Add(ProduceItem);
				ActionItems.Add(AttackItem);
				ActionItems.Add(MoveItem);
				ActionItems.Add(CancelItem);
				
				ItemCount = ActionItems.Count;				
				this.UpdateUIElementsPositions();
			}
			else if(type == Constants.UI_ELEMENT_CONFIG_CANCEL_PRODUCE)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_CANCEL_PRODUCE;
				ProduceItem.Text_Action.Text = "BUILD [" +BuildingUtil.GetHighLightedBuildingGoldToProduce().ToString()+ "]" ;
				
				Panel.AddChildLast(Image_Background);
				Panel.AddChildLast(CancelItem.Panel);
				Panel.AddChildLast(ProduceItem.Panel);
				
				ActionItems.Add(ProduceItem);
				ActionItems.Add(CancelItem);
				
				ItemCount = ActionItems.Count;				
				this.UpdateUIElementsPositions();
			}
			
			else if(type == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE)
			{
				Configuration = Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE;
				ProduceItem.Text_Action.Text = "BUILD [" +BuildingUtil.GetHighLightedBuildingGoldToProduce().ToString()+ "]" ;
				
				Panel.AddChildLast(Image_Background);
				Panel.AddChildLast(CancelItem.Panel);
				Panel.AddChildLast(MoveItem.Panel);
				Panel.AddChildLast(ProduceItem.Panel);
				
				ActionItems.Add(ProduceItem);
				ActionItems.Add(MoveItem);
				ActionItems.Add(CancelItem);
				
				ItemCount = ActionItems.Count;				
				this.UpdateUIElementsPositions();
			}
			
			FocusByIndex();
		}
		
		private void UpdateUIElementsPositions()
		{
			Panel.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
			Image_Background.Height = Constants.UI_ELEMENT_ACTIONBOX_HEIGHT;
			
			AttackItem.Panel.SetPosition(0,0);
			ProduceItem.Panel.SetPosition(0,0);
			MoveItem.Panel.SetPosition(0,0);
			CancelItem.Panel.SetPosition(0,0);
			
			int X_anchor = 5;
			int Y_offset = 10;
			
			if(this.Configuration == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL)
			{
				MoveItem.Panel.SetPosition(X_anchor, 10 + Y_offset);
				CancelItem.Panel.SetPosition(X_anchor, 40 + Y_offset);
				
				Panel.Height -= 30;
				Image_Background.Height -= 30;
				
			}
			else if(this.Configuration == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK || this.Configuration == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE)
			{
				ProduceItem.Panel.SetPosition(X_anchor,10 + Y_offset);
				AttackItem.Panel.SetPosition(X_anchor,10 + Y_offset);
				MoveItem.Panel.SetPosition(X_anchor, 40 + Y_offset);
				CancelItem.Panel.SetPosition(X_anchor, 70 + Y_offset);
				
				//Panel.Height += 30;
				//Image_Background.Height += 30;
			}
			else if(this.Configuration == Constants.UI_ELEMENT_CONFIG_CANCEL_PRODUCE)
			{
				ProduceItem.Panel.SetPosition(X_anchor,10 + Y_offset);
				CancelItem.Panel.SetPosition(X_anchor, 40 + Y_offset);
				
				Panel.Height -= 30;
				Image_Background.Height -= 30;
			}
			else if(this.Configuration == Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE_ATTACK)
			{
				ProduceItem.Panel.SetPosition(X_anchor, 10 + Y_offset);
				AttackItem.Panel.SetPosition(X_anchor,40 + Y_offset);
				MoveItem.Panel.SetPosition(X_anchor,70 + Y_offset);
				CancelItem.Panel.SetPosition(X_anchor, 100 + Y_offset);
				
				Panel.Height += 40;
				Image_Background.Height += 40;
			}
		}
		
		public void FocusNextItemUp()
		{
			int lastIndex = SelectedIndex;
			SelectedIndex = (SelectedIndex - 1) % this.ItemCount;
			
			if(SelectedIndex >= 0){
				FocusByIndex();
			} else {
				SelectedIndex = lastIndex;
			}
		}
		
		public void FocusNextItemDown()
		{		
			SelectedIndex = (SelectedIndex + 1) % this.ItemCount;	
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
		
		public void SetToRightOfScreen()
		{
			Panel.SetPosition(Right_Anchor.X, Right_Anchor.Y);
		}
		
		public void SetToLeftOfScreen()
		{
			Panel.SetPosition(Left_Anchor.X, Left_Anchor.Y);
		}
		
		public void SetTop()
		{
			Panel.SetPosition(23, 109);
		}
		
		public void SetBottom()
		{
			Panel.SetPosition(23, 320);
		}
	}
}

