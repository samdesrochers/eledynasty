using System;

namespace INF4000
{
	public class Farmer : Unit
	{
		public Farmer (int remainingMoves, int remainingLife)
		{
			this.LifePoints = Constants.UNIT_HP_FARMER;
			this.AttackDamage = Constants.UNIT_AD_FARMER;
			this.Armor = Constants.UNIT_AD_FARMER;
			this.Move_MaxRadius = Constants.UNIT_MOVE_FARMER;
			this.Move_RadiusLeft = Move_MaxRadius;			
		}
	}
}

