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
		
		public Building (int type, int posX, int posY, int def, int goldPT, int goldProd, int prodType)
		{
			Type = type;
			
			// Assign stats
			Defense = def;
			GoldPerTurn = goldPT;
			GoldToProduce = goldProd;
			ProductionType = prodType;
			CanProduceThisTurn = true;
			
			// Set size, rotation and position
			Quad.S = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE);
			Position = new Vector2(posX * Constants.TILE_SIZE, posY * Constants.TILE_SIZE);			
			WorldPosition = new Vector2i(posX, posY);
		}
		
		public void AssignGraphics(Vector2i index)
		{
			SpriteIndex = index;
			
			SpriteTile = new SpriteTile(AssetsManager.Instance.BuildingsTextureInfo, SpriteIndex);
			SpriteTile.Quad = this.Quad;
			SpriteTile.Position = this.Position;
		}
		
		public void ProduceUnit()
		{
			CanProduceThisTurn = true;
			// UTIL CREATE UNIT ON BUILDING TILE OR AN ADJACENT
		}
	}
}

