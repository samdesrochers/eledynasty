using System;
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
			this.Units = new System.Collections.Generic.List<Unit>();
			this.IsActive = false;
			this.IsHuman = true;
			this.FocusPoints = 5;
			Icon = new ImageAsset("/Application/Assets/Players/araki.png",false);
		}
	}
}

