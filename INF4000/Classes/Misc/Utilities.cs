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
		public static void AssignUnitToTileByPosition(Vector2i unitPos, Unit unit)
		{					
			Tile t = GameScene.Instance.CurrentMap.Tiles[unitPos.Y, unitPos.X];
			t.CurrentUnit = unit;		
		}
		
		public static void RemoveUnitFromTileByPosition(Vector2i unitPos)
		{					
			Tile t = GameScene.Instance.CurrentMap.Tiles[unitPos.Y, unitPos.X];
			if(t.CurrentUnit != null)
				t.CurrentUnit = null;		
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
		
		public static bool CanUnitAttackFromDestination(Tile tile)
		{
			foreach(Vector2i v in tile.AdjacentPositions){
				if(GameScene.Instance.CurrentMap.GetTile(v) != null)
				{
					Tile t = GameScene.Instance.CurrentMap.GetTile(v);
					if(t.CurrentUnit != null && t.CurrentUnit.OwnerName != GameScene.Instance.ActivePlayer.Name)
						return true;
				}
			}
			return false;
		}
		
		public static int GetUnitActionsChoices(Vector2i pos, Building buil)
		{
			Tile dest = GameScene.Instance.CurrentMap.SelectTileFromPosition(pos);
			bool canAttack = CanUnitAttackFromDestination(dest);
			
			// Same unit as origin unit, either sleep, attack or cancel choice
			if(dest.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name 
			   && !canAttack && buil == null)
				return Constants.ACTION_SLEEP;
			
			else if(dest.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name 
			        && canAttack && buil == null)
				return Constants.ACTION_NOMOVE_ATTACK;
			
			else if(dest.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name 
			   && !canAttack && buil != null && buil.CanProduceThisTurn)
				return Constants.ACTION_PRODUCE;
			
			else if(dest.CurrentUnit.OwnerName == GameScene.Instance.ActivePlayer.Name 
			        && canAttack && buil != null && buil.CanProduceThisTurn)
				return Constants.ACTION_PRODUCE_ATTACK;
			
			return Constants.ACTION_CANCEL;
		}
		#endregion
		
		#region UI Util
		public static void ShowGameUI()
		{
			GameScene.Instance.GameUI.SetActive();
		}
		
		public static void ShowDialogUI()
		{
			GameScene.Instance.DialogUI.SetActive();
		}
		
		public static void ShowActionPanel()
		{
			GameScene.Instance.GameUI.ActionPanel.SetActive(true);
		}
		
		public static void HideActionPanel()
		{
			GameScene.Instance.GameUI.ActionPanel.SetActive(false);
		}
		
		public static void ShowAttackPanel()
		{
			GameScene.Instance.GameUI.ActionPanel.SetActive(true);
		}
		
		public static void HideAttackPanel()
		{
			GameScene.Instance.GameUI.OddsPanel.SetActive(false);
		}
		
		public static void ShowAttackOddsPanel()
		{
			GameScene.Instance.GameUI.OddsPanel.SetActive(true);
		}
		
		public static void ShowStatsPanels()
		{
			GameScene.Instance.GameUI.TileStatsPanel.Panel.Visible = true;
			if(GameScene.Instance.GameUI.UnitStatsPanel.IsActive)
				GameScene.Instance.GameUI.UnitStatsPanel.Panel.Visible = true;
		}
		
		public static void HideStatsPanels()
		{
			GameScene.Instance.GameUI.TileStatsPanel.Panel.Visible = false;
			GameScene.Instance.GameUI.UnitStatsPanel.Panel.Visible = false;
		}
		
		public static void AdjustStatsPanelLocation()
		{
			Camera2D camera = GameScene.Instance.Camera as Camera2D;
			
			if(GameScene.Instance.Cursor.Position.X > camera.Center.X)
			{
				GameScene.Instance.GameUI.TileStatsPanel.SetToLeftOfScreen();
				GameScene.Instance.GameUI.UnitStatsPanel.SetToLeftOfScreen();
				GameScene.Instance.GameUI.ActionPanel.SetToRightOfScreen();
				GameScene.Instance.GameUI.PlayerPanel.SetToRightOfScreen();	
				
			} else {
				GameScene.Instance.GameUI.TileStatsPanel.SetToRightOfScreen();
				GameScene.Instance.GameUI.UnitStatsPanel.SetToRightOfScreen();
				GameScene.Instance.GameUI.ActionPanel.SetToLeftOfScreen();
				GameScene.Instance.GameUI.PlayerPanel.SetToLeftOfScreen();	
			}
			
			if(GameScene.Instance.Cursor.Position.Y > camera.Center.Y + 64)
			{
				GameScene.Instance.GameUI.PlayerPanel.SetBottom();
				
			} else {
				GameScene.Instance.GameUI.PlayerPanel.SetTop();
			}
		}
		#endregion
		
		public static void LoadAllSpritesFromPlayer(List<Player> players, Node parent)
		{
			foreach(Player p in players)
			{
				foreach(Building b in p.Buildings)
				{
					parent.AddChild(b.SpriteTile);
				}
				
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
		
		#region Player Util
		public static Player GetPlayerByName(string name)
		{
			foreach(Player p in GameScene.Instance.Players)
			{
				if(p.Name == name)
					return p;
			}
			return null;
		}
		
		public static void AssignUnitToPlayerByIndex(Unit unit, int playerIndex)
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
		
		public static void AssignUnitToPlayerByName(Unit unit, string name)
		{
			if(unit == null)
				return;
			
			Player p = Utilities.GetPlayerByName(name);
			p.Units.Add(unit);
			unit.OwnerName = p.Name;
		}
		
		public static void AssignBuildingToPlayer(Building build, int playerIndex)
		{
			if(build == null)
				return;
			
			if(playerIndex > 0)
			{
				Player desiredPlayer = GameScene.Instance.Players[playerIndex - 1];
				build.OwnerName = desiredPlayer.Name;
				desiredPlayer.Buildings.Add(build);
			}
			build.SetGraphics();
		}
		
		public static void AssignBuildingToPlayerByName(Building build, string name)
		{
			if(build == null)
				return;
			
			Player p = Utilities.GetPlayerByName(name);
			p.Buildings.Add(build);
			build.OwnerName = p.Name;
			build.SetGraphics();
		}
		
		public static string GetWinner()
		{
			foreach(Player p in GameScene.Instance.Players)
			{
				if(p.Units.Count > 0)
					return p.Name;
			}
			return "";
		}
		#endregion
		
		#region AI Helper methods
		public static bool IsDestinationValid(Vector2i destination)
		{
			
			Tile t = GameScene.Instance.CurrentMap.GetTile(destination);
			if(t.CurrentUnit != null)
				return false;
			else if(!t.IsMoveValid)
				return false;
			
			return true;
		}
		#endregion
	}
}

