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
		public List<int> Damages;
		
		public BattleManager (Unit attacker, Unit defender, Tile targetTile, Tile originTile)
		{
			AttackingUnit = attacker;
			DefendingUnit = defender;
			ContestedTile = targetTile;
			AttackerOriginTile = originTile;
			
			Utilities.RemoveUnitFromTileByPosition(GameScene.Instance.ActivePlayer.ActiveUnit.Path.Origin);
		}
		
		public void ComputeDamageDealt()
		{			
			Damages = new List<int>();
			int tileDefenseBonus = Math.Max(ContestedTile.Defense - AttackerOriginTile.Defense, 0);
			
			int attackHPBonus = (AttackingUnit.LifePoints == AttackingUnit.MaxLifePoints) ? (AttackingUnit.AttackDamage / 2) : 0;
			int defenseHPBonus = (DefendingUnit.LifePoints == DefendingUnit.MaxLifePoints) ? (DefendingUnit.AttackDamage / 2) : 0;
			
			int damageToDefender = (AttackingUnit.AttackDamage + attackHPBonus) + 3; //Heuristique
			int damageToAttacker = (DefendingUnit.AttackDamage + defenseHPBonus) + 1;
		
			int HP_Lost_Attacker = Math.Max(damageToAttacker - AttackingUnit.Armor/2, 0);
			int HP_Lost_Defender = Math.Max(damageToDefender - (AttackingUnit.Armor/2 + tileDefenseBonus), 0);
			
			Damages.Add(HP_Lost_Defender);
			Damages.Add(HP_Lost_Attacker);
		}	
		
		public void ExecuteAttack()
		{
			ComputeDamageDealt();
			AttackingUnit.LifePoints -= Damages[1];
			DefendingUnit.LifePoints -= Damages[0];
			
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

