using System;
using System.Threading;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public static class GameActions
	{
		#region Unit Actions Preparation
		public static void PrepareUnitMove(Path path)
		{
			// Prepare the Action Panel with Move and Cancel actions only
			GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL);
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			Utilities.AssignMovePathToActiveUnit(path); // Concrete move action
		}
		
		public static void PrepareUnitAttack(Path path)
		{				
			// Prepare the Action Panel with Move Attack and Cancel actions 
			GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK);
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			Utilities.AssignMovePathToActiveUnit(path); // Concrete move action
		}
		
		public static void PrepareUnitSleep()
		{
			// Prepare the Action Panel with Move Attack and Cancel actions 
			GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL);
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			
			// Show Panel
			Utilities.ShowActionPanel();
			GameScene.Instance.UI.ActionPanel.MoveItem.Text_Action.Text = "SLEEP";
		}
		
		public static void PrepareUnitAttackFromOrigin()
		{
			// Prepare the Action Panel with Move Attack and Cancel actions 
			GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_ATTACK);
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			
			// Show Actions Panel
			Utilities.ShowActionPanel();
			GameScene.Instance.UI.ActionPanel.MoveItem.Text_Action.Text = "SLEEP";
		}
		
		public static void PrepareUnitSleepOrProduce()
		{
			// Prepare the Action Panel with Move Attack and Cancel actions 
			GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE);
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			
			// Show Actions Panel
			Utilities.ShowActionPanel();
			GameScene.Instance.UI.ActionPanel.MoveItem.Text_Action.Text = "SLEEP";
		}
			
		public static void PrepareUnitAttackFromOriginOrProduce()
		{
			// Prepare the Action Panel with Move Attack and Cancel actions 
			GameScene.Instance.UI.ActionPanel.SetActiveConfiguration(Constants.UI_ELEMENT_CONFIG_WAIT_CANCEL_PRODUCE_ATTACK);
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ACTIONPANEL_ACTIVE;
			
			// Show Actions Panel
			Utilities.ShowActionPanel();
			GameScene.Instance.UI.ActionPanel.MoveItem.Text_Action.Text = "SLEEP";
		}
		
		public static void CancelUnitMove()
		{
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
				
			// Unselect unit and remove tint from tiles
			GameScene.Instance.ActivePlayer.ActiveUnit.Unselect ();
			GameScene.Instance.ActivePlayer.ActiveUnit = null;
			GameScene.Instance.CurrentMap.UnTintAllTiles ();
			GameScene.Instance.Cursor.TintToWhite();
		}
		#endregion
		
		#region Unit Actions
		public static void TargetEnemyUnits()
		{
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_ATTACKPANEL_ACTIVE;
					
			GameScene.Instance.ActivePlayer.TargetUnits.Clear();
			GameScene.Instance.CurrentMap.SetTargetedTiles(GameScene.Instance.Cursor.SelectedTile);
			GameScene.Instance.ActivePlayer.AssignTarget(0); // Sets first unit in array as target
				
			GameScene.Instance.Cursor.MoveToTileByWorldPosition(GameScene.Instance.ActivePlayer.TargetUnit.WorldPosition);
			GameScene.Instance.Cursor.TintToRed();
			
			// CALCULE LES ODDS AND SHNIZZLES (UTIL) Prepare Attack Panel
			Utilities.ShowAttackOddsPanel();
		}
		
		public static void SleepSelectedUnit()
		{
			// Player is sleeping this unit, inactive state, close UI, sleep unit
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			GameScene.Instance.UI.ActionPanel.SetActive(false);
			
			GameScene.Instance.ActivePlayer.ActiveUnit.Sleep();
			GameScene.Instance.ActivePlayer.ActiveUnit = null;
		}
		
		public static void MoveSelectedUnit()
		{
			// Unit is moved concretly, finalize the action and hide panel
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			GameScene.Instance.UI.ActionPanel.SetActive(false);
			Utilities.FinalizeMoveAction();
		}

		public static void MoveBackToOriginSelectedUnit()
		{
			// Move is canceled, hide panel and reset unit to origin position
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_UNIT_SELECTION_ACTIVE;
			GameScene.Instance.UI.ActionPanel.SetActive(false);
			
			GameScene.Instance.ActivePlayer.ActiveUnit.RevertMove();
			GameScene.Instance.Cursor.MoveToTileByWorldPosition(GameScene.Instance.ActivePlayer.ActiveUnit.WorldPosition);
		}
		
		public static void AttackUnit(Unit attacker, Unit defender, Tile target, Tile origin)
		{
			Utilities.RemoveUnitFromTileByPosition(GameScene.Instance.ActivePlayer.ActiveUnit.Path.Origin);
			BattleManager battle = new BattleManager(attacker, defender,  target, origin);
			
			battle.ExecuteAttack();
			battle.ExecuteCombatOutcome();
			battle.ExecuteFinalizePostCombat();	
			
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
		}
		
		public static void ProduceUnit(Vector2i originPos, string playerName)
		{
			Tile t = GameScene.Instance.CurrentMap.GetTile(originPos);
			if(t.CurrentBuilding != null)
			{
				t.CurrentBuilding.ProduceUnit(t, playerName, GameScene.Instance.ActivePlayer);
			}
			
			// Clear the shit
			Utilities.HideActionPanel();		
			GameScene.Instance.Cursor.TintToWhite();
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			GameScene.Instance.CurrentMap.UnTintAllTiles();
			
			if(GameScene.Instance.ActivePlayer.ActiveUnit != null)
				GameScene.Instance.ActivePlayer.ActiveUnit.Unselect();
			
		}	
		
		#endregion
	}
}

