using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Path
	{
		public Queue<string> Sequence;
		
		const int TickPerStep = Constants.PATH_TICKS; // arbitrary value
		const int MovePerStep = Constants.PATH_STEP;  // 8*8 = 64 pixels to move per step

		int currentTick = TickPerStep;
		
		public event EventHandler PathCompleted;
		public bool IsActive;
		
		public Path()
		{
			Sequence = new Queue<string>();
			IsActive = false;
		}
		
		public void BuildMoveToSequence(Vector2i origin, Vector2i destination)
		{
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
			IsActive = true;
		}
		
		public int GetDestinationAction(Vector2i pos)
		{
			Tile dest = GameScene.Instance.CurrentMap.SelectTileFromPosition(pos);
			
			// If tile is empty, move is necessarliy valid
			if(dest.CurrentUnit == null)
				return Constants.ACTION_MOVE;
			
			// User is trying to move unit on another of his own unit
			if(dest.CurrentUnit != null && dest.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name)
				return Constants.ACTION_CANCEL;
			
			// User is trying to attack enemy unit
			if(dest.CurrentUnit != null && dest.CurrentUnit.OwnerName != GameScene.Instance.ActivePlayer.Name)
				return Constants.ACTION_ATTACK;
			
			return Constants.ACTION_CANCEL;
		}
		
		public void Update()
		{
			if(Sequence.Count > 0 && IsActive)
			{					
				if(currentTick == 0)
				{
					Sequence.Dequeue();
					currentTick = TickPerStep;
				}
				currentTick --;
			} 
			else if(IsActive)
			{
				OnPathCompleted();
			}
		}
		
		private void OnPathCompleted()
		{
			EventHandler handler = PathCompleted;
			if(handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}
