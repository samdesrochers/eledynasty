using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class HumanPlayer : Player
	{
		public HumanPlayer ()
		{
			this.Name = "";
			this.Units = new List<Unit>();
			this.Buildings = new List<Building>();
			this.IsActive = false;
			this.IsHuman = true;
			this.FocusPoints = 5;
			this.Gold = 5;
			
			this.TargetUnits = new List<Unit>();
			this.Icon = AssetsManager.Instance.Image_Player_1_Icon;
		}
	}
}

