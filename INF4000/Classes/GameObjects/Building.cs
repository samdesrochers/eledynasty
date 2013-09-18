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
		public bool CanProduceThisTurn;
		
		public Vector2i WorldPosition;
		public SpriteTile SpriteTile;
		
		Vector2i SpriteIndex;
		
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
			
			// Set size, rotation and position
			Quad.S = new Vector2(64, 80);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void Reset()
		{
			CanProduceThisTurn = true;
		}
		
		public void AssignGraphics(Vector2i index)
		{
			SpriteIndex = index; 
			
			SpriteTile = new SpriteTile(AssetsManager.Instance.BuildingsTextureInfo, SpriteIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
		
		public void ProduceUnit(Tile t, string owner, Player player)
		{
			if(!CanProduceThisTurn || player.Gold < this.GoldToProduce)
				return;
			
			int unitTypeToProduce = -1;
			switch(this.Type)
			{
				case Constants.BUILD_FARM:
					unitTypeToProduce = Constants.UNIT_TYPE_FARMER;
					break;
			}
			
			bool produced = false;
			Unit unit = null;
			
			if(t.CurrentUnit == null)
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
					Tile adj = GameScene.Instance.CurrentMap.GetTile(v);
					if(adj.CurrentUnit == null)
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
			}
			
			CanProduceThisTurn = false;
		}
	}
}

