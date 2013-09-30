using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Samurai : Unit
	{
		public Samurai (int moves, int lifePoints, int posX, int posY)
		{
			IsSelected = false;
			this.Label = "Samurai";
			this.Path = new Path();
			
			this.Type = Constants.UNIT_TYPE_SAMURAI;
			this.LifePoints = System.Math.Min(Constants.UNIT_HP_SAMURAI, lifePoints);
			this.MaxLifePoints = Constants.UNIT_HP_SAMURAI;	
			this.AttackDamage = Constants.UNIT_AD_SAMURAI;
			this.Armor = Constants.UNIT_DEF_SAMURAI;
			this.Move_MaxRadius = Constants.UNIT_MOVE_SAMURAI;
			this.Move_RadiusLeft = System.Math.Min(Constants.UNIT_MOVE_SAMURAI, moves);
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
			
			HealthDisplay = new TextImage(LifePoints.ToString(), this.Position);
		}	
	}
}

