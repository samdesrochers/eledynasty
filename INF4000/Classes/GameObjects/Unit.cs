using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Unit : SpriteUV
	{
		public int Team; // Changera
		
		public int LifePoints;
		public int Move_MaxRadius;
		public int Move_RadiusLeft;
		public int AttackDamage;
		public int Armor;		
		
		public Vector2i WorldPosition;
		public SpriteTile SpriteTile;
		
		public void Update(){}
		
		public void MoveTo(Tile tile){}
		public void Attack(Unit unit){}
		
		public void EndTurn(){}
		
		public static Unit Create(int tileUnit, int moves, int lifePoints)
		{
			Unit unit = new Unit();
			switch(tileUnit)
			{
				case Constants.UNIT_TYPE_FARMER:
					unit = new Farmer(moves, lifePoints);
					break;
				case Constants.UNIT_TYPE_SWORD:
					break;
				case Constants.UNIT_TYPE_ARCHER:
					break;
				case Constants.UNIT_TYPE_KNIGHT:
					break;
				case Constants.UNIT_TYPE_WIZARD:
					break;
				default:
					unit = new Farmer(moves, lifePoints);
					break;
			}
			
			return unit;
		}
	}
}

