using System;
using System.Collections.Generic;

namespace INF4000
{
	public class Player
	{
		public List<Unit> Units;
		public string Name;
		public bool IsActive;
		
		private Unit _ActiveUnit;
		public Unit ActiveUnit
		{
			get { return _ActiveUnit; } 
			set
			{
				_ActiveUnit = value; 
				this._ActiveUnit.IsSelected = true; 
			}
		}
		
		public Tile ActiveTile;
		// Add Building list here
		
		public bool HasMovableUnits()
		{
			if(Units.Count == 0)
				return false;
			
			int nb_units = Units.Count - 1;
			int nb_units_no_move = 0;
			foreach(Unit u in Units)
			{
				if(u.Move_RadiusLeft == 0)
				{
					nb_units_no_move++;
				}
			}		
			
			return nb_units == nb_units_no_move;
		}
		
		public void UnselectAllUnits()
		{
			foreach(Unit u in Units)
			{
				u.IsSelected = false;
			}
		}
	}
}

