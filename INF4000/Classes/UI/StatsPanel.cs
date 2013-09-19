using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.UI;

namespace INF4000
{
	public class StatsPanel
	{
		public List<ActionItem> StatsItems;
		public Panel Panel;
		
		public StatItem HealthItem;
		public StatItem DefItem;
		public StatItem DamageItem;
		public StatItem GoldItem;
		
		public Label Label_Name;
		public ImageBox Image_Icon;
		public ImageBox Image_Background;
		
		public Vector2 Position;
		public int Configuration;
		
		private bool _IsActive;
		public bool IsActive
		{
			get{ return _IsActive; }
			set
			{ 
				_IsActive = value;
				this.Panel.Visible = _IsActive;
			}
		}
		
		public StatsPanel (Vector2 pos)
		{
			this.Position = pos;
			this.Configuration = 0;
			
			Panel = new Panel();
			Panel.Width = Constants.UI_ELEMENT_STATSBOX_WIDTH;
			Panel.Height = Constants.UI_ELEMENT_STATSBOX_HEIGHT;
			Panel.SetPosition(this.Position.X, this.Position.Y);
					
			DefItem = new StatItem("1", pos, AssetsManager.Instance.Image_Panel_Def_Icon);
			HealthItem = new StatItem("5", pos, AssetsManager.Instance.Image_Panel_HP_Icon);
			DamageItem = new StatItem("4", pos, AssetsManager.Instance.Image_Panel_Dmg_Icon);
			GoldItem = new StatItem("3", pos, AssetsManager.Instance.Image_Panel_Gld_Icon);
			
			Image_Background = new ImageBox();
			Image_Background.Width = Panel.Width;
			Image_Background.Height = Panel.Height;
			Image_Background.Image = AssetsManager.Instance.Image_Stats_Panel_BG;		
			Image_Background.Alpha = 0.6f;
			Image_Background.Visible = true;
			
			Image_Icon = new ImageBox();
			Image_Icon.Width = Constants.TILE_SIZE - 10;
			Image_Icon.Height = Constants.TILE_SIZE - 10;
			Image_Icon.SetPosition(10,35);
			Image_Icon.Image = AssetsManager.Instance.Image_Icon_Grass;		
			Image_Icon.Alpha = 0.9f;
			Image_Icon.Visible = true;
			
			Label_Name = new Label();
			Label_Name.Text = "NAME";
			Label_Name.Width = 150;
			Label_Name.Height = 30;
			Label_Name.SetPosition(10,4);
			Label_Name.Font = AssetsManager.Instance.PixelFont_18;
			Label_Name.Visible = true;
		}
		
		public void SetElements(int def, int dmg, int hp, int gold, string name, int type, string owner)
		{
			Sce.PlayStation.HighLevel.UI.ImageAsset img = null;
			
			switch(type)
			{
				case Constants.TILE_TYPE_GRASS_MIDDLE:
					img = AssetsManager.Instance.Image_Icon_Grass;	
					break;
				case Constants.TILE_TYPE_ROAD_HORIZONTAL:
				case Constants.TILE_TYPE_ROAD_VERTICAL:
				case Constants.TILE_TYPE_ROAD_HV_LEFT:
				case Constants.TILE_TYPE_ROAD_HV_RIGHT:
				case Constants.TILE_TYPE_ROAD_VH_LEFT:
				case Constants.TILE_TYPE_ROAD_VH_RIGHT:
					img = AssetsManager.Instance.Image_Icon_Road;	
					break;
				case Constants.TILE_TYPE_BUILD_FARM:
					img = AssetsManager.Instance.Image_Icon_Farm;	
					break;
				case Constants.TILE_TYPE_BUILD_FORT:
					img = AssetsManager.Instance.Image_Icon_Fort;	
					break;
				case Constants.UNIT_TYPE_FARMER:
					if(owner != Constants.CHAR_KENJI)
						img = AssetsManager.Instance.Image_Icon_Fort;
					else
						img = AssetsManager.Instance.Image_Icon_Farm;
					break;
				case Constants.UNIT_TYPE_SAMURAI:
					img = AssetsManager.Instance.Image_Icon_Fort;	
					break;
				
				default:
					img = AssetsManager.Instance.Image_Icon_Grass;	
					break;
			}
			
			Image_Icon.Image = img;
			Label_Name.Text = name;
			DefItem.Text_Stat.Text = def.ToString();
			HealthItem.Text_Stat.Text = hp.ToString();
			DamageItem.Text_Stat.Text = dmg.ToString();
			GoldItem.Text_Stat.Text = gold.ToString();
		}
		
		public void SetConfiguration(int config)
		{
			ResetPanel();
			this.Configuration = config;
			int Y_offset = 8;
			switch(Configuration)
			{
				case Constants.UI_ELEMENT_CONFIG_STATS_UNIT:
					DefItem.Panel.SetPosition(70, 15 + Y_offset);
					Panel.AddChildLast(DefItem.Panel);
				
					HealthItem.Panel.SetPosition(70, 40 + Y_offset);
					Panel.AddChildLast(HealthItem.Panel);
				
					DamageItem.Panel.SetPosition(70, 65 + Y_offset);
					Panel.AddChildLast(DamageItem.Panel);
				
					Panel.Width = 210;
					break;
				case Constants.UI_ELEMENT_CONFIG_STATS_BUILDING:
					DefItem.Panel.SetPosition(70, 25 + Y_offset);			
					Panel.AddChildLast(DefItem.Panel);
				
					GoldItem.Panel.SetPosition(70, 55 + Y_offset);
					Panel.AddChildLast(GoldItem.Panel);
				
					Panel.Width = 210;
					break;
				case Constants.UI_ELEMENT_CONFIG_STATS_TERRAIN:
					DefItem.Panel.SetPosition(70, 40 + Y_offset);
					Panel.AddChildLast(DefItem.Panel);
					Panel.Width = 210;
					break;
			}
		}
		
		private void ResetPanel()
		{
			Panel.RemoveChild(Image_Background);
			Panel.RemoveChild(Image_Icon);
			Panel.RemoveChild(Label_Name);
			Panel.RemoveChild(DefItem.Panel);
			Panel.RemoveChild(DamageItem.Panel);
			Panel.RemoveChild(HealthItem.Panel);
			Panel.RemoveChild(GoldItem.Panel);
			
			Panel.AddChildLast(Image_Background);
			
			if(Image_Icon != null) //TEMP
				Panel.AddChildLast(Image_Icon);
			
			Panel.AddChildLast(Label_Name);
		}
	}
}

