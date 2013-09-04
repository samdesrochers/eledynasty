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
		public string Label;
		public int Type;
		
		public int LifePoints;
		public int Move_MaxRadius;
		public int Move_RadiusLeft;
		public int AttackDamage;
		public int Armor;		
		
		public Vector2i WorldPosition;
		
		public Texture2D Texture;
		public SpriteTile UnitSprite;
		public SpriteTile UnitHP;
		public TextImage MovePointsDisplay;
		
		private const string AssetsName = "/Application/Assets/Units/units.png";
		
		public Path Path;
		
		public bool IsSelected;
		
		public virtual void Update()
		{
			Path.Update();
			MovePointsDisplay.AssignRelativePosition(this.Position);
			
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
				
				UnitSprite.Position = this.Position;
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
		
		public void LoadGraphics()
		{
			// Create the actual texture object and specify its size
			Texture = new Texture2D (AssetsName, false);
			this.TextureInfo = new TextureInfo (Texture, new Vector2i (2, 2));
			
			Vector2i index = new Vector2i(0,0);
			switch (this.Type) {
					case Constants.UNIT_TYPE_FARMER:
						index = new Vector2i (0, 1);
						break;
					case Constants.UNIT_TYPE_SWORD:
						index = new Vector2i (3, 0);
						break;
					case Constants.UNIT_TYPE_ARCHER:
						index = new Vector2i (0, 0);
						break;
					case Constants.UNIT_TYPE_KNIGHT:
						index = new Vector2i (1, 1);
						break;
				}
			
			// Create the tile sprite for specific unit type
			UnitSprite = new SpriteTile(this.TextureInfo, index);
			UnitSprite.Quad = this.Quad;
			UnitSprite.Position = this.Position;
					
			MovePointsDisplay = new TextImage("", this.Position);
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
		
		public void TintToWhite()
		{
			UnitSprite.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.White, 1f), 0.3f));
		}
		
		public void TintToBlue()
		{
			UnitSprite.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Lime, 1f), 0.3f));
		}
	}
}

