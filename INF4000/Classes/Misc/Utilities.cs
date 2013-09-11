using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;

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
		
		public static void AssignMovePathToActiveUnit(Path path)
		{
			// Build Unit Path for move action
			GameScene.Instance.ActivePlayer.ActiveUnit.Path = path;
			GameScene.Instance.ActivePlayer.ActiveUnit.MoveTo(GameScene.Instance.Cursor.WorldPosition);				
		}
		
		public static void FinalizeMoveAction()
		{
			GameScene.Instance.ActivePlayer.ActiveUnit.FinalizeMove();
			
			// Remove unit from previous tile
			GameScene.Instance.ActivePlayer.ActiveTile.CurrentUnit = null;
					
			// Unselect unit and remove tint from tiles
			GameScene.Instance.ActivePlayer.ActiveUnit = null;
			
		}
		#endregion
		
		public static bool CanUnitAttackFromDestination(Tile tile)
		{
			foreach(Vector2i v in tile.AdjacentPosition){
				if(GameScene.Instance.CurrentMap.GetTile(v) != null)
				{
					Tile t = GameScene.Instance.CurrentMap.GetTile(v);
					if(t.CurrentUnit != null && t.CurrentUnit.OwnerName != GameScene.Instance.ActivePlayer.Name)
						return true;
				}
			}
			return false;
		}
		
		public static void ShowActionPanel()
		{
			GameScene.Instance.UI.ActionPanel.SetActive(true);
		}
		
		public static void ShowAttackPanel()
		{
			GameScene.Instance.UI.ActionPanel.SetActive(true);
		}
		
		public static void HideAttackPanel()
		{
			GameScene.Instance.UI.OddsPanel.SetVisible(false);
		}
		
		public static void MoveAttackOddsPanel(Vector2 pos)
		{
			GameScene.Instance.UI.OddsPanel.SetPosition(new Vector2(pos.X, 544 - pos.Y));
			GameScene.Instance.UI.OddsPanel.SetVisible(true);
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
		
		#region Attack Methods
		public static void CycleEnemyUnitsRight()
		{
			Unit u = GameScene.Instance.ActivePlayer.TargetUnit;
			int idx = GameScene.Instance.ActivePlayer.TargetUnits.IndexOf(u);
			
			int nextIdx = (idx + 1) %  GameScene.Instance.ActivePlayer.TargetUnits.Count;
			u = GameScene.Instance.ActivePlayer.TargetUnits[nextIdx];
			GameScene.Instance.ActivePlayer.TargetUnit = u;
			GameScene.Instance.Cursor.MoveToTileByWorldPosition(u.WorldPosition);
		}
		
		public static void CycleEnemyUnitsLeft()
		{
			Unit u = GameScene.Instance.ActivePlayer.TargetUnit;
			int idx = GameScene.Instance.ActivePlayer.TargetUnits.IndexOf(u);
			
			int nextIdx = System.Math.Abs((idx - 1) %  GameScene.Instance.ActivePlayer.TargetUnits.Count);
			u = GameScene.Instance.ActivePlayer.TargetUnits[nextIdx];
			GameScene.Instance.ActivePlayer.TargetUnit = u;
			GameScene.Instance.Cursor.MoveToTileByWorldPosition(u.WorldPosition);
		}
		#endregion
	}
}

