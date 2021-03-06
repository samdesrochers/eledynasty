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
			this.SpellPoints = 0;
			this.Gold = 5;
			
			this.TargetUnits = new List<Unit>();
			AI_State = Constants.AI_STATE_BEGIN_TURN;
			AI_Action = Constants.AI_ACTION_NONE;
			
			IsTurnOver = false;
		}
		
		public override void Reset() 
		{
			base.Reset();
			AI_State = Constants.AI_STATE_BEGIN_TURN;
			IsTurnOver = false;
		}
		
		float waitTimer = 0;
		
		public override void Update (float dt)
		{
			base.Update (dt);		
			CheckTurnOver();
			
			if( AI_State == Constants.AI_STATE_WAITING ){
				if(waitTimer > 0.2f) {
					SelectUnit();
					waitTimer = 0;
				}
				waitTimer += dt;
			} else if( AI_State == Constants.AI_STATE_UNIT_SELECTED ) {
				AssignBehavior();
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
			Utilities.ShowAIPlayerPanel();		
			
			TryProduce();
			AI_State = Constants.AI_STATE_WAITING;
		}
		
		private void EndTurn()
		{
			TryProduce();
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
		
		private void FlushHeuristics()
		{
			foreach(Building b in Utilities.AI_GetEnemyBuildings())
				b.Heuristic = 0;
			
			foreach(Unit u in Units)
				u.Heuristic = 0;
			
			foreach(Unit u in Utilities.GetHumanPlayer().Units)
				u.Heuristic = 0;
		}
		
		private void AssignBehavior()
		{
			FlushHeuristics();
							
			ActiveUnit.Behavior = Constants.UNIT_AI_BEHAV_DECIDING;
		
			// Get decision Heuristics
			List<Tuple<int,int>> heuristics = new List<Tuple<int, int>>();
			Tuple<int,int> attack = new Tuple<int,int>(GetAttackDecisionHeuristic(), Constants.UNIT_AI_BEHAV_ATTACK);
			Tuple<int,int> defend = new Tuple<int,int>(GetDefendDecisionHeuristic(), Constants.UNIT_AI_BEHAV_DEFEND);
			Tuple<int,int> capture = new Tuple<int,int>(GetCaptureDecisionHeuristic(), Constants.UNIT_AI_BEHAV_CAPTURE);
			
			heuristics.Add(attack);
			heuristics.Add(defend);
			heuristics.Add(capture);

			// Cap = 2, Def = 1, Att = 0
			heuristics.Sort((x, y) => y.Item1.CompareTo(x.Item1));
			Tuple<int,int> choice = heuristics.Last();
			ActiveUnit.Behavior = choice.Item2;
				
			FlushHeuristics();
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
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVECAPTURE);
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
				case Constants.AI_ACTION_MOVECAPTURE:
					MoveCaptureAction();
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
		}
		
		private void MoveCaptureAction()
		{
			if(Candidates != null && Candidates.Count > 0)
			{
				bool pathFound = false;
				while(Candidates.Count > 0 && !pathFound)
				{
					AIState state = Candidates.First();
					pathFound = GameActions.AI_MoveForCapture(ActiveUnit);
					if(state.Position.X == ActiveUnit.Position.X && state.Position.Y == ActiveUnit.Position.Y) {
						FinalizeMovePreparation();
					} else if (!pathFound) {
						Candidates.Remove(state);
					} else if (pathFound) {
						FinalizeMovePreparation();
					}
				}
			} else {
				FinalizeMovePreparation();
			}
		}
		
		private void AttackAction()
		{
			//Console.WriteLine("Attacking ");
			Tile origin = Utilities.GetTile(ActiveUnit.WorldPosition);
			Candidates.Clear();
			
			foreach(Vector2i p in origin.AdjacentPositions)
				if(p.X >= 0 && p.Y >= 0)
					Candidates.Add( new AIState(){ Position = Utilities.GetTile(p).WorldPosition } );
			
			foreach(AIState a in Candidates)
			{
				Tile t = Utilities.GetTile(a.Position);
				if(t != null && t.CurrentUnit != null && t.CurrentUnit.OwnerName != this.Name) {			
					GameActions.AttackUnit(ActiveUnit, t.CurrentUnit, t, origin);
					AI_State = Constants.AI_STATE_EXECUTING_ACTION;
					break;
				}
			}
		}
		
		private void CaptureAction()
		{
			//Console.WriteLine("Capturing ");				
			if(Candidates.Count == 1) { // Only the "building" State was inherited from
				NextAction(); // Skip
			} else {
				Tile origin = Utilities.GetTile(ActiveUnit.WorldPosition);
				Candidates.Clear();
				
				foreach(Vector2i p in origin.AdjacentPositions)
					Candidates.Add( new AIState(){ 
						Position = p,
						Heuristic = AssignAttackAndCaptureHeuristic(p)
					} );
				
				foreach(AIState a in Candidates)
				{
					Tile t = Utilities.GetTile(a.Position);
					if(t != null && t.CurrentUnit != null && t.CurrentUnit.OwnerName != this.Name) {			
						GameActions.AttackUnit(ActiveUnit, t.CurrentUnit, t, origin);
						AI_State = Constants.AI_STATE_EXECUTING_ACTION;
						break;
					}
				}
			}
			
			if(ActiveUnit.WorldPosition.X == ActiveUnit.FinalDestination.X && ActiveUnit.WorldPosition.Y == ActiveUnit.FinalDestination.Y) {
				ActiveUnit.Path.CompleteSequence.Clear();
				ActiveUnit.Path.Visited.Clear();
			}
		}
		
		private void SleepAction()
		{
			//Console.WriteLine("Sleeping ");
			if(ActiveUnit != null) {
				ActiveUnit.AI_Sleep();
			} else {
				Console.WriteLine("Error with inactive unit - at SleepAction ");
			}
			AI_State = Constants.AI_STATE_WAITING;
		}
		
		#endregion	
		
		#region AI Inner Method
		
		private void FinalizeMovePreparation()
		{
			GameScene.Instance.UpdateCameraPositionBySelectedUnit();		
			AI_State = Constants.AI_STATE_EXECUTING_ACTION;
		}
		
		private void SelectDestination_Attack()
		{
			//Console.WriteLine("Selecting target location");
			
			// Get all of the human units
			List<Unit> enemyUnits = Utilities.GetHumanPlayer().Units;
		
			// Calculate initial heuristic
			foreach(Unit u in enemyUnits) {
				
				u.Heuristic = 0;
				Tile t = Utilities.GetTile(u.WorldPosition);
				
				if(t.CurrentBuilding != null)
					u.Heuristic += GetBuidlingTypeValue_Capture(t.CurrentBuilding);
				
				u.Heuristic += GetDistanceValue(u.WorldPosition, ActiveUnit.WorldPosition);
				u.Heuristic += GetAttackStatisticsValue(t, u);
				
				foreach(Building b in Buildings) {
					Tile tb = Utilities.GetTile(b.WorldPosition);
					if(b.Type == Constants.BUILD_FORT) {
						if(tb.CurrentUnit != null && tb.CurrentUnit.UniqueId == u.UniqueId) {
							u.Heuristic += -10;
							break;
						}
						foreach(Vector2i v in tb.AdjacentPositions) { // Enemy near the fort
							Tile a = Utilities.GetTile(v);
							if(a.CurrentUnit != null && a.CurrentUnit.UniqueId == u.UniqueId) {
								u.Heuristic += -10;
								break;
							}
						}		
					}
				}
			}
			
			List<Unit> sortedEnemyUnits = enemyUnits.OrderBy(o=>o.Heuristic).ToList();
			
			int unitIndex = 0;
			bool destinationFound = false;
			
			// Try the currently selected unit destination as potential move target
			while( !destinationFound && unitIndex < enemyUnits.Count ) {
				Unit bestPick = sortedEnemyUnits[unitIndex];
				
				Candidates = GenerateAttackDestinationsFromSeed(bestPick.WorldPosition);
				if(Candidates.Count > 0) {
					Candidates.ForEach(c => AssignAttackHeuristic(c, bestPick));
					Candidates = Candidates.OrderBy(o=>o.Heuristic).ToList();
					destinationFound = true;
				} else {
					unitIndex ++;
				}
			}
			
			NextAction();
		}
		
		private void SelectDestination_Defend()
		{
			//Console.WriteLine("Selecting defend location");
			
			// Get all of our buildings
			List<Building> AIBuildings = this.Buildings;
			List<Tile> candidateTiles = new List<Tile>();
			List<AIState> AIDestinations = new List<AIState>();		
			
			// Assign decision heuristics
			foreach(Building b in AIBuildings) {
				if(!HasUnit(b.WorldPosition)){
					AIState s = new AIState();
					s.Position = b.WorldPosition;
					s.Heuristic += GetDistanceValue(b.WorldPosition, ActiveUnit.WorldPosition);
					s.Heuristic += GetBuidlingTypeValue_Defense(b);
					s.Heuristic += GetReachableThisTurnValue(b.WorldPosition, ActiveUnit.WorldPosition, ActiveUnit.Move_RadiusLeft);
					AIDestinations.Add(s);
				}
			}
			
			candidateTiles = Utilities.AI_GetCandidateMoveTiles(ActiveUnit.WorldPosition, ActiveUnit.Move_RadiusLeft);
			foreach(Tile t in candidateTiles)
			{
				if(!HasUnit(t.WorldPosition)){
					AIState s = new AIState();
					s.Position = t.WorldPosition;
					s.Heuristic += GetDistanceValue(t.WorldPosition, ActiveUnit.WorldPosition);
					s.Heuristic += GetTileTypeValue_Defense(t);
					s.Heuristic += GetReachableThisTurnValue(t.WorldPosition, ActiveUnit.WorldPosition, ActiveUnit.Move_RadiusLeft);				
					AIDestinations.Add(s);
				}
			}
			
			AIDestinations = AIDestinations.OrderBy(o=>o.Heuristic).ToList();
			
			int tileIndex = 0;
			bool destinationFound = false;
			
			// Try the currently selected building as potential move target
			while( !destinationFound && tileIndex < AIDestinations.Count ) {
				AIState bestPick = AIDestinations[tileIndex];
				
				Candidates = GenerateDefendDestinationsFromSeed(bestPick.Position);
				if(Candidates.Count > 0) {
					destinationFound = true;
				} else {
					tileIndex ++;
				}
			}
			
			// Check if unit is already on optimal tile; if not, it will need to move
			if(Candidates.Count > 0 && IsOnTile_Defense(ActiveUnit.WorldPosition, Candidates.First().Position)) { 
				ActiveUnit.AI_Actions.Clear();
				ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);		
			}
			
			NextAction();
		}
		
		private void SelectDestination_Capture()
		{
			//Console.WriteLine("Selecting capture location");
			
			// Get all of the human buildings
			List<Building> enemyBuildings = Utilities.AI_GetEnemyBuildings();
			List<AIState> AIDestinations = new List<AIState>();	
			
			// Assign decision heuristics
			foreach(Building b in enemyBuildings) {
				AIState s = new AIState();
				s.Position = b.WorldPosition;
				s.Heuristic += 2*GetDistanceValue(b.WorldPosition, ActiveUnit.WorldPosition);
				s.Heuristic += GetBuidlingTypeValue_Capture(b);
				s.Heuristic += GetBuidlingOccupied_Capture(b);
				s.Heuristic += GetReachableThisTurnValue(b.WorldPosition, ActiveUnit.WorldPosition, ActiveUnit.Move_RadiusLeft);
				AIDestinations.Add(s);
			}
			
			AIDestinations = AIDestinations.OrderBy(o=>o.Heuristic).ToList();
			
			int buildIndex = 0;
			bool destinationFound = false;
			
			// Try the currently selected building as potential move target
			while( !destinationFound && buildIndex < AIDestinations.Count ) {
				AIState bestPick = AIDestinations[buildIndex];
				
				Candidates = GenerateCaptureDestinationsFromSeed(bestPick.Position);
				if(Candidates.Count > 0) {
					if(Candidates.Count > 1) { // More than one candidate means we're going all out war to capture
						ActiveUnit.AI_Actions.Clear();
						ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_MOVE);
						ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_ATTACK);
						ActiveUnit.AI_Actions.Enqueue(Constants.AI_ACTION_SLEEP);
						SelectDestination_Attack();
					} 
					ActiveUnit.FinalDestination = bestPick.Position;
					destinationFound = true;
				} else {
					buildIndex ++;
				}
			}
			
			NextAction();
		}
		
		private void TryProduce()
		{
			Random decider = new Random();
			
			int gamble = decider.Next(1,100);
			bool shouldSpend = false;
			
			if(gamble > 80 && this.Gold > 10)
				shouldSpend = true;
			if(gamble > 50 && this.Gold > 20)
				shouldSpend = true;
			if(gamble > 20 && this.Gold > 40)
				shouldSpend = true;
			if(gamble < 90 && this.Units.Count < 3)
				shouldSpend = true;	
			
			// Last Stand - Make it rain
			foreach(Building b in Buildings) {
				if(b.Type == Constants.BUILD_FORT) {
					Tile t = Utilities.GetTile(b.WorldPosition);
					foreach(Vector2i v in t.AdjacentPositions) {
						Tile enemy = Utilities.GetTile(v);
						if(enemy.CurrentUnit != null && enemy.CurrentUnit.OwnerName == Constants.CHAR_KENJI) {
							shouldSpend = true;
							break;
						}						
					}
				}
			}
			
			if(shouldSpend) {				
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
		}
		
		private void AssignAttackHeuristic(AIState state, Unit enemy)
		{	
			// Remember : we want to minimize the heuristic value
			Tile t = Utilities.GetTile(state.Position);
			state.Heuristic += GetDistanceValue(state.Position, ActiveUnit.WorldPosition);
			state.Heuristic += GetUnitOnTileValue(t);
			// state.Heuristic += GetBuildingOnTileValue_Attack(t);
			state.Heuristic += GetAttackStatisticsValue(t, enemy);
			state.Heuristic += GetCanAttackWithoutMovingValue(Utilities.GetTile(ActiveUnit.WorldPosition), enemy, state);
			state.Heuristic += GetReachableThisTurnValue(state.Position, ActiveUnit.WorldPosition, ActiveUnit.Move_RadiusLeft);
		}
		
		private int AssignAttackAndCaptureHeuristic(Vector2i pos)
		{	
			// Remember : we want to minimize the heuristic value
			Tile t = Utilities.GetTile(pos);
			return GetBuildingOnTileValue_Attack(t);
		}
		
		private int GetCaptureDecisionHeuristic()
		{
			int heuristic = 0;
			
			// Check if other units are set to capture
			int counter = 0;
			foreach(Unit u in Units)
				if(u.Behavior == Constants.UNIT_AI_BEHAV_CAPTURE)
					counter ++;
			
			int toCapture = (counter > 2) ? 8 : -8;
			
			// Check for active unit type
			int activeType = 0;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_FARMER)
					activeType = -1;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_MONK)
					activeType = -10;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_SAMURAI)
					activeType = -3;
			
			if(ActiveUnit.LifePoints < 4)
				heuristic += 15;
			
			// Path computed already in motion
			if(ActiveUnit.Path != null && ActiveUnit.Path.CompleteSequence != null)
				heuristic -= 10;
			
			// Check for shortest distance to any building
			int bHeuristic = 0;
			List<int> distances = new List<int>();
			foreach(Building b in Utilities.AI_GetEnemyBuildings())
			{
				b.Heuristic = GetDistanceValue(b.WorldPosition, ActiveUnit.WorldPosition);
				if(b.Type == Constants.BUILD_FORT)
					if(Utilities.GetTile(b.WorldPosition).CurrentUnit == null )
						b.Heuristic += 3*GetReachableThisTurnValue(b.WorldPosition, ActiveUnit.WorldPosition,ActiveUnit.Move_RadiusLeft);
				distances.Add(b.Heuristic);
			}
			distances.Sort();
			bHeuristic = distances.First();
			
			heuristic = toCapture + activeType + bHeuristic;
			return heuristic;
		}
		
		private int GetAttackDecisionHeuristic()
		{
			int heuristic = 0;
			
			// Check if other units are set to attack
			int counter = 0;
			foreach(Unit u in Units)
				if(u.Behavior == Constants.UNIT_AI_BEHAV_ATTACK)
					counter ++;
			
			int toCapture = (counter > 4) ? 3 : -7;
			
			// Check for active unit type
			int activeType = 0;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_FARMER)
					activeType = -9;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_MONK)
					activeType = -4;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_SAMURAI)
					activeType = -6;
			
			// Check for an enemy unit on the AI fort
			foreach(Building b in Buildings)
			{
				if(b.Type == Constants.TILE_TYPE_BUILD_FORT || b.Type == Constants.TILE_TYPE_BUILD_FORT_2) {
					Tile t = Utilities.GetTile(b.WorldPosition);
					if(t.CurrentUnit != null && t.CurrentUnit.OwnerName != this.Name) { // Enemy on fort, rush him
						heuristic -= 10;
					}
					foreach(Vector2i v in t.AdjacentPositions) { // Enemy near the fort
						Tile a = Utilities.GetTile(v);
						if(a.CurrentUnit != null && a.CurrentUnit.OwnerName != this.Name) {
							heuristic -= 10;
						}
					}
				}
			}	
			
			// Check for shortest distance to any building
			int unitH = 0;
			List<int> unitHs = new List<int>();
			foreach(Unit u in Utilities.GetHumanPlayer().Units)
			{
				u.Heuristic = GetDistanceValue(u.WorldPosition, ActiveUnit.WorldPosition);
				u.Heuristic += (u.LifePoints < ActiveUnit.LifePoints) ? -8 : 0;
				//Tile c = GetPlayerFort(this);
				
				unitHs.Add(u.Heuristic);
			}
			unitHs.Sort();
			unitH = unitHs.First();
			
			heuristic = toCapture + activeType + unitH;
			return heuristic;
		}
		
		private int GetDefendDecisionHeuristic()
		{
			int heuristic = 0;
			
			// Check if other units are set to defend
			int counter = 0;
			foreach(Unit u in Units)
				if(u.Behavior == Constants.UNIT_AI_BEHAV_DEFEND)
					counter ++;
			
			int toDefend = (counter > 2) ? 5 : -1;
		 	toDefend += (counter < 1) ? -5 : 0;
			
			// Check if fort is defended
			foreach(Building b in Buildings) {
				Tile t = Utilities.GetTile(b.WorldPosition);
				if(b.Type == Constants.BUILD_FORT) {
					if(t.CurrentUnit == null) {
						heuristic += -12;
					} else if (t.CurrentUnit != null && t.CurrentUnit.UniqueId == ActiveUnit.UniqueId) { // Already defending the tile
						heuristic += -10;
					}
					break;
				}
			}
			
			// Check for active unit type
			int activeType = 0;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_FARMER)
					activeType = -3;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_MONK)
					activeType = -2;
			if(ActiveUnit.Type == Constants.UNIT_TYPE_SAMURAI)
					activeType = -6;
			
			// Check for shortest distance to any building
			int shortestDistance = 0;
			List<int> distances = new List<int>();
			foreach(Building b in Buildings)
			{
				b.Heuristic = GetDistanceValue(b.WorldPosition, ActiveUnit.WorldPosition);
				distances.Add(b.Heuristic);
			}
			distances.Sort();
			shortestDistance = distances.First();
			
			heuristic = toDefend + activeType + shortestDistance;
			return heuristic;
		}
		
		#endregion
		
		#region Heuristics methods
		
		/* ---------------- GENERAL HEURISTICS --------------- */
		private int GetDistanceValue(Vector2i destination, Vector2i origin)
		{
			return System.Math.Abs(destination.X - origin.X) + System.Math.Abs(destination.Y - origin.Y);
		}
		
		private int GetReachableThisTurnValue(Vector2i destination, Vector2i origin, int radius)
		{
			if(System.Math.Abs(destination.X - origin.X) + System.Math.Abs(destination.Y - origin.Y) <= radius)
				return -4;
			return 0;
		}
		
		private int GetUnitTypeValue(Unit u)
		{
			return (u.AttackDamage + u.Armor - u.LifePoints);
		}
		
		private bool HasUnit(Vector2i pos)
		{
			Tile t = Utilities.GetTile(pos);
			if(t.CurrentUnit != null && t.CurrentUnit.WorldPosition != ActiveUnit.WorldPosition)	// Reject defense to tile 
				return true;
			return false;
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
				return GetBuidlingTypeValue_Capture(t.CurrentBuilding) - 3;
			else if(t.CurrentBuilding != null && t.CurrentBuilding.OwnerName != this.Name)
				return GetBuidlingTypeValue_Capture(t.CurrentBuilding) - 5;
			return 0;
		}
		
		private int GetAttackStatisticsValue(Tile t, Unit enemy)
		{
			int statValue = 0;
			statValue += (enemy.Armor >= ActiveUnit.Armor) ? 2 : -4;
			statValue += (enemy.LifePoints > ActiveUnit.LifePoints) ? 10 : -5;
			statValue += (enemy.AttackDamage >= ActiveUnit.AttackDamage) ? 6 : -8;
			statValue += (enemy.AttackDamage >= ActiveUnit.AttackDamage && enemy.LifePoints > ActiveUnit.LifePoints) ? 10 : -5;
			return statValue;
		}
		
		private int GetCanAttackWithoutMovingValue(Tile t, Unit enemy, AIState s)
		{
			if(t.AdjacentPositions.Any(a => a == enemy.WorldPosition) && s.Position == ActiveUnit.WorldPosition)
				return -15;
			return 0;
		}
		
		/* ---------------- CAPTURE HEURISTICS --------------- */
		private int GetBuidlingTypeValue_Capture(Building building)
		{
			switch(building.Type) {
				case Constants.BUILD_FARM 	: return -2;
				case Constants.BUILD_FORT 	: return -15;
				case Constants.BUILD_FORGE 	: return -3;
				case Constants.BUILD_TEMPLE : return -2;
			}
			return 0;
		}
		
		private int GetBuidlingOccupied_Capture(Building building)
		{
			if(building.OwnerName == null || building.OwnerName == "")
				return -15;
			return 0;
		}
		
		/* ---------------- DEFENSE HEURISTICS --------------- */
		private int GetBuidlingTypeValue_Defense(Building building)
		{
			// Check if fort is defended
			int fortBonus = 0;
			foreach(Building b in Buildings) {
				Tile t = Utilities.GetTile(b.WorldPosition);
				if(b.Type == Constants.BUILD_FORT || b.Type == Constants.TILE_TYPE_BUILD_FORT_2) {
					if(t.CurrentUnit == null) {
						fortBonus = -15;				
					} 
					if(t.CurrentBuilding.PointsToCapture < 20) {
						fortBonus += -15;
					}
					break;
				}
			}
			
			switch(building.Type) {
				case Constants.BUILD_FARM 	: return -3;
				case Constants.BUILD_FORT 	:
				case Constants.TILE_TYPE_BUILD_FORT_2 	:
					return (-10 + fortBonus);
				case Constants.BUILD_FORGE 	: return -5;
				case Constants.BUILD_TEMPLE : return -4;
			}
			return 0;
		}
		
		private int GetTileTypeValue_Defense(Tile tile)
		{
			if(tile.CurrentBuilding != null)
				return GetBuidlingTypeValue_Defense(tile.CurrentBuilding);
			
			switch(tile.TerrainType) {
				case Constants.TILE_TYPE_HILL :
				case Constants.TILE_TYPE_HILL_2 : 			
					return -4;
				case Constants.TILE_TYPE_TREES_1 :
				case Constants.TILE_TYPE_TREES_2 :
					return -2;
			}
			return 0;
		}
		
		private bool IsOnTile_Defense(Vector2i unitPos, Vector2i candidatePos)
		{
			return (unitPos.X == candidatePos.X && unitPos.Y == candidatePos.Y);
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
				Tile t = Utilities.GetTile(v);
				if(v.X < 0 || v.X > GameScene.Instance.CurrentMap.Width - 1
				   || v.Y < 0 || v.Y > GameScene.Instance.CurrentMap.Height - 1
				   || (t.CurrentUnit != null && t.WorldPosition != ActiveUnit.WorldPosition)
				   || !t.IsMoveValid )
					candidates.Remove(s);
			}
			return candidates;	
		}
		
		private List<AIState> GenerateDefendDestinationsFromSeed(Vector2i seed)
		{
			List<AIState> candidates = new List<AIState>();
			Tile tile = Utilities.GetTile(seed);
			
			if(tile.CurrentUnit == null) {
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y) });
			} else if (IsOnTile_Defense(ActiveUnit.WorldPosition, seed)) {
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y) });
			} else {	
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X - 1, seed.Y) });
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X + 1, seed.Y) });
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y + 1) });
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y - 1) });
				
				// Remove invalid destinations
				for( int i = candidates.Count - 1; i >= 0; i-- ) {
					AIState s = candidates[i];
					Vector2i v = s.Position;
					Tile t = Utilities.GetTile(v);
					if(v.X < 0 
					   || v.X > GameScene.Instance.CurrentMap.Width - 1
					   || v.Y < 0 || v.Y > GameScene.Instance.CurrentMap.Height - 1 
					   || (t.CurrentUnit != null && t.WorldPosition != ActiveUnit.WorldPosition)
					   || !t.IsMoveValid )
						candidates.Remove(s);
				}
			}
			return candidates;
		}
		
		private List<AIState> GenerateCaptureDestinationsFromSeed(Vector2i seed)
		{
			List<AIState> candidates = new List<AIState>();
			Tile buildingTile = Utilities.GetTile(seed);
			
			if(buildingTile.CurrentUnit == null || (ActiveUnit.WorldPosition.X == seed.X && ActiveUnit.WorldPosition.Y == seed.Y)) {
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y) });
			} else {	
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X - 1, seed.Y) });
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X + 1, seed.Y) });
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y + 1) });
				candidates.Add(new AIState(){ Position = new Vector2i(seed.X, seed.Y - 1) });
				
				// Remove invalid destinations (for attack)
				for( int i = candidates.Count - 1; i >= 0; i-- ) {
					AIState s = candidates[i];
					Vector2i v = s.Position;
					Tile t = Utilities.GetTile(v);
					if(v.X < 0 
					   || v.X > GameScene.Instance.CurrentMap.Width - 1
					   || v.Y < 0 || v.Y > GameScene.Instance.CurrentMap.Height - 1 
					   || (t.CurrentUnit != null)
					   || !t.IsMoveValid )
						candidates.Remove(s);
				}
			}
			return candidates;	
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