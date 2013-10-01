using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class AIPlayer : Player
	{	
		public int AI_State;
		
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
			if(AI_State == Constants.AI_STATE_WAITING)
			{
				this.ActiveUnit = SelectUnit();
				if(this.ActiveUnit != null)
				{
					AI_State = Constants.AI_STATE_ACTION_DECIDED;
				}
			}
			else if(AI_State == Constants.AI_STATE_ACTION_DECIDED) // Do the actual move/attack
			{
				LaunchActionOnActiveUnit();
			}
			else if ( AI_State == Constants.AI_STATE_EXECUTING_ACTION)
			{
				if(ActiveUnit != null && !ActiveUnit.IsActive) {
					AI_State = Constants.AI_STATE_WAITING;
				}
			}
		}
		
		#region AI Methods
		private Unit SelectUnit()
		{
			foreach(Unit unit in Units)
			{
				if(unit.IsMovable())
					return unit;
			}
			return null;
		}
		
		private void LaunchActionOnActiveUnit()
		{
			int radiusLeft = ActiveUnit.Move_RadiusLeft;
			Vector2i destination = GetValidDestination(radiusLeft);
			
			if(destination.X >= 0 && destination.Y >= 0) {
				GameActions.AI_MoveUnitTo(destination, ActiveUnit);
				AI_State = Constants.AI_STATE_EXECUTING_ACTION;
			} else {
				AI_State = Constants.AI_STATE_WAITING; // error finding next move, switch unit (fatal abort)
			}
		}
		#endregion	
		
		#region Util Methods for AI decisions
		private Vector2i GetValidDestination(int radius)
		{
			bool found = false;
			int retries = 4;
			while(!found && retries > 0)
			{
				if(retries == 4)
					if(Utilities.IsDestinationValid(new Vector2i(ActiveUnit.WorldPosition.X + radius, ActiveUnit.WorldPosition.Y)))
						return ( new Vector2i(ActiveUnit.WorldPosition.X + radius, ActiveUnit.WorldPosition.Y) );	
				
				if(retries == 3)
					if(Utilities.IsDestinationValid(new Vector2i(ActiveUnit.WorldPosition.X - radius, ActiveUnit.WorldPosition.Y)))
						return ( new Vector2i(ActiveUnit.WorldPosition.X - radius, ActiveUnit.WorldPosition.Y) );	
				
				if(retries == 2)
					if(Utilities.IsDestinationValid(new Vector2i(ActiveUnit.WorldPosition.X, ActiveUnit.WorldPosition.Y + radius)))
						return ( new Vector2i(ActiveUnit.WorldPosition.X, ActiveUnit.WorldPosition.Y + radius) );	
					
				retries --;
			}
			return new Vector2i(-1, -1);
		}
		#endregion
	}
}

