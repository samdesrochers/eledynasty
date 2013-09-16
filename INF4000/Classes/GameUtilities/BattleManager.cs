using System;

namespace INF4000
{
	public class BattleManager
	{
		Unit AttackingUnit;
		Unit DefendingUnit;
		
		Tile ContestedTile;
		Tile AttackerOriginTile;
		
		public int CombatEndState;
		
		public BattleManager (Unit attacker, Unit defender, Tile targetTile, Tile originTile)
		{
			AttackingUnit = attacker;
			DefendingUnit = defender;
			ContestedTile = targetTile;
			AttackerOriginTile = originTile;
		}
		
		public void ExecuteAttack()
		{
			int tileDefenseBonus = Math.Max(ContestedTile.Defense - AttackerOriginTile.Defense, 0);
			int damageToDefender = (AttackingUnit.AttackDamage * (AttackingUnit.LifePoints / AttackingUnit.MaxLifePoints)) + 3; //Heuristique
			int damageToAttacker = (DefendingUnit.AttackDamage * (DefendingUnit.LifePoints / DefendingUnit.MaxLifePoints)) + 1;
			
			int HP_Lost_Attacker = damageToAttacker - AttackingUnit.Armor/2;
			int HP_Lost_Defender = damageToDefender - (AttackingUnit.Armor/2 + tileDefenseBonus);
			
			AttackingUnit.LifePoints -= HP_Lost_Attacker;
			DefendingUnit.LifePoints -= HP_Lost_Defender;
			
			if(AttackingUnit.LifePoints <= 0 && DefendingUnit.LifePoints > 0)
			{
				CombatEndState = Constants.BATTLE_END_DEFENDER_TOTALWIN;
			}
			
			if(DefendingUnit.LifePoints <= 0 && AttackingUnit.LifePoints > 0)
			{
				CombatEndState = Constants.BATTLE_END_ATTACKER_TOTALWIN;
			}
			
			if(AttackingUnit.LifePoints > 0 && DefendingUnit.LifePoints > 0)
			{
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
			AttackingUnit.Path = new Path();
			AttackingUnit.Path.BuildMoveToSequence(AttackingUnit.WorldPosition, DefendingUnit.WorldPosition);
			                                       
			Player defenderPlayer = Utilities.GetPlayerByName(DefendingUnit.OwnerName);
			defenderPlayer.KillUnit(DefendingUnit);
		}
		
		private void ExecuteDefenderTotalWin()
		{
			Player attackerPlayer = Utilities.GetPlayerByName(AttackingUnit.OwnerName);
			attackerPlayer.KillUnit(AttackingUnit);
		}
		
		private void ExecuteTie()
		{
			
		}
		
		public void ExecuteFinalizePostCombat()
		{
			Utilities.HideAttackPanel();
			Utilities.HideActionPanel();
			
			if(AttackingUnit != null && AttackingUnit.LifePoints > 0)
				AttackingUnit.Sleep();
		}
	}
}

