using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class Building : SpriteUV
	{
		public string OwnerName;
		public string Label;
		public int Type;
		
		public int Defense;
		public int GoldPerTurn;
		public int GoldToProduce;
		public int ProductionType;
		public int PointsToCapture;
		public bool CanProduceThisTurn;
		
		public int AI_ProductionValue;
		public int Heuristic;
		
		public Vector2i WorldPosition;
		public SpriteTile SpriteTile;
		
		Vector2i SpriteIndex_P1;
		Vector2i SpriteIndex_P2;
		Vector2i SpriteIndex_N;
		
		public Building (int type, int posX, int posY, int def, int goldPT, int goldProd, int prodType, string label)
		{
			Type = type;
			
			// Assign stats
			Defense = def;
			GoldPerTurn = goldPT;
			GoldToProduce = goldProd;
			ProductionType = prodType;
			Label = label;
			CanProduceThisTurn = true;
			PointsToCapture = 20;
			
			// Set size, rotation and position
			Quad.S = new Vector2(64, 64);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void Reset()
		{
			CanProduceThisTurn = true;
		}
		
		public void AssignGraphics(Vector2i indexp1, Vector2i indexp2, Vector2i indexN)
		{
			SpriteIndex_P1 = indexp1;
			SpriteIndex_P2 = indexp2;
			SpriteIndex_N = indexN;
			
			SpriteTile = new SpriteTile(AssetsManager.Instance.BuildingsTextureInfo, SpriteIndex_P1);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
			
			SetGraphics();
		}
		
		public void SetGraphics()
		{
			if(this.OwnerName == Constants.CHAR_KENJI)
				SpriteTile.TileIndex2D = SpriteIndex_P1;
			else if(this.OwnerName != null && this.OwnerName != "")
				SpriteTile.TileIndex2D = SpriteIndex_P2;
			else
				SpriteTile.TileIndex2D = SpriteIndex_N;	
		}
		
		public void ProduceUnit(string owner, Player player)
		{
			Tile t = Utilities.GetTile(this.WorldPosition);
			if(!CanProduceThisTurn || player.Gold < this.GoldToProduce) {
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_CANCEL);
				return;
			}
			
			int unitTypeToProduce = -1;
			switch(this.Type)
			{
				case Constants.BUILD_FARM:
					unitTypeToProduce = Constants.UNIT_TYPE_FARMER;
					break;
				case Constants.BUILD_TEMPLE:
					unitTypeToProduce = Constants.UNIT_TYPE_MONK;
					break;
				case Constants.BUILD_FORGE:
					unitTypeToProduce = Constants.UNIT_TYPE_SAMURAI;
					break;
			}
			
			bool produced = false;
			Unit unit = null;
			
			if(t.CurrentUnit == null && t.IsMoveValid)
			{				
				unit = Unit.CreateByType(unitTypeToProduce, 9000, 9000, t.WorldPosition.X, t.WorldPosition.Y); // Will ignore gigantic value
				Utilities.AssignUnitToPlayerByName(unit, owner);
				
				unit.LoadGraphics();
				t.CurrentUnit = unit;
				produced = true;
			}
			else
			{
				foreach(Vector2i v in t.AdjacentPositions)
				{
					Tile adj = Utilities.GetTile(v);
					if(adj.CurrentUnit == null && t.IsMoveValid)
					{
						unit = Unit.CreateByType(unitTypeToProduce, 9000, 9000, adj.WorldPosition.X, adj.WorldPosition.Y); // Will ignore gigantic value
						Utilities.AssignUnitToPlayerByName(unit, owner);
						
						unit.LoadGraphics();
						adj.CurrentUnit = unit;
						produced = true;
						break;
					}
				}
			}
			
			if(produced && unit != null)
			{					
				GameScene.Instance.RemoveChild(GameScene.Instance.Cursor.SpriteTile, false);
				GameScene.Instance.AddChild(unit.UnitSprite);
				GameScene.Instance.AddChild(unit.HealthDisplay);
				GameScene.Instance.AddChild(GameScene.Instance.Cursor.SpriteTile);
				
				unit.Sleep();
				player.Gold -= this.GoldToProduce;
			} else {
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_CANCEL);
			}
			
			CanProduceThisTurn = false;
		}
	}
}

