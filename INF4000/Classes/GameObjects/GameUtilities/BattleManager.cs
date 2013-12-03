using System;
using System.Collections.Generic;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class BattleManager
	{
		public Unit AttackingUnit;
		public Unit DefendingUnit;
		
		public Tile ContestedTile;
		public Tile AttackerOriginTile;
		
		public int CombatEndState;
		public List<float> Damages;
		public List<float> DmgPercentages;
		
		private bool IsDamageCalculated;
		
		public BattleManager (Unit attacker, Unit defender, Tile targetTile, Tile originTile)
		{
			AttackingUnit = attacker;
			DefendingUnit = defender;
			ContestedTile = targetTile;
			AttackerOriginTile = originTile;
		}
		
		public void ComputeDamagePercantages(bool countAlly)
		{			
			Damages = new List<float>();
			DmgPercentages = new List<float>();
			
			Random random = new Random();
			float coeff_attacker = 100;
			float coeff_defender = 100;
			
			float ran_attacker = random.Next(1, 9);
			float ran_defender = random.Next(1, 9);
			
//			if(AttackingUnit.OwnerName == Constants.CHAR_KENJI)
//				coeff_attacker = 105; 
//			else if(DefendingUnit.OwnerName == Constants.CHAR_KENJI)
//				coeff_defender = 103;
			
			if(countAlly){
				int attackerSupportCoeffBonus = GetSupportCoefficient(AttackerOriginTile, AttackingUnit);
				int defenderSupportCoeffBonus = GetSupportCoefficient(ContestedTile, DefendingUnit);
				
				coeff_attacker += attackerSupportCoeffBonus;
				coeff_defender += defenderSupportCoeffBonus;
			}
			
			float AttackerHP = (float)AttackingUnit.LifePoints;
			float DefenderHP = (float)DefendingUnit.LifePoints;
			
			float HP_Lost_Defender = ((10*AttackingUnit.AttackDamage * (coeff_attacker/100)) + 2*ran_attacker ) * (AttackerHP/10) * (( 200 - (coeff_defender + 2*DefendingUnit.Armor + (ContestedTile.Defense*DefenderHP) ))/100);				
			float HP_Lost_Attacker = ((10*DefendingUnit.AttackDamage * (coeff_defender/100)) + 2*ran_defender ) * (DefenderHP/10) * (( 200 - (coeff_attacker + 2*AttackingUnit.Armor + (2*AttackerHP))) /100); 
			
			Damages.Add((int)(HP_Lost_Defender/10));
			Damages.Add((int)(HP_Lost_Attacker/10));
			
			DmgPercentages.Add(HP_Lost_Defender);
			DmgPercentages.Add(HP_Lost_Attacker);
			IsDamageCalculated = true;
		}	
		
		public void ExecuteAttack()
		{
			Utilities.RemoveUnitFromTileById(AttackingUnit.UniqueId);
			Utilities.AssignUnitToTileByPosition(AttackingUnit.WorldPosition, AttackingUnit);
			
			if(!IsDamageCalculated) {
				
				ComputeDamagePercantages(true);
				IsDamageCalculated = false;
			}

			int dmg1 = (int)Damages[1];
			int dmg2 = (int)Damages[0];
			
			AttackingUnit.LifePoints -= dmg1;
			DefendingUnit.LifePoints -= dmg2;
			
			if(AttackingUnit.LifePoints <= 0 && DefendingUnit.LifePoints > 0) {
				CombatEndState = Constants.BATTLE_END_DEFENDER_TOTALWIN;
				
			} else if(DefendingUnit.LifePoints <= 0 && AttackingUnit.LifePoints > 0) {
				CombatEndState = Constants.BATTLE_END_ATTACKER_TOTALWIN;
				
			} else if(AttackingUnit.LifePoints > 0 && DefendingUnit.LifePoints > 0) {
				CombatEndState = Constants.BATTLE_END_TIE; 
			}
			
			if(AttackingUnit.LifePoints <= 0 && DefendingUnit.LifePoints <= 0)
			{
				CombatEndState = Constants.BATTLE_END_ATTACKER_TOTALWIN;
				AttackingUnit.LifePoints = 1;
			}
		}
		
		public void ExecuteCombatOutcome()
		{
			switch(CombatEndState)
			{
			case Constants.BATTLE_END_ATTACKER_TOTALWIN:
				ExecuteAttackerTotalWin();
				break;
			case Constants.BATTLE_END_DEFENDER_TOTALWIN:
				ExecuteDefenderTotalWin();
				break;
			case Constants.BATTLE_END_TIE:
				ExecuteTie();
				break;
			}
		}
		
		private void ExecuteAttackerTotalWin()
		{
			Player defenderPlayer = Utilities.GetPlayerByName(DefendingUnit.OwnerName);
			defenderPlayer.KillUnit(DefendingUnit);
					
			AttackingUnit.MoveToAfterWin(ContestedTile.WorldPosition);
			AttackerOriginTile.CurrentUnit = null;
			
			GameScene.Instance.ActivePlayer.SpellPoints = (GameScene.Instance.ActivePlayer.IsHuman) ? 
				GameScene.Instance.ActivePlayer.SpellPoints += 1 : GameScene.Instance.ActivePlayer.SpellPoints;
		}
		
		private void ExecuteDefenderTotalWin()
		{
			Player attackerPlayer = Utilities.GetPlayerByName(AttackingUnit.OwnerName);
			attackerPlayer.KillUnit(AttackingUnit);
			AttackerOriginTile.CurrentUnit = null;
			
			GameScene.Instance.ActivePlayer.SpellPoints = (GameScene.Instance.ActivePlayer.IsHuman) ? 
				GameScene.Instance.ActivePlayer.SpellPoints += 1 : GameScene.Instance.ActivePlayer.SpellPoints;
		}
		
		private void ExecuteTie()
		{
			AttackerOriginTile.CurrentUnit = AttackingUnit;
			
			GameScene.Instance.CurrentMap.UnTintAllTiles();
			GameScene.Instance.Cursor.TintToWhite();
			AttackingUnit.Sleep();
		}
		
		public void ExecuteFinalizePostCombat()
		{
			Utilities.HideAttackPanel();
			
			if(AttackingUnit != null && AttackingUnit.LifePoints > 0)
				AttackingUnit.Sleep();
			
			GameScene.Instance.CurrentMap.UnTintAllTiles();
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.ActivePlayer.ActiveUnit = null;
		}
		
		private int GetSupportCoefficient(Tile t, Unit ally)
		{
			int coeff = 0;
			Unit supporter = GetAllyUnit(t, ally);
			if(supporter != null)
			{
				coeff += (supporter.AttackDamage + supporter.Armor);
			}
			return coeff;
		}
		
		private Unit GetAllyUnit(Tile t, Unit ally)
		{
			if(t == null || ally == null)
				return null;
			
			foreach(Vector2i adjPos in t.AdjacentPositions)
			{
				Tile adj = Utilities.GetTile(adjPos);
				if(adj != null && adj.CurrentUnit != null && ally.OwnerName == adj.CurrentUnit.OwnerName && adj.CurrentUnit.UniqueId != ally.UniqueId)
					return adj.CurrentUnit;
			}
			return null;
		}
	}
}

