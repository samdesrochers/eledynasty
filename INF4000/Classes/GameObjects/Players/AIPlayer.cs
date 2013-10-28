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
		private int AI_State;
		private int AI_Action;
		private int AI_Behavior;
		
		/* AI Destination Variables */
		private List<AIState> Candidates;
		
		public bool IsTurnOver;
		
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
			AI_Behavior = Constants.AI_BEHAVIOR_ALL_OFFENSE_DEBUG;
			
			IsTurnOver = false;
		}
		
		public override void Reset() 
		{
			base.Reset();
			AI_State = Constants.AI_STATE_BEGIN_TURN;
			IsTurnOver = false;
		}
		
		public override void Update ()
		{
			base.Update ();		
			CheckTurnOver();
			
			if( AI_State == Constants.AI_STATE_WAITING ){
				SelectUnit();
			} else if( AI_State == Constants.AI_STATE_UNIT_SELECTED ) {
				SetActions();
			} else if( AI_State == Constants.AI_STATE_ACTIONS_PREPARED ) {
				LaunchActionOnSelectedUnit();
			} else if ( AI_State == Constants.AI_STATE_EXECUTING_ACTION ) {
				// Wait
			} else if( AI_State == Constants.AI_STATE_BEGIN_TURN ) {
				BeginTurn();
			} else if( AI_State == Constants.AI_STATE_END_TURN ) {
				EndTurn();
			}
		}
		
		public void NextAction()
		{
			AI_State = Constants.AI_STATE_ACTIONS_PREPARED;
		}
		
		#region AI Methods
		private void CheckTurnOver()
		{
			int counter = 0;
			foreach(Unit u in Units) {
				if( !u.IsMovable() )
					counter ++;
				if( u.AI_Actions != null && u.AI_Actions.Count > 0 )
					counter --;
			}
			
			if(counter == Units.Count)
				AI_State = Constants.AI_STATE_END_TURN;
		}
		
		private void BeginTurn()
		{
			GameScene.Instance.GameUI.SetNoneVisible();
			AssignBehavior();
			//TryProduce();
			AI_State = Constants.AI_STATE_WAITING;
		}
		
		private void EndTurn()
		{
			IsTurnOver = true;
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
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SELECT_ATTACK);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_ATTACK);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);
			} else if(ActiveUnit.Behavior == Constants.UNIT_AI_BEHAV_DEFEND) {
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SELECT_DEFEND);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);
			} else if(ActiveUnit.Behavior == Constants.UNIT_AI_BEHAV_CAPTURE) {
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SELECT_CAPTURE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_CAPTURE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);
			} else {
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);
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
				case Constants.AI_ACTION_SELECT_ATTACK:
					SelectDestination_Attack();
					break;
				case Constants.AI_ACTION_SELECT_DEFEND:
					SelectDestination_Defend();
					break;
				case Constants.AI_ACTION_SELECT_CAPTURE:
					SelectDestination_Capture();
					break;
				case Constants.AI_ACTION_SLEEP:
					SleepAction();
					break;	
				default:
					Abort();
					break;
			}
		}
		
		private void MoveAction()
		{
			if(Candidates != null && Candidates.Count > 0)
			{
				bool pathFound = false;
				while(Candidates.Count > 0 && !pathFound)
				{
					AIState state = Candidates.First();
					pathFound = GameActions.AI_MoveUnitTo(state.Position, ActiveUnit);
					if(state.Position.X == ActiveUnit.Position.X && state.Position.Y == ActiveUnit.Position.Y) {
						FinalizeMovePreparation();
					} else if (!pathFound) {
						Candidates.Remove(state);
					} else if (pathFound) {
						FinalizeMovePreparation();
					}
				}
			}
			else { // Random - ish
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
		}
		
		private void AttackAction()
		{
			Console.WriteLine("Attacking ");
			Tile origin = GameScene.Instance.CurrentMap.GetTile(ActiveUnit.WorldPosition);
			Candidates.Clear();
			
			foreach(Vector2i p in origin.AdjacentPositions)
				Candidates.Add( new AIState(){ Position = GameScene.Instance.CurrentMap.GetTile(p).WorldPosition } );
			
			foreach(AIState a in Candidates)
			{
				Tile t = GameScene.Instance.CurrentMap.GetTile(a.Position);
				if(t != null && t.CurrentUnit != null && t.CurrentUnit.OwnerName != this.Name) {			
					GameActions.AttackUnit(ActiveUnit, t.CurrentUnit, t, origin);
					AI_State = Constants.AI_STATE_EXECUTING_ACTION;
					break;
				}
			}
		}
		
		private void CaptureAction()
		{
			Console.WriteLine("Capturing ");
		}
		
		private void SleepAction()
		{
			Console.WriteLine("Sleeping ");
			if(ActiveUnit != null)
				ActiveUnit.AI_Sleep();
			else
				Console.WriteLine("Error with inactive unit - at SleepAction ");
			AI_State = Constants.AI_STATE_WAITING;
		}
		
		#endregion	
		
		#region AI Inner Methodd
		
		private void FinalizeMovePreparation()
		{
			GameScene.Instance.UpdateCameraPositionBySelectedUnit();		
			AI_State = Constants.AI_STATE_EXECUTING_ACTION;
		}
		
		private void SelectDestination_Attack()
		{
			Console.WriteLine("Selecting target location");
			
			// Get all of the human units
			List<Unit> enemyUnits = Utilities.GetHumanPlayer().Units;
			
			// Calculate initial distance value. We want to attack the enemy closer to the current unit
			foreach(Unit u in enemyUnits)
				u.Heuristic = GetDistanceValue(u.WorldPosition, ActiveUnit.WorldPosition);
			
			List<Unit> sortedEnemyUnits = enemyUnits.OrderBy(o=>o.Heuristic).ToList();
			Unit nearestUnit = sortedEnemyUnits[0];
			
			Candidates = GenerateAttackDestinationsFromSeed(nearestUnit.WorldPosition);
			Candidates.ForEach(c => AssignAttackHeuristic(c, nearestUnit));
			Candidates = Candidates.OrderBy(o=>o.Heuristic).ToList();
			
			NextAction();
		}
		
		private void SelectDestination_Defend()
		{
			Console.WriteLine("Selecting defend location");
			NextAction();
		}
		
		private void SelectDestination_Capture()
		{
			Console.WriteLine("Selecting capture location");
			NextAction();
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
					
					
				} else if(AI_Behavior == Constants.AI_BEHAVIOR_ALL_OFFENSE_DEBUG) {
					if(u.Type == Constants.UNIT_TYPE_FARMER)
						u.Behavior = Constants.UNIT_AI_BEHAV_ATTACK;
					else if(u.Type == Constants.UNIT_TYPE_MONK)
						u.Behavior = Constants.UNIT_AI_BEHAV_ATTACK;
					else if(u.Type == Constants.UNIT_TYPE_SAMURAI)
						u.Behavior = Constants.UNIT_AI_BEHAV_ATTACK;
				} else if(AI_Behavior == Constants.AI_BEHAVIOR_ALL_DEFENSE_DEBUG) {
					if(u.Type == Constants.UNIT_TYPE_FARMER)
						u.Behavior = Constants.UNIT_AI_BEHAV_DEFEND;
					else if(u.Type == Constants.UNIT_TYPE_MONK)
						u.Behavior = Constants.UNIT_AI_BEHAV_DEFEND;
					else if(u.Type == Constants.UNIT_TYPE_SAMURAI)
						u.Behavior = Constants.UNIT_AI_BEHAV_DEFEND;
				} else if(AI_Behavior == Constants.AI_BEHAVIOR_ALL_CAPTURE_DEBUG) {
					if(u.Type == Constants.UNIT_TYPE_FARMER)
						u.Behavior = Constants.UNIT_AI_BEHAV_CAPTURE;
					else if(u.Type == Constants.UNIT_TYPE_MONK)
						u.Behavior = Constants.UNIT_AI_BEHAV_CAPTURE;
					else if(u.Type == Constants.UNIT_TYPE_SAMURAI)
						u.Behavior = Constants.UNIT_AI_BEHAV_CAPTURE;
				}
			}
		}
		
		private void AssignAttackHeuristic(AIState state, Unit enemy)
		{	
			// Remember : we want to minimize the heuristic value
			Tile t = GameScene.Instance.CurrentMap.GetTile(state.Position);
			state.Heuristic += GetDistanceValue(state.Position, ActiveUnit.WorldPosition);
			state.Heuristic += GetUnitOnTileValue(t);
			// state.Heuristic += GetBuildingOnTileValue_Attack(t);
			state.Heuristic += GetAttackStatisticsValue(t, enemy);
			state.Heuristic += GetCanAttackWithoutMovingValue(t, enemy);
		}
		
		#endregion
		
		#region Heuristics methods
		
		/* ---------------- GENERAL HEURISTICS --------------- */
		private int GetDistanceValue(Vector2i destination, Vector2i origin)
		{
			return System.Math.Abs(destination.X - origin.X) + System.Math.Abs(destination.Y - origin.Y);
		}
		
		private int GetUnitTypeValue(Unit u)
		{
			return (u.AttackDamage + u.Armor - u.LifePoints);
		}
		
		/* ---------------- ATTACK HEURISTICS --------------- */
		private int GetUnitOnTileValue(Tile t)
		{
			if(t.CurrentUnit != null && t.WorldPosition != ActiveUnit.WorldPosition)
				return 100; // very bad
			return 0;
		}
		
		private int GetBuildingOnTileValue_Attack(Tile t)
		{	// Offensive stance
			if(t.CurrentBuilding != null && t.CurrentBuilding.OwnerName == this.Name)
				return -5;
			else if(t.CurrentBuilding != null && t.CurrentBuilding.OwnerName != this.Name)
				return -10;
			return 0;
		}
		
		private int GetAttackStatisticsValue(Tile t, Unit enemy)
		{
			return - ( ActiveUnit.AttackDamage + ActiveUnit.LifePoints + ActiveUnit.Armor 
			              - ( enemy.LifePoints + enemy.AttackDamage + t.Defense + enemy.Armor) );
		}
		
		private int GetCanAttackWithoutMovingValue(Tile t, Unit enemy)
		{
			if(t.AdjacentPositions.Any(a => a == enemy.WorldPosition))
				return -100;
			return 0;
		}
		
		#endregion
		
		#region Util Methods 
		
		public void AddMoveAfterWin(Vector2i destination)
		{
			Candidates.Clear();
			Candidates.Add(new AIState(){Position = destination});
			ActiveUnit.AI_Actions.Clear();
			ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
			ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);
		}
		
		private List<AIState> GenerateAttackDestinationsFromSeed(Vector2i seed)
		{
			List<AIState> candidates = new List<AIState>();
			candidates.Add(new AIState(){ Position = new Vector2i(seed.X - 1, seed.Y) });
			candidates.Add(new AIState(){ Position = new Vector2i(seed.X + 1, seed.Y) });
			candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y + 1) });
			candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y - 1) });
			candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y) });
			
			// Clean up
			for( int i = candidates.Count - 1; i >= 0; i-- ) {
				AIState s = candidates[i];
				Vector2i v = s.Position;
				if(v.X < 0 || v.X > GameScene.Instance.CurrentMap.Width - 1
				   || v.Y < 0 || v.X > GameScene.Instance.CurrentMap.Height - 1)
					candidates.Remove(s);
			}
			return candidates;	
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

