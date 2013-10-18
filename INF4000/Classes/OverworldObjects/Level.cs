using System;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.Core;

namespace INF4000
{
	public class Level
	{
		public string Name;
		public string Description;
		public string EnemyName;	
		public string GameLevelPath;
		
		public bool IsLocked;
		public int LevelNumber;
		public Vector2 Position;
		
		public ImageAsset EnemyAvatar;
		
		public Level (string name, string description, string enemyName, int levelNo, ImageAsset enemyAvatar)
		{
			IsLocked = false;
			Name = name;
			Description = description;
			EnemyName = enemyName;
			LevelNumber = levelNo;
			EnemyAvatar = enemyAvatar;
		}
	}
}

