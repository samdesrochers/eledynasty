using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace INF4000
{
	public class AIState
	{
		public Vector2i Position;
		public int Heuristic;
		public bool IsOccupied;
		public AIState ()
		{
		}
	}
}

