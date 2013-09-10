using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class Player
	{
		public List<Unit> Units;
		public string Name;
		public bool IsActive;
		public bool IsHuman;
		public ImageAsset Icon;
		
		public int FocusPoints;
		public int LastAction;
		
		private Unit _ActiveUnit;
		public Unit ActiveUnit
		{
			get { return _ActiveUnit; } 
			set
			{
				_ActiveUnit = value;
				
				if(_ActiveUnit != null)
					this._ActiveUnit.IsSelected = true; 
			}
		}
		
		public Tile ActiveTile;
		public List<Unit> TargetUnits;
		public Unit TargetUnit;
		// Add Building list here
		
		public bool HasMovableUnits()
		{
			if(Units.Count == 0)
				return false;
			
			int nb_units = Units.Count;
			int nb_units_no_move = 0;
			foreach(Unit u in Units)
			{
				if(!u.IsMovable() && !u.IsActive)
				{
					nb_units_no_move++;
				}
			}		
			
			if(nb_units == nb_units_no_move)
				return false;
			
			return true;
		}
		
		public void ResetUnits()
		{
			foreach(Unit u in Units)
			{
				u.Reset();
			}	
		}
		
		public void AssignTarget(int index)
		{
			if(index >= TargetUnits.Count)
				return;
			
			Unit u = TargetUnits[index];
			this.TargetUnit = u;
		}
	}
}

