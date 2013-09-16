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
		/***********************************
		 *  FIELDS
		 ***********************************/
		public int MOVE_DISTANCE_TICK = Constants.PATH_STEP;
		
		public string OwnerName;
		public string Label;
		public int Type;
		
		public int MaxLifePoints;
		
		private int _LifePoints;
		public int LifePoints
		{
			get { return _LifePoints; }
			set
			{
				_LifePoints = value;
				if(HealthDisplay != null)
					HealthDisplay.Text = _LifePoints.ToString();
			}
		}
		
		public int Move_MaxRadius;
		public int Move_RadiusLeft;
		public int AttackDamage;
		public int Armor;		
		
		public Vector2i WorldPosition;
		
		public SpriteTile UnitSprite;
		public TextImage HealthDisplay;
		
		public Path Path;
		
		public bool IsSelected;
		public bool IsActive;
				
		/***********************************
		 *  METHODS
		 ***********************************/
		
		public void Attack(Unit unit){}	
		
		// Called at the end of a turn to restore various values
		public void Reset()
		{
			Unselect();
			Move_RadiusLeft = Move_MaxRadius;
			HealthDisplay.Text = _LifePoints.ToString();
		}
		
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
				case Constants.UNIT_TYPE_SAMURAI:
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
			Vector2i index = new Vector2i(0,0);
			switch (this.Type) {
					case Constants.UNIT_TYPE_FARMER:
						if(this.OwnerName != Constants.CHAR_KENJI)
							index = new Vector2i (0, 0);
						else
							index = new Vector2i (0, 1);
						break;
					case Constants.UNIT_TYPE_SWORD:
						index = new Vector2i (3, 0);
						break;
					case Constants.UNIT_TYPE_ARCHER:
						index = new Vector2i (0, 0);
						break;
					case Constants.UNIT_TYPE_SAMURAI:
						index = new Vector2i (1, 1);
						break;
				}
			
			// Create the tile sprite for specific unit type
			UnitSprite = new SpriteTile(AssetsManager.Instance.UnitsTextureInfo, index);
			UnitSprite.Quad = this.Quad;
			UnitSprite.Position = this.Position;
			
			if(this.OwnerName != Constants.CHAR_KENJI)
				UnitSprite.FlipU = true;
					
			HealthDisplay = new TextImage(LifePoints.ToString(), this.Position);
		}
		
		public void ClearGraphics()
		{
			GameScene.Instance.RemoveChild(this.UnitSprite, true);
			GameScene.Instance.RemoveChild(this.HealthDisplay, true);
		}
		
		public void Update()
		{
			Path.Update();
			HealthDisplay.UpdatePosition(this.Position);
			
			if(Path.Sequence.Count > 0)
			{
				IsActive = true;
				string movement = Path.Sequence.Peek();
			
				if(movement == Constants.PATH_LEFT)
					this.Position = new Vector2(Position.X - MOVE_DISTANCE_TICK, Position.Y);
				else if(movement == Constants.PATH_RIGHT)
					this.Position = new Vector2(Position.X + MOVE_DISTANCE_TICK, Position.Y);
				else if(movement == Constants.PATH_UP)
					this.Position = new Vector2(Position.X, Position.Y + MOVE_DISTANCE_TICK);
				else if(movement == Constants.PATH_DOWN)
					this.Position = new Vector2(Position.X, Position.Y - MOVE_DISTANCE_TICK);
				
				UnitSprite.Position = this.Position;
				
				// Check if this path current's step is "completed". If so, remove a MovePoint and reset the path counter
				Path.distanceMoved += Constants.PATH_STEP;
				if(Path.distanceMoved == Constants.PATH_STEP * Constants.PATH_TICKS)
				{
					Path.distanceMoved = 0;
					this.Move_RadiusLeft --;
				}
			}		
		}
		
		public void MoveTo(Vector2i destination)
		{
			this.Path.BuildMoveToSequence (this.WorldPosition, destination);
			this.Path.PathCompleted += Unit_PathCompleted;	
		}
		
		public void Unit_PathCompleted(object sender, EventArgs args)
    	{
       		// Adjust new WorldPosition
			this.WorldPosition = new Vector2i((int) this.Position.X/Constants.TILE_SIZE, (int)this.Position.Y/Constants.TILE_SIZE);
			
			//Move is completed, pop the Action Panel
			Utilities.ShowActionPanel();
			
			// Remove event so it doesn't loop 
			this.Path.PathCompleted -= Unit_PathCompleted;
		}
		
		public void FinalizeMove()
		{
			// Set the unit to the new tile it is hovering
			Utilities.AssignUnitToTileByPosition(WorldPosition, this);
			
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.CurrentMap.UnTintAllTiles ();
			
			Unselect();
			
			if(this.Move_RadiusLeft == 0)
				SetInactive();
		}
		
		public void RevertMove()
		{
			IsActive = true;
			Select();
			
			Move_RadiusLeft += Path.RadiusUsed;
			
			this.Position = new Vector2(Path.Origin.X * 64, Path.Origin.Y * 64);
			UnitSprite.Position = this.Position;
			this.WorldPosition = Path.Origin;
		}
		
		public void Sleep()
		{
			Move_RadiusLeft = 0;
			Unselect();
			SetInactive();
			
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.CurrentMap.UnTintAllTiles ();
		}
		
		#region Utilities
		public void Unselect()
		{
			IsSelected = false;
			UnitSprite.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.White, 1f), 0.3f));
		}
		
		public void Select()
		{
			// Change Selected Unit and Cursor's color
			IsSelected = true;
			GameScene.Instance.Cursor.TintToBlue();
			UnitSprite.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Lime, 1f), 0.3f));
		}
		
		public void SetInactive()
		{
			IsActive = false;
			UnitSprite.RunAction(new TintTo(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.SetAlpha(Colors.Grey30, 1f), 0.3f));
		}
		
		public bool IsMovable()
		{
			if(this.Move_RadiusLeft > 0)
				return true;
			
			return false;
		}
		#endregion
	}
}

