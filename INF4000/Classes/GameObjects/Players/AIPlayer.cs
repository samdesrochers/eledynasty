using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class AIPlayer : Player
	{	
		public int AI_State;
		public int AI_Action;
		
		public AIPlayer ()
		{
			this.Name = "";
			this.Units = new List<Unit>();
			this.Buildings = new List<Building>();
			this.IsActive = false;
			this.IsHuman = false;
			this.FocusPoints = 0;
			this.Gold = 5;
			
			this.TargetUnits = new List<Unit>();
			AI_State = Constants.AI_STATE_WAITING;
			AI_Action = Constants.AI_ACTION_NONE;
		}
		
		public override void Reset() 
		{
			base.Reset();
			AI_State = Constants.AI_STATE_WAITING;
		}
		
		public override void Update ()
		{
			base.Update ();		
			
			// Try to pick a Unit so that we can still do a move/attack
			if( AI_State == Constants.AI_STATE_WAITING )
			{
				SelectUnit();
			}
			else if( AI_State == Constants.AI_STATE_UNIT_SELECTED ) 
			{
				SelectAction();
			}
			else if( AI_State == Constants.AI_STATE_ACTION_SELECTED ) // Do the actual move/attack
			{
				LaunchActionOnSelectedUnit();
			}
			else if ( AI_State == Constants.AI_STATE_EXECUTING_ACTION )
			{
				if(ActiveUnit != null && !ActiveUnit.IsActive) {
					AI_State = Constants.AI_STATE_WAITING;
					AI_Action = Constants.AI_ACTION_NONE;
				}
			}
		}
		
		#region AI Methods
		private void SelectUnit()
		{
			this.ActiveUnit = TrySelectUnit();
			if(this.ActiveUnit != null)
			{
				AI_State = Constants.AI_STATE_UNIT_SELECTED;
			}
		}
		
		private void SelectAction()
		{
			AI_Action = Constants.AI_ACTION_MOVE;
			
			AI_State = Constants.AI_STATE_ACTION_SELECTED;	
		}
		
		private Unit TrySelectUnit()
		{
			foreach(Unit unit in Units)
			{
				if(unit.IsMovable())
					return unit;
			}
			return null;
		}
		
		private void LaunchActionOnSelectedUnit()
		{
			switch(AI_Action) {
				case Constants.AI_ACTION_MOVE:
					MoveAction();
					break;
				default:
					ActiveUnit.Move_RadiusLeft = 0;
					AI_State = Constants.AI_STATE_WAITING; // error finding next move, switch unit (fatal abort)
					break;
			}
			
			if(AI_Action == Constants.AI_ACTION_NONE) { // abort premarturaly
					ActiveUnit.Move_RadiusLeft = 0;
					AI_State = Constants.AI_STATE_WAITING; 
			}
		}
		
		private void MoveAction()
		{
			int radiusLeft = ActiveUnit.Move_RadiusLeft;
			Vector2i destination = GetValidDestination(radiusLeft);
			
			if(destination.X >= 0 && destination.Y >= 0) {
				GameActions.AI_MoveUnitTo(destination, ActiveUnit);
				GameScene.Instance.UpdateCameraPositionBySelectedUnit();		
				AI_State = Constants.AI_STATE_EXECUTING_ACTION;
				
			} else {
				AI_Action = Constants.AI_ACTION_NONE; // fatal error, remove action. Will be handeled exiting the switch case in launch action
			}
		}
		
		#endregion	
		
		#region Util Methods for AI decisions
		private Vector2i GetValidDestination(int radius)
		{
			int retries = 4;
			while(retries > 0)
			{
				if(retries == 4)
					if(Utilities.IsDestinationValid(new Vector2i(ActiveUnit.WorldPosition.X, ActiveUnit.WorldPosition.Y + radius)))
						return ( new Vector2i(ActiveUnit.WorldPosition.X, ActiveUnit.WorldPosition.Y + radius) );	
				
				if(retries == 3)
					if(Utilities.IsDestinationValid(new Vector2i(ActiveUnit.WorldPosition.X + radius, ActiveUnit.WorldPosition.Y)))
						return ( new Vector2i(ActiveUnit.WorldPosition.X + radius, ActiveUnit.WorldPosition.Y) );	
				
				if(retries == 2)
					if(Utilities.IsDestinationValid(new Vector2i(ActiveUnit.WorldPosition.X - radius, ActiveUnit.WorldPosition.Y)))
						return ( new Vector2i(ActiveUnit.WorldPosition.X - radius, ActiveUnit.WorldPosition.Y) );	
					
				retries --;
			}
			return new Vector2i(-1, -1);
		}
		#endregion
	}
}

