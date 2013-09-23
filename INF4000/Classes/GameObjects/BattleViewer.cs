using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class BattleViewer : SpriteUV
	{
		public BattleManager BattleManager;
		public float AnimationTime;
		private Vector2 Center;
		
		private SpriteTile AttackerSprite;
		private SpriteTile DefenderSprite;
		
		private SpriteTile TerrainSprite;
		private Vector2i terrainIndex;
		
		private TextImage AttackerDamage;
		private TextImage DefenderDamage;
		
		float SpeedModifier = 0;
		bool FirstPass = true;
		
		public BattleViewer ()
		{
			// Load sprites
			AttackerDamage = new TextImage("-2", new Vector2(0,0), 120);
			DefenderDamage = new TextImage("-5", new Vector2(0,0), 120);
			
			LoadGraphics();
			AnimationTime = 0.0f;
		}
		
		public void SetBattleManager(Unit attacker, Unit defender, Tile targetTile, Tile originTile)
		{	
			BattleManager = new BattleManager(attacker, defender, targetTile, originTile);
			BattleManager.ComputeDamageDealt();
			
			DefenderSprite = BattleManager.AttackingUnit.UnitSprite;
			
			AttackerSprite = new SpriteTile(attacker.UnitSprite.TextureInfo, attacker.UnitSprite.TileIndex2D);
			DefenderSprite = new SpriteTile(defender.UnitSprite.TextureInfo, defender.UnitSprite.TileIndex2D);
			
			AttackerSprite.Quad.S = new Vector2(128,128);
			DefenderSprite.Quad.S = new Vector2(128,128);
			DefenderSprite.FlipU = true;
			//PickCurrentTerrain();
			
			AttackerDamage.Text = "-" + BattleManager.Damages[0].ToString();
			AttackerDamage.Quad.S = new Vector2(92, 92);
			AttackerDamage.Position = new Vector2(Center.X + 100, Center.Y);
			AttackerDamage.Visible = false;
			
			DefenderDamage.Text = "-" + BattleManager.Damages[1].ToString();
			DefenderDamage.Quad.S = new Vector2(92, 92);
			DefenderDamage.Position = new Vector2(Center.X - 100, Center.Y);
			DefenderDamage.Visible = false;
			
			SetGraphicsOverScene();
		}
		
		private void SetGraphicsOverScene()
		{
			Camera2D camera = GameScene.Instance.Camera as Camera2D;
			Center = camera.Center;
			
			// Terrain
			TerrainSprite.Position = new Vector2(Center.X - TerrainSprite.Quad.S.X/2,
			                                     Center.Y - TerrainSprite.Quad.S.Y/2);
			TerrainSprite.Color.A = 0;
			GameScene.Instance.AddChild(TerrainSprite);
			
			// Units
			AttackerSprite.Position = new Vector2(Center.X - 120 - 64, Center.Y - 64);
			DefenderSprite.Position = new Vector2(Center.X + 120 - 64, Center.Y - 64);
			GameScene.Instance.AddChild(DefenderSprite);
			GameScene.Instance.AddChild(AttackerSprite);
			
			// Units HP
			AttackerDamage.UpdatePositionBattle(new Vector2(AttackerSprite.Position.X + 10, AttackerSprite.Position.Y + 50));
			DefenderDamage.UpdatePositionBattle(new Vector2(DefenderSprite.Position.X + 40, DefenderSprite.Position.Y + 50));
			GameScene.Instance.AddChild(AttackerDamage);
			GameScene.Instance.AddChild(DefenderDamage);
		}
		
		private void CleanGraphicsOverScene()
		{	
			GameScene.Instance.RemoveChild(TerrainSprite, false);
			GameScene.Instance.RemoveChild(AttackerSprite, false);
			GameScene.Instance.RemoveChild(DefenderSprite, false);
			GameScene.Instance.RemoveChild(AttackerDamage, false);
			GameScene.Instance.RemoveChild(DefenderDamage, false);

		}
		
		private void LoadGraphics()
		{
			terrainIndex = new Vector2i(0,0);
			
			// Create the tile sprite for specific terrain type
			TerrainSprite = new SpriteTile(AssetsManager.Instance.BattleTextureInfo, terrainIndex);
			TerrainSprite.Quad = this.Quad;
			TerrainSprite.Quad.S = new Vector2(640,320);
		}
		
		private void PickCurrentTerrain()
		{
			switch (BattleManager.ContestedTile.TerrainType) 
			{
				case 0: break;
			}
			TerrainSprite.TileIndex2D = terrainIndex;
		}
		
		private void AttackInGame()
		{
			BattleManager.ExecuteAttack();
			BattleManager.ExecuteCombatOutcome();
			BattleManager.ExecuteFinalizePostCombat();	
		
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			GameScene.Instance.CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
		}
		
		public override void Update(float dt)
		{			
			base.Update(dt);
			AnimationTime += dt;
			
			if(FirstPass)
			{
				GameScene.Instance.UI.SetBattleAnimation();
				FirstPass = false;
			}
					
			if(AnimationTime >= 2.0f) 
			{		
				// Prepare next round
				AnimationTime = 0.0f;			
				SpeedModifier = 0.0f;
				FirstPass = true;
				
				// Remove sprites from scene
				CleanGraphicsOverScene();
				
				// Execute real attack on game map
				AttackInGame();
				
			} else {
				
				// Animate the scene
				if(TerrainSprite.Color.A < 1)
					TerrainSprite.Color.A += 2*dt;
				
				// Animate the units
				if(AnimationTime < 1.0f && AnimationTime > 0.4f && AttackerSprite.Position.X < Center.X + 250 && DefenderSprite.Position.X > Center.X - 250)
				{
					SpeedModifier += dt;
					AttackerSprite.Position = new Vector2(AttackerSprite.Position.X + 800*(dt + SpeedModifier), AttackerSprite.Position.Y);
					DefenderSprite.Position = new Vector2(DefenderSprite.Position.X - 800*(dt + SpeedModifier), DefenderSprite.Position.Y);
				}					
				
				// Animate HP Text
				if(AttackerSprite.Position.X > DefenderSprite.Position.X)
				{
					AttackerDamage.Visible = true;
					DefenderDamage.Visible = true;
					AttackerDamage.Color = new Vector4(1,0.4f,0,AttackerDamage.Alpha);
					DefenderDamage.Color = new Vector4(1,0.4f,0,DefenderDamage.Alpha);
					
					AttackerDamage.Alpha -= 0.5f*dt;
					DefenderDamage.Alpha -= 0.5f*dt;
					
					AttackerDamage.Position = new Vector2(AttackerDamage.Position.X, AttackerDamage.Position.Y + 10*dt);
					DefenderDamage.UpdatePositionBattle(new Vector2(DefenderDamage.Position.X, DefenderDamage.Position.Y + 10*dt));
				}
			} 
		}
	}
}

