using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class SpellManager
	{
		public SpellManager ()
		{
			
		}
		
		public void Update()
		{
			if (Input2.GamePad.GetData (0).Cross.Release) 
			{
				MassHeal();
				GameScene.Instance.SpellUI.SetInactive();
			}
			
			if (Input2.GamePad.GetData (0).Circle.Release) 
			{
				SoundManager.Instance.PlaySound(Constants.SOUND_CURSOR_CANCEL);
				GameScene.Instance.SpellUI.SetInactive();
			}
		}
		
		private void MassHeal()
		{
			Player p = GameScene.Instance.ActivePlayer;
			SoundManager.Instance.PlaySound(Constants.SOUND_HEAL);
			
			if(p != null && p.SpellPoints >= Constants.SPELL_MASS_HEAL_COST) 
			{
				p.SpellPoints -= Constants.SPELL_MASS_HEAL_COST;
				foreach(Unit u in p.Units)
				{
					u.LifePoints += Constants.SPELL_MASS_HEAL_EFFECT;
					if(u.LifePoints > Constants.UNIT_HP_FARMER)
						u.LifePoints = Constants.UNIT_HP_FARMER;
					
				}
			}
		}	
	}
}

