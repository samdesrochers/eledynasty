using System;
using System.Collections.Generic;

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
			
			Utilities.RemoveUnitFromTileByPosition(GameScene.Instance.ActivePlayer.ActiveUnit.Path.Origin);
		}
		
		public void ComputeDamagePercantages()
		{			
			Damages = new List<float>();
			DmgPercentages = new List<float>();
			
			Random random = new Random();
			float coeff_attacker = 100;
			float coeff_defender = 100;
			
			float ran_attacker = random.Next(1, 9);
			float ran_defender = random.Next(1, 9);
			
			if(AttackingUnit.OwnerName == Constants.CHAR_KENJI)
				coeff_attacker = 110; 
			else if(DefendingUnit.OwnerName == Constants.CHAR_KENJI)
				coeff_defender = 110;
			
			float AttackerHP = (float)AttackingUnit.LifePoints;
			float DefenderHP = (float)DefendingUnit.LifePoints;
			
			float HP_Lost_Defender = ((10*AttackingUnit.AttackDamage * (coeff_attacker/100)) + 4*ran_attacker ) * (AttackerHP/10) * (( 200 - (coeff_defender + 6*DefendingUnit.Armor + (2*ContestedTile.Defense*DefenderHP) ))/100);				
			float HP_Lost_Attacker = ((10*DefendingUnit.AttackDamage * (coeff_defender/100)) + 4*ran_defender ) * (DefenderHP/10) * (( 200 - (coeff_attacker + 6*AttackingUnit.Armor + (2*AttackerHP))) /100); 
			
			Damages.Add((int)(HP_Lost_Defender/10));
			Damages.Add((int)(HP_Lost_Attacker/10));
			
			DmgPercentages.Add(HP_Lost_Defender);
			DmgPercentages.Add(HP_Lost_Attacker);
			IsDamageCalculated = true;
		}	
		
		public void ExecuteAttack()
		{
			if(!IsDamageCalculated) {
				ComputeDamagePercantages();
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
		}
		
		private void ExecuteDefenderTotalWin()
		{
			Player attackerPlayer = Utilities.GetPlayerByName(AttackingUnit.OwnerName);
			attackerPlayer.KillUnit(AttackingUnit);
			AttackerOriginTile.CurrentUnit = null;
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
			Utilities.HideActionPanel();
			
			if(AttackingUnit != null && AttackingUnit.LifePoints > 0)
				AttackingUnit.Sleep();
			
			GameScene.Instance.CurrentMap.UnTintAllTiles();
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.ActivePlayer.ActiveUnit = null;
		}
	}
}
