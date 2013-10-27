using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class AIPlayer : Player
	{	
		public int AI_State;
		public int AI_Action;
		public int AI_Behavior;
		
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
			AI_State = Constants.AI_STATE_BEGIN_TURN;
			AI_Action = Constants.AI_ACTION_NONE;
			AI_Behavior = Constants.AI_BEHAVIOR_DEFENSE;
		}
		
		public override void Reset() 
		{
			base.Reset();
			AI_State = Constants.AI_STATE_BEGIN_TURN;
		}
		
		public override void Update ()
		{
			base.Update ();		
			
			// Try to pick a Unit so that we can still do a move/attack
			if( AI_State == Constants.AI_STATE_WAITING ){
				SelectUnit();
			} else if( AI_State == Constants.AI_STATE_UNIT_SELECTED ) {
				SetActions();
			} else if( AI_State == Constants.AI_STATE_ACTIONS_PREPARED ) {
				LaunchActionOnSelectedUnit();
			} else if ( AI_State == Constants.AI_STATE_EXECUTING_ACTION ) {
				if(ActiveUnit != null && !ActiveUnit.IsActive && ActiveUnit.AI_Actions.Count == 0) {
					AI_State = Constants.AI_STATE_WAITING;
					AI_Action = Constants.AI_ACTION_NONE;
				} else if(ActiveUnit != null && !ActiveUnit.IsActive && ActiveUnit.AI_Actions.Count != 0) {
					AI_State = Constants.AI_STATE_ACTIONS_PREPARED;
				}
			} else if( AI_State == Constants.AI_STATE_BEGIN_TURN ) {
				BeginTurn();
			} else if( AI_State == Constants.AI_STATE_END_TURN ) {
				EndTurn();
			}
		}
		
		#region AI Methods
		private void BeginTurn()
		{
			AssignBehavior();
			TryProduce();
			AI_State = Constants.AI_STATE_WAITING;
		}
		
		private void EndTurn()
		{
			
		}
		
		private void SelectUnit()
		{
			this.ActiveUnit = TrySelectUnit();
			if(this.ActiveUnit != null)
			{
				this.ActiveUnit.AI_Actions = new Queue();
				AI_State = Constants.AI_STATE_UNIT_SELECTED;
			}
		}
		
		private void SetActions()
		{
			if(ActiveUnit.Behavior == Constants.UNIT_AI_BEHAV_ATTACK){
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_ATTACK);
			} else if(ActiveUnit.Behavior == Constants.UNIT_AI_BEHAV_DEFEND) {
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
			} else if(ActiveUnit.Behavior == Constants.UNIT_AI_BEHAV_CAPTURE) {
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_CAPTURE);
			} else {
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
			}	
					
			AI_State = Constants.AI_STATE_ACTIONS_PREPARED;	
		}
		
		private void LaunchActionOnSelectedUnit()
		{
			if(ActiveUnit.AI_Actions == null || ActiveUnit.AI_Actions.Count == 0) { 
				Abort();
				return;
			}
			
			AI_Action = (int)ActiveUnit.AI_Actions.Dequeue();
			switch(AI_Action) {
				case Constants.AI_ACTION_MOVE:
					MoveAction();
					break;
				case Constants.AI_ACTION_ATTACK:
					AttackAction();
					break;
				case Constants.AI_ACTION_DEFEND:
					MoveAction();
					break;
				case Constants.AI_ACTION_CAPTURE:
					CaptureAction();
					break;
				default:
					Abort();
					break;
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
		
		private void AttackAction()
		{
			Console.WriteLine("Attacking yo");
		}
		
		private void CaptureAction()
		{
			Console.WriteLine("Capturing yall");
		}
		
		#endregion	
		
		#region AI Inner Methods
		private void SelectDestination_Attack()
		{
			
		}
		
		private void SelectDestination_Defend()
		{
			
		}
		private void SelectDestination_Capture()
		{
			
		}
		
		private void TryProduce()
		{
			Building fort = Buildings[0]; // always first building
			List<Building> farms = new List<Building>();
			List<Building> temples = new List<Building>();
			List<Building> forges = new List<Building>();
			
			foreach(Building b in Buildings)
				if(b.Type == Constants.BUILD_FORT)
					fort = b;
			
			foreach(Building b in Buildings)
			{
				if(b.Type ==  Constants.BUILD_FARM)
					farms.Add(b);
				else if(b.Type == Constants.BUILD_TEMPLE)
					temples.Add(b);
				else if(b.Type == Constants.BUILD_FORGE)
					forges.Add(b);
				
				int value = GetDistanceValue(fort.WorldPosition, b.WorldPosition);
				b.AI_ProductionValue = value;
			}
			
			List<Building> sortedForges = forges.OrderBy(o=>o.AI_ProductionValue).ToList();
			foreach(Building b in sortedForges)
				b.ProduceUnit(Name, this);
			
			List<Building> sortedTemples = temples.OrderBy(o=>o.AI_ProductionValue).ToList();
			foreach(Building b in sortedTemples)
				b.ProduceUnit(Name, this);
			
			List<Building> sortedFarms = farms.OrderBy(o=>o.AI_ProductionValue).ToList();
			foreach(Building b in sortedFarms)
				b.ProduceUnit(Name, this);
		}
		
		private void AssignBehavior()
		{
			// Should decide dynamically which global AI behavior to use
			foreach(Unit u in Units)
			{
				if(AI_Behavior == Constants.AI_BEHAVIOR_DEFENSE) {
					if(u.Type == Constants.UNIT_TYPE_FARMER)
						u.Behavior = Constants.UNIT_AI_BEHAV_ATTACK;
					else if(u.Type == Constants.UNIT_TYPE_MONK)
						u.Behavior = Constants.UNIT_AI_BEHAV_CAPTURE;
					else if(u.Type == Constants.UNIT_TYPE_SAMURAI)
						u.Behavior = Constants.UNIT_AI_BEHAV_DEFEND;
				} else if(AI_Behavior == Constants.AI_BEHAVIOR_OFFENSE) {
					if(u.Type == Constants.UNIT_TYPE_FARMER)
						u.Behavior = Constants.UNIT_AI_BEHAV_DEFEND;
					else if(u.Type == Constants.UNIT_TYPE_MONK)
						u.Behavior = Constants.UNIT_AI_BEHAV_CAPTURE;
					else if(u.Type == Constants.UNIT_TYPE_SAMURAI)
						u.Behavior = Constants.UNIT_AI_BEHAV_ATTACK;
				}
			}
		}
		#endregion
		
		#region Util Methods for AI decisions

		private int GetDistanceValue(Vector2i destination, Vector2i origin)
		{
			return ((destination.X - origin.X)*(destination.X - origin.X)) + ((destination.Y - origin.Y)*(destination.Y - origin.Y));
		}
		
		private int GetUnitTypeValue(Unit u)
		{
			return (u.AttackDamage + u.Armor - u.LifePoints);
		}
		
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
		
		private Unit TrySelectUnit()
		{
			foreach(Unit unit in Units)
			{
				if(unit.IsMovable())
					return unit;
			}
			return null;
		}
		
		private void Abort()
		{
			ActiveUnit.Move_RadiusLeft = 0;
			ActiveUnit.IsActive = false;
			AI_State = Constants.AI_STATE_WAITING; 
		}
		#endregion
	}
}

