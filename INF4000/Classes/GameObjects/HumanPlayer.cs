using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class HumanPlayer : Player
	{
		public HumanPlayer (string name)
		{
			this.Name = name;
			this.Units = new System.Collections.Generic.List<Unit>();
			this.IsActive = false;
		}
	}
}

