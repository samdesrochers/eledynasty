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
		public int MOVE_DISTANCE_TICK = Constants.PATH_STEP;
		
		public string OwnerName;
		public string Identifier;
		public int Type;
		
		public int LifePoints;
		public int Move_MaxRadius;
		public int Move_RadiusLeft;
		public int AttackDamage;
		public int Armor;		
		
		public Vector2i WorldPosition;
		public SpriteTile SpriteTile;
		
		public Path Path;
		
		public bool IsSelected;
		
		public void Update()
		{
			Path.Update();
			
			if(Path.Sequence.Count > 0)
			{
				string movement = Path.Sequence.Peek();
			
				if(movement == Constants.PATH_LEFT)
				{
					this.Position = new Vector2(Position.X - MOVE_DISTANCE_TICK, Position.Y);
				}
				else if(movement == Constants.PATH_RIGHT)
				{
					this.Position = new Vector2(Position.X + MOVE_DISTANCE_TICK, Position.Y);
				}
				else if(movement == Constants.PATH_UP)
				{
					this.Position = new Vector2(Position.X, Position.Y + MOVE_DISTANCE_TICK);
				}
				else if(movement == Constants.PATH_DOWN)
				{
					this.Position = new Vector2(Position.X, Position.Y - MOVE_DISTANCE_TICK);
				}
				SpriteTile.Position = this.Position;
				GameScene.Instance.DebugHelp.Text = this.Position.ToString();
			}
		}
		
		public void Attack(Unit unit){}
		
		public void EndTurn(){}
		
		public static Unit CreateByType(int type, int moves, int lifePoints, int posX, int posY)
		{
			switch(type)
			{
				case Constants.UNIT_TYPE_FARMER:
					return new Farmer(moves, lifePoints, posX, posY);
				case Constants.UNIT_TYPE_SWORD:
					break;
				case Constants.UNIT_TYPE_ARCHER:
					break;
				case Constants.UNIT_TYPE_KNIGHT:
					break;
				case Constants.UNIT_TYPE_WIZARD:
					break;
				default:
					return new Farmer(moves, lifePoints, posX, posY);
			}
			return null;
		}
		
		public void CreateSpriteTile(Vector2i index)
		{
			SpriteTile = new SpriteTile(this.TextureInfo, index);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
		
		public void AssignUnitToTile(object sender, EventArgs args)
    	{
       		// Adjust new WorldPosition
			this.WorldPosition = new Vector2i((int) this.Position.X/Constants.TILE_SIZE, (int)this.Position.Y/Constants.TILE_SIZE);
			
			// Set the unit to the new tile it is hovering
			Utilities.AssignUnitToTileByPosition(WorldPosition, this);
			
			// Remove event so it doesn't loop 
			this.Path.PathCompleted -= AssignUnitToTile;
		}
	}
}

