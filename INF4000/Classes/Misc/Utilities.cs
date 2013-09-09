using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public static class Utilities
	{
		
		#region Unit Methods
		public static void AssignUnitToPlayer(Unit unit, int playerIndex)
		{
			if(unit == null)
				return;
			
			if(playerIndex > 0)
			{
				Player desiredPlayer = GameScene.Instance.Players[playerIndex - 1];
				unit.OwnerName = desiredPlayer.Name;
				desiredPlayer.Units.Add(unit);
			}
		}
		
		public static void AssignUnitToTileByPosition(Vector2i unitPos, Unit unit)
		{					
			Tile t = GameScene.Instance.CurrentMap.Tiles[unitPos.Y, unitPos.X];
			t.CurrentUnit = unit;		
		}
		
		public static void AssignMovePathToActiveUnit()
		{
			// Build Unit Path for move action
			GameScene.Instance.ActivePlayer.ActiveUnit.Path = new Path();
			GameScene.Instance.ActivePlayer.ActiveUnit.MoveTo(GameScene.Instance.Cursor.WorldPosition);				
		}
		
		public static void FinalizeMoveAction()
		{
			GameScene.Instance.ActivePlayer.ActiveUnit.FinalizeMove();
			
			// Remove unit from previous tile
			GameScene.Instance.ActivePlayer.ActiveTile.CurrentUnit = null;
					
			// Unselect unit and remove tint from tiles
			GameScene.Instance.ActivePlayer.ActiveUnit.Unselect();
			GameScene.Instance.ActivePlayer.ActiveUnit = null;
			GameScene.Instance.CurrentMap.UnTintAllTiles ();
		}
		#endregion
		
		public static void ShowActionPanel()
		{
			GameScene.Instance.UI.ActionPanel.SetActive(true);
		}
		
		public static void LoadAllSpritesFromPlayer(List<Player> players, Node parent)
		{
			foreach(Player p in players)
			{
				foreach(Unit u in p.Units)
				{
					parent.AddChild(u.UnitSprite);
					
					if(u.HealthDisplay.TextureInfo != null)
					{
						parent.AddChild(u.HealthDisplay);
					}
				}
			}
		}
		
		public static void CycleThroughUnits()
		{
			
		}
	}
}

