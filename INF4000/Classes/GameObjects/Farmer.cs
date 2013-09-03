using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Farmer : Unit
	{
		public Farmer (int moves, int lifePoints, int posX, int posY)
		{
			IsSelected = false;
			this.Label = "Farmer";
			this.Path = new Path();
			
			this.Type = Constants.UNIT_TYPE_FARMER;
			this.LifePoints = System.Math.Min(Constants.UNIT_HP_FARMER, lifePoints);
			this.AttackDamage = Constants.UNIT_AD_FARMER;
			this.Armor = Constants.UNIT_AD_FARMER;
			this.Move_MaxRadius = 3;
			this.Move_RadiusLeft = 3;
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
			
			MovePointsDisplay = new TextImage(this.Move_RadiusLeft.ToString(), this.Position);
		}	
		
		public override void Update()
		{
			base.Update();
		}
	}
}

