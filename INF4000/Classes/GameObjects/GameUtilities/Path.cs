using System;
using System.Linq;
using Sce.PlayStation.Core;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Path
	{
		public Queue<string> Sequence;
		public Queue<string> CompleteSequence;
		
		const int TickPerStep = Constants.PATH_TICKS; // arbitrary value
		const int MovePerStep = Constants.PATH_STEP;  // 8*8 = 64 pixels to move per step

		public int currentTick = TickPerStep;
		
		public event EventHandler PathCompleted;
		public bool IsActive;
		public int distanceMoved;
		public int RadiusUsed;
		public Vector2i Origin;
		
		public List<AIState> Visited;
		
		public Path()
		{
			Sequence = new Queue<string>();
			CompleteSequence = new Queue<string>();
			IsActive = false;
			distanceMoved = 0;
			Visited = new List<AIState>();
		}
		
		public void BuildMoveToSequence(Vector2i origin, Vector2i destination)
		{
			Origin = origin;
			Vector2i finalPos = origin; 
			while(destination.X != finalPos.X || destination.Y != finalPos.Y)
			{
				if(destination.X > finalPos.X)
				{
					Sequence.Enqueue(Constants.PATH_RIGHT);
					finalPos = new Vector2i(finalPos.X + 1, finalPos.Y);
				}
				else if(destination.X < finalPos.X)
				{
					Sequence.Enqueue(Constants.PATH_LEFT);
					finalPos = new Vector2i(finalPos.X - 1, finalPos.Y);
				}
				
				if(destination.Y > finalPos.Y)
				{
					Sequence.Enqueue(Constants.PATH_UP);
					finalPos = new Vector2i(finalPos.X, finalPos.Y + 1);
				}
				else if(destination.Y < finalPos.Y)
				{
					Sequence.Enqueue(Constants.PATH_DOWN);
					finalPos = new Vector2i(finalPos.X, finalPos.Y - 1);
				}
			}
			RadiusUsed = Sequence.Count;
			IsActive = true;
			
			SoundManager.Instance.PlaySound(Constants.SOUND_UNIT_MARCH);
		}
		
		public bool AI_BuildMoveToSequence(Vector2i origin, Vector2i destination, int movePoints)
		{
			if(movePoints == 0 || (origin.X == destination.X && origin.Y == destination.Y)) {
				IsActive = true;		
				SoundManager.Instance.PlaySound(Constants.SOUND_UNIT_MARCH);
				return true;
			}
			
			Origin = origin;
			Vector2i finalPos = origin;
			int availableMovePoints = movePoints;
			
			Tile originTile = Utilities.GetTile(origin);
			List<AIState> candidates = new List<AIState>();
			
			// Assign heuristic value (manhattan). Also filters tiles where move is impossible
			foreach(Vector2i v in originTile.AdjacentPositions)
			{
				Tile can = Utilities.GetTile(v);
				if(can != null && can.IsMoveValid && Visited.FindIndex(s => (s.Position.X == can.WorldPosition.X && s.Position.Y == can.WorldPosition.Y)) < 0 ) {
					AIState s = new AIState();
					s.Position = can.WorldPosition;
					s.IsOccupied = (can.CurrentUnit != null) ? true : false;
					s.Heuristic = GetManhattanValue(destination, s.Position);
					candidates.Add(s);
				}			
			}
			
			candidates = candidates.OrderBy(o=>o.Heuristic).ToList();	
			while(candidates.Count > 0) {
				bool legalCandidatePicked = false;
				AIState candidate = null;
				while (!legalCandidatePicked) {
					
					if(candidates.Count == 0 && Sequence.Count != 0) {
						IsActive = true;
						return true;
					}
					
					candidate = candidates.First();	
					if ( candidate.Position.X == destination.X && candidate.Position.Y == destination.Y && !candidate.IsOccupied  ) {
						legalCandidatePicked = true; // Found a match for destination		
					} else if(candidate.IsOccupied && candidate.Position.X == destination.X && candidate.Position.Y == destination.Y) {
						candidates.Remove(candidate);
					} else {
						legalCandidatePicked = true;
					}
				}
				
				Visited.Add(candidate);
				
				if(candidate.Position.X > origin.X) {
					Sequence.Enqueue(Constants.PATH_RIGHT);
					candidates.Remove(candidate);
					availableMovePoints --;
					AI_BuildMoveToSequence(candidate.Position, destination, availableMovePoints);
					return true;
				} else if(candidate.Position.X < origin.X) {
					Sequence.Enqueue(Constants.PATH_LEFT);
					candidates.Remove(candidate);
					availableMovePoints --;
					AI_BuildMoveToSequence(candidate.Position, destination, availableMovePoints);
					return true;
				}
				if(candidate.Position.Y > origin.Y) {
					Sequence.Enqueue(Constants.PATH_UP);
					candidates.Remove(candidate);
					availableMovePoints --;
					AI_BuildMoveToSequence(candidate.Position, destination, availableMovePoints);
					return true;
				} else if(candidate.Position.Y < origin.Y) {
					Sequence.Enqueue(Constants.PATH_DOWN);
					candidates.Remove(candidate);
					availableMovePoints --;
					AI_BuildMoveToSequence(candidate.Position, destination, availableMovePoints);
					return true;
				}
			}
			
			return false;
		}
		
		public bool AI_BuildMoveToCaptureSequence(Vector2i origin, Vector2i destination, int movePoints)
		{
			if(origin.X == destination.X && origin.Y == destination.Y) {
				IsActive = true;		
				SoundManager.Instance.PlaySound(Constants.SOUND_UNIT_MARCH);
				return true;
			}
			
			// Total heuristic
			if(CompleteSequence.Count > 7) {
				IsActive = true;		
				SoundManager.Instance.PlaySound(Constants.SOUND_UNIT_MARCH);
				return true;
			}
			
			Origin = origin;
			Vector2i finalPos = origin;
			
			Tile originTile = Utilities.GetTile(origin);
			List<AIState> candidates = new List<AIState>();
			
			// Assign heuristic value (manhattan). Also filters tiles where move is impossible
			foreach(Vector2i v in originTile.AdjacentPositions)
			{
				Tile can = Utilities.GetTile(v);
				if(can != null && can.IsMoveValid && Visited.FindIndex(s => (s.Position.X == can.WorldPosition.X && s.Position.Y == can.WorldPosition.Y)) < 0 ) {
					AIState s = new AIState();
					s.Position = can.WorldPosition;
					s.IsOccupied = (can.CurrentUnit != null) ? true : false;
					s.Heuristic = GetManhattanValue(destination, s.Position);
					candidates.Add(s);
				}			
			}
			
			candidates = candidates.OrderBy(o=>o.Heuristic).ToList();	
			while(candidates.Count > 0) {
				bool legalCandidatePicked = false;
				AIState candidate = null;
				while (!legalCandidatePicked) {
					
					if(candidates.Count == 0 && Sequence.Count != 0) {
						IsActive = true;
						return true;
					}
					
					if(candidates.Count > 0)
						candidate = candidates.First();
					else {
						Visited.Clear();
						Sequence.Clear();
						CompleteSequence.Clear();
						AI_BuildMoveToSequence(origin, destination, movePoints);
					}
					
					if ( candidate.Position.X == destination.X && candidate.Position.Y == destination.Y && !candidate.IsOccupied  ) {
						legalCandidatePicked = true; // Found a match for destination		
					} else if(candidate.IsOccupied && candidate.Position.X == destination.X && candidate.Position.Y == destination.Y) {
						candidates.Remove(candidate);
					} else {
						legalCandidatePicked = true;
					}
				}
				
				Visited.Add(candidate);
				
				if(candidate.Position.X > origin.X) {
					CompleteSequence.Enqueue(Constants.PATH_RIGHT);
					candidates.Remove(candidate);
					AI_BuildMoveToCaptureSequence(candidate.Position, destination, movePoints);
					return true;
				} else if(candidate.Position.X < origin.X) {
					CompleteSequence.Enqueue(Constants.PATH_LEFT);
					candidates.Remove(candidate);
					AI_BuildMoveToCaptureSequence(candidate.Position, destination, movePoints);
					return true;
				}
				if(candidate.Position.Y > origin.Y) {
					CompleteSequence.Enqueue(Constants.PATH_UP);
					candidates.Remove(candidate);
					AI_BuildMoveToCaptureSequence(candidate.Position, destination, movePoints);
					return true;
				} else if(candidate.Position.Y < origin.Y) {
					CompleteSequence.Enqueue(Constants.PATH_DOWN);
					candidates.Remove(candidate);
					AI_BuildMoveToCaptureSequence(candidate.Position, destination, movePoints);
					return true;
				}
			}
			
			return false;
		}
				
		public int GetDestinationAction(Vector2i pos, Vector2i origin)
		{			
			Tile dest = GameScene.Instance.CurrentMap.SelectTileFromPosition(pos);
			
			// If tile not in active tiles abort
			if(!GameScene.Instance.CurrentMap.ActiveTiles.Contains(dest))
				return Constants.ACTION_CANCEL;

			// User is trying to move unit on another of his own unit
			else if(dest.CurrentUnit != null && dest.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name && pos != origin)
				return Constants.ACTION_CANCEL;
			
			// If tile is empty, move is necessarliy valid
			else if(dest.CurrentUnit == null && !Utilities.CanUnitAttackFromDestination(dest))
				return Constants.ACTION_MOVE;
			
			// User is trying to attack enemy unit
			else if(dest.CurrentUnit == null && Utilities.CanUnitAttackFromDestination(dest))
				return Constants.ACTION_ATTACK;
			
			return Constants.ACTION_CANCEL;
		}
		
		public void Update()
		{
			try {
			if(Sequence.Count > 0 && IsActive)
			{	
				if(currentTick == 0)
				{
					Sequence.Dequeue();
					currentTick = TickPerStep;
				}
				currentTick --;
			} 
			else if(IsActive && Sequence.Count == 0)
			{
				OnPathCompleted();
			}
			} catch (Exception e) {Console.WriteLine(e.Message + "Path error");}
		}
		
		private void OnPathCompleted()
		{
			EventHandler handler = PathCompleted;
			if(handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
		
		#region AI Methods
		private int GetManhattanValue(Vector2i destination, Vector2i origin)
		{
			return System.Math.Abs(destination.X - origin.X) + System.Math.Abs(destination.Y - origin.Y);
		}
		#endregion
	}
}

