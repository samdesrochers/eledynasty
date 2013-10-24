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
		
		private SpriteTile SupportAttackerSprite;
		private SpriteTile SupportDefenderSprite;
		
		private SpriteTile BackgroundSprite;
		private SpriteTile SplashSprite;
		private SpriteTile TerrainSprite;
		private Vector2i terrainIndex;
		
		private TextImage AttackerDamage;
		private TextImage DefenderDamage;
		
		float SpeedModifier = 0;
		bool FirstPass = true;
		bool PlayCombatSound = true;
		
		public BattleViewer ()
		{
			// Load sprites
			AttackerDamage = new TextImage("-2", new Vector2(0,0), 120);
			DefenderDamage = new TextImage("-5", new Vector2(0,0), 120);
			
			LoadGraphics();
			AnimationTime = 0.0f;
		}
		
		public void PrepareBattleAnimation(Unit attacker, Unit defender, Tile targetTile, Tile originTile)
		{	
			PlayCombatSound = true;
			
			BattleManager = new BattleManager(attacker, defender, targetTile, originTile);
			BattleManager.ComputeDamagePercantages();
			
			DefenderSprite = BattleManager.AttackingUnit.UnitSprite;
			
			AttackerSprite = new SpriteTile(attacker.UnitSprite.TextureInfo, attacker.UnitSprite.TileIndex2D);
			DefenderSprite = new SpriteTile(defender.UnitSprite.TextureInfo, defender.UnitSprite.TileIndex2D);
			SupportAttackerSprite = null;
			SupportDefenderSprite = null;
			
			// Support Units (if any)
			Unit supAttack = GetAllyUnitGraphics(originTile, attacker);
			Unit defAttack = GetAllyUnitGraphics(targetTile, defender);
			
			if(supAttack != null) {
				SupportAttackerSprite = new SpriteTile(supAttack.UnitSprite.TextureInfo, supAttack.UnitSprite.TileIndex2D);
				SupportAttackerSprite.Quad.S = new Vector2(60,60);
			}			
			
			if(defAttack != null) {
				SupportDefenderSprite = new SpriteTile(defAttack.UnitSprite.TextureInfo, defAttack.UnitSprite.TileIndex2D);
				SupportDefenderSprite.Quad.S = new Vector2(60,60);
				SupportDefenderSprite.FlipU = true;
			}
			
			AttackerSprite.Quad.S = new Vector2(128,128);
			DefenderSprite.Quad.S = new Vector2(128,128);
			DefenderSprite.FlipU = true;
			//PickCurrentTerrain();
			
			AttackerDamage.Text = "-" + BattleManager.Damages[0].ToString();
			AttackerDamage.Quad.S = new Vector2(92, 92);
			AttackerDamage.Position = new Vector2(Center.X + 100, Center.Y);
			AttackerDamage.Alpha = 1.0f;
			AttackerDamage.Visible = false;
			
			DefenderDamage.Text = "-" + BattleManager.Damages[1].ToString();
			DefenderDamage.Quad.S = new Vector2(92, 92);
			DefenderDamage.Position = new Vector2(Center.X - 100, Center.Y);
			DefenderDamage.Alpha = 1.0f;
			DefenderDamage.Visible = false;
			
			SetGraphicsOverScene();
		}
		
		private void SetGraphicsOverScene()
		{
			Camera2D camera = GameScene.Instance.Camera as Camera2D;
			Center = camera.Center;
			
			//Background
			BackgroundSprite.Position = new Vector2(Center.X - BackgroundSprite.Quad.S.X/2,
			                                        Center.Y - BackgroundSprite.Quad.S.Y/2);
			BackgroundSprite.Color.A = 0;
			GameScene.Instance.AddChild(BackgroundSprite);
			
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
			
			// Support - Attack
			if(SupportAttackerSprite != null) {
				SupportAttackerSprite.Position = new Vector2(Center.X - 284, Center.Y + 40);
				GameScene.Instance.AddChild(SupportAttackerSprite);
			}
			
			if (SupportDefenderSprite != null) {
				SupportDefenderSprite.Position = new Vector2(Center.X + 224, Center.Y + 40);
				GameScene.Instance.AddChild(SupportDefenderSprite);
			}			
			
			// Units HP
			AttackerDamage.UpdatePositionBattle(new Vector2(AttackerSprite.Position.X + 10, AttackerSprite.Position.Y + 50));
			DefenderDamage.UpdatePositionBattle(new Vector2(DefenderSprite.Position.X + 40, DefenderSprite.Position.Y + 50));
			GameScene.Instance.AddChild(AttackerDamage);
			GameScene.Instance.AddChild(DefenderDamage);
			
			// Splash
			SplashSprite.Position = new Vector2(Center.X - SplashSprite.Quad.S.X/2,
                                        		Center.Y - SplashSprite.Quad.S.Y/2);
			GameScene.Instance.AddChild(SplashSprite);		
		}
		
		private void CleanGraphicsOverScene()
		{	
			GameScene.Instance.RemoveChild(BackgroundSprite, false);
			GameScene.Instance.RemoveChild(SplashSprite, false);
			GameScene.Instance.RemoveChild(TerrainSprite, false);
			GameScene.Instance.RemoveChild(AttackerSprite, false);
			GameScene.Instance.RemoveChild(DefenderSprite, false);
			GameScene.Instance.RemoveChild(AttackerDamage, true);
			GameScene.Instance.RemoveChild(DefenderDamage, true);
			GameScene.Instance.RemoveChild(SupportAttackerSprite, true);
			GameScene.Instance.RemoveChild(SupportDefenderSprite, true);
		}
		
		private void LoadGraphics()
		{
			terrainIndex = new Vector2i(0,1);
			
			// Create the tile sprite for specific terrain type and background
			BackgroundSprite = new SpriteTile(AssetsManager.Instance.BattleTextureInfo, new Vector2i(0,0));
			BackgroundSprite.Quad = this.Quad;
			BackgroundSprite.Quad.S = new Vector2(960,544);
			
			TerrainSprite = new SpriteTile(AssetsManager.Instance.BattleTextureInfo, terrainIndex);
			TerrainSprite.Quad = this.Quad;
			TerrainSprite.Quad.S = new Vector2(640,320);
			
			SplashSprite = new SpriteTile(AssetsManager.Instance.BattleTextureInfo, new Vector2i(1,0));
			SplashSprite.Quad = this.Quad;
			SplashSprite.Quad.S = new Vector2(640,320);
			SplashSprite.Color.A = 0;
		}
		
		public override void Update(float dt)
		{			
			base.Update(dt);
			AnimationTime += dt;
			
			if(FirstPass)
			{
				GameScene.Instance.GameUI.SetBattleAnimation();
				FirstPass = false;
			}
					
			if(AnimationTime >= 1.5f) 
			{		
				// Prepare next round
				AnimationTime = 0.0f;			
				SpeedModifier = 0.0f;
				SplashSprite.Color.A = 0.0f;
				FirstPass = true;
				
				// Remove sprites from scene
				CleanGraphicsOverScene();
				
				// Execute real attack on game map
				AttackInGame();
				
				// Reset correct Playing UI
				GameScene.Instance.GameUI.SetPlaying();
				
			} else {
				
				// Animate the scene
				if(TerrainSprite.Color.A < 1) {
					TerrainSprite.Color.A += 2*dt;
					BackgroundSprite.Color.A += 1.2f*dt;
				}
				
				// White Splash
				if(AttackerSprite.Position.X > Center.X - 150 && AttackerSprite.Position.X <= Center.X && SplashSprite.Color.A <= 1) {
					SplashSprite.Color.A += 15*dt;
				} else if(AttackerSprite.Position.X > Center.X && SplashSprite.Color.A >= 0)
				{
					SplashSprite.Color.A -= 5*dt;
				}
				
				// Combat Clash Sound at moment X
				if(PlayCombatSound && AttackerSprite.Position.X > Center.X - 90)
				{
					PlayCombatSound = false;
					SoundManager.Instance.PlaySound(Constants.SOUND_COMBAT);
				}
				
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
					DefenderDamage.Position = new Vector2(DefenderDamage.Position.X, DefenderDamage.Position.Y + 10*dt);
				}
			} 
		}
		
		private void AttackInGame()
		{
			BattleManager.ExecuteAttack();
			BattleManager.ExecuteCombatOutcome();
			BattleManager.ExecuteFinalizePostCombat();	
		
			GameScene.Instance.CurrentGameState = Constants.GAME_STATE_SELECTION_INACTIVE;
			GameScene.Instance.CurrentGlobalState = Constants.GLOBAL_STATE_PLAYING_TURN;
		}
		
		private void PickCurrentTerrain()
		{
			switch (BattleManager.ContestedTile.TerrainType) 
			{
				case 0: break;
			}
			TerrainSprite.TileIndex2D = terrainIndex;
		}
		
		private Unit GetAllyUnitGraphics(Tile t, Unit ally)
		{
			foreach(Vector2i adjPos in t.AdjacentPositions)
			{
				Tile adj = GameScene.Instance.CurrentMap.GetTile(adjPos);
				if(adj.CurrentUnit != null && ally.OwnerName == adj.CurrentUnit.OwnerName)
					return adj.CurrentUnit;
			}
			return null;
		}
	}
}

