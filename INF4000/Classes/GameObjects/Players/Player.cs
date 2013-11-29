using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class Player
	{
		public List<Unit> Units;
		public List<Building> Buildings;
		
		public string Name;
		public bool IsActive;
		public bool IsHuman;
		public bool IsStartingTurn;
		public ImageAsset Icon;
		public int LastAction;
		
		private int _SpellPoints;
		public int SpellPoints {
			get { return _SpellPoints; }
			set { 
				_SpellPoints = value; 
				_SpellPoints = (_SpellPoints > 20) ? 20 : _SpellPoints;
				if(GameScene.Instance.GameUI != null && GameScene.Instance.GameUI.PlayerPanel != null)
					GameScene.Instance.GameUI.PlayerPanel.SetFocus(_SpellPoints.ToString());
			}
		}

		private int _Gold;
		public int Gold {
			get { return _Gold; }
			set { 
				_Gold = value;
				_Gold = (_SpellPoints > 60) ? 60 : _Gold;
				if(GameScene.Instance.GameUI != null && GameScene.Instance.GameUI.PlayerPanel != null)
					GameScene.Instance.GameUI.PlayerPanel.SetGold(_Gold.ToString());
			}
		}		
		
		public int GoldEarnedThisTurn;
		
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
		
		public Building ActiveBuilding;
		
		public virtual void Reset(){}
		public virtual void Update(float dt){}
		
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
		
		public void ResetBuildings()
		{
			foreach(Building b in Buildings)
			{
				b.Reset();
			}	
		}
		
		public void AssignTarget(int index)
		{
			if(index >= TargetUnits.Count)
				return;
			
			Unit u = TargetUnits[index];
			this.TargetUnit = u;
		}
		
		public void KillUnit(Unit u)
		{
			Tile t = Utilities.GetTile(u.WorldPosition);
			t.CurrentUnit = null;
			
			u.ClearGraphics();
			Units.Remove(u);
		}
		
		public void CollectGoldFromBuildings()
		{
			int amount = 0;
			foreach(Building b in Buildings)
			{
				amount += b.GoldPerTurn;
			}
			this.GoldEarnedThisTurn = amount;
			this.Gold += amount;
		}
	}
}

