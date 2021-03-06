using System;
using System.Collections;
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
		
		public string UniqueId;
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
		private bool DidCaptureThisTurn;
		
		/* AI Fields */
		public int Behavior;
		public Vector2i FinalDestination;
		public Queue AI_Actions;
		public int Heuristic; // The lower the value is, the most likely the AI will act on the unit
				
		/***********************************
		 *  METHODS
		 ***********************************/
		
		public void Attack(Unit unit){}	
		
		// Called at the end of a turn to restore various values
		public void Reset()
		{
			if(!DidCaptureThisTurn) {
				TryReclaimBuilding();
				TryCaptureBuilding();
			}
			IsActive = false;
			Unselect();
			Move_RadiusLeft = Move_MaxRadius;
			HealthDisplay.Text = _LifePoints.ToString();
			DidCaptureThisTurn = false;
		}
		
		public static Unit CreateByType(int type, int moves, int lifePoints, int posX, int posY)
		{
			switch(type)
			{
				case Constants.UNIT_TYPE_FARMER:
					return new Farmer(moves, lifePoints, posX, posY);
				case Constants.UNIT_TYPE_SAMURAI:
					return new Samurai(moves, lifePoints, posX, posY);
				case Constants.UNIT_TYPE_MONK:
					return new Monk(moves, lifePoints, posX, posY);
				case Constants.UNIT_TYPE_WIZARD:
					break;
				default:
					return null;
			}
			return null;
		}
		
		public void LoadGraphics()
		{			
			Vector2i index = new Vector2i(0,0);
			switch (this.Type) {
					case Constants.UNIT_TYPE_FARMER:
						if(this.OwnerName == Constants.CHAR_KENJI)
							index = new Vector2i (0, 2);
						else
							index = new Vector2i (1, 2);
						break;
					case Constants.UNIT_TYPE_SAMURAI:
						if(this.OwnerName == Constants.CHAR_KENJI)
							index = new Vector2i (0, 3);
						else
							index = new Vector2i (1, 3);
						break;
					case Constants.UNIT_TYPE_MONK:
						if(this.OwnerName == Constants.CHAR_KENJI)
							index = new Vector2i (2, 3);
						else
							index = new Vector2i (3, 3);
						break;
				}
			
			// Create the tile sprite for specific unit type
			UnitSprite = new SpriteTile(AssetsManager.Instance.UnitsTextureInfo, index);
			UnitSprite.Quad = this.Quad;
			UnitSprite.Position = this.Position;
			
			if(this.OwnerName != Constants.CHAR_KENJI)
				UnitSprite.FlipU = true;
					
			HealthDisplay = new TextImage(LifePoints.ToString(), new Vector2(this.Position.X + 15, this.Position.Y));
		}
		
		public void ClearGraphics()
		{
			GameScene.Instance.RemoveChild(this.UnitSprite, true);
			GameScene.Instance.RemoveChild(this.HealthDisplay, true);
		}
		
		public void Update()
		{
			Path.Update();
			
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
				HealthDisplay.UpdatePositionUnit(new Vector2(this.Position.X + 15, this.Position.Y));
				
				// Check if this path current's step is "completed". If so, remove a MovePoint and reset the path counter
				Path.distanceMoved += Constants.PATH_STEP;
				if(Path.distanceMoved == Constants.PATH_STEP * Constants.PATH_TICKS)
				{
					Path.distanceMoved = 0;
					this.Move_RadiusLeft --;
				}
			}
			
			HealthDisplay.UpdatePositionUnit(new Vector2(this.Position.X + 15, this.Position.Y));
		}
		
		public void MoveTo(Vector2i destination)
		{
			this.Path.BuildMoveToSequence (this.WorldPosition, destination);
			this.Path.PathCompleted += Unit_PathCompleted;	
		}
		
		public void MoveToAfterWin(Vector2i destination)
		{
			this.Path = new Path();
			if(GameScene.Instance.ActivePlayer.IsHuman) {
				this.Path.BuildMoveToSequence (this.WorldPosition, destination);
				this.Path.PathCompleted += Unit_PathCompletedAfterWin;	
			} else {
				((AIPlayer)GameScene.Instance.ActivePlayer).AddMoveAfterWin(destination);
				this.Move_RadiusLeft = 1;
			}
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
		
		public void AI_Unit_PathCompleted(object sender, EventArgs args)
    	{
       		// Adjust new WorldPosition
			this.WorldPosition = new Vector2i((int) this.Position.X/Constants.TILE_SIZE, (int)this.Position.Y/Constants.TILE_SIZE);
			
			// Remove event so it doesn't loop 
			this.Path.PathCompleted -= AI_Unit_PathCompleted;
			Utilities.AssignUnitToTileByPosition(WorldPosition, this);
			
			((AIPlayer)GameScene.Instance.ActivePlayer).NextAction();
			
		}
		
		public void AI_Sleep()
		{
			this.Move_RadiusLeft = 0;
			SetInactive();
			TryCaptureBuilding();
			TryReclaimBuilding();
		}
		
		public void Unit_PathCompletedAfterWin(object sender, EventArgs args)
    	{
       		// Adjust new WorldPosition
			this.WorldPosition = new Vector2i((int) this.Position.X/Constants.TILE_SIZE, (int)this.Position.Y/Constants.TILE_SIZE);
			
			// Remove event so it doesn't loop 
			this.Path.PathCompleted -= Unit_PathCompletedAfterWin;			
			Utilities.AssignUnitToTileByPosition(WorldPosition, this);
			
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.CurrentMap.UnTintAllTiles ();
			Move_RadiusLeft = 0;
			
			Unselect();
			SetInactive();
			TryCaptureBuilding();
			TryReclaimBuilding();
		}
		
		public void FinalizeMove()
		{
			// Set the unit to the new tile it is hovering
			Utilities.AssignUnitToTileByPosition(WorldPosition, this);
			
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.CurrentMap.UnTintAllTiles ();
			
			Unselect();
			Path.RadiusUsed = 0;
			
			if(this.Move_RadiusLeft == 0)
				SetInactive();
			
			//TryCaptureBuilding();
			//TryReclaimBuilding();
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
			TryCaptureBuilding();
			TryReclaimBuilding();
		}
		
		private void TryCaptureBuilding()
		{
			Tile t = Utilities.GetTile(this.WorldPosition);
			if(t.CurrentBuilding != null && t.CurrentBuilding.OwnerName != this.OwnerName && !DidCaptureThisTurn )
			{	
				this.DidCaptureThisTurn = true;
				BuildingUtil.LastCapturedBuildings.Push(t.CurrentBuilding);
				
				t.CurrentBuilding.PointsToCapture -= this.LifePoints;
				t.CurrentBuilding.PointsToCapture = (t.CurrentBuilding.PointsToCapture < 0) ? 0 : t.CurrentBuilding.PointsToCapture;
				
				if(t.CurrentBuilding.PointsToCapture == 0) {
					SoundManager.Instance.PlaySound(Constants.SOUND_CAPTURE);
					if(t.CurrentBuilding.Type == Constants.TILE_TYPE_BUILD_FORT || t.CurrentBuilding.Type == Constants.TILE_TYPE_BUILD_FORT_2)
					{
						GameScene.Instance.IsGameOver = true; // SOMEONE JUST WON THE FREAKING GAME!
						GameScene.Instance.WinnerName = this.OwnerName;
					}
					if(t.CurrentBuilding.OwnerName != null && t.CurrentBuilding.OwnerName != "")
						Utilities.RemoveBuildingFromPlayerByName(t.CurrentBuilding, t.CurrentBuilding.OwnerName);
					
					Utilities.AssignBuildingToPlayerByName(t.CurrentBuilding, OwnerName);
					t.CurrentBuilding.PointsToCapture = 20;
				}
			}
		}
		
		private void TryReclaimBuilding()
		{
			Tile t = Utilities.GetTile(this.WorldPosition);
			if(t.CurrentBuilding != null && t.CurrentBuilding.OwnerName == this.OwnerName && !DidCaptureThisTurn)
			{
				if(t.CurrentBuilding.PointsToCapture < 20) {
					this.DidCaptureThisTurn = true;
					t.CurrentBuilding.PointsToCapture += this.LifePoints;
					BuildingUtil.LastCapturedBuildings.Push(t.CurrentBuilding);
				}
				t.CurrentBuilding.PointsToCapture = (t.CurrentBuilding.PointsToCapture >= 20) ? 20 : t.CurrentBuilding.PointsToCapture;
			}
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

