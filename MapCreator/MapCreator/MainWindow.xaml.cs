using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string MAP_NAME = "Default Map";

        static int MAP_WIDTH = 17;
        static int MAP_HEIGHT = 10;

        /* TERRAIN EMPTY TILES */
        static string TILE_EMPTY_GRASS        = "1020000000";
        static string TILE_EMPTY_HORI_ROAD    = "1220000000";
        static string TILE_EMPTY_VERT_ROAD    = "1320000000";
        static string TILE_EMPTY_HILL         = "8020000000";
        static string TILE_EMPTY_TREES_1      = "8120000000";
        static string TILE_EMPTY_VH_LEFT_ROAD  = "4020000000";
        static string TILE_EMPTY_VH_RIGHT_ROAD = "4120000000";
        static string TILE_EMPTY_HV_LEFT_ROAD  = "4220000000";
        static string TILE_EMPTY_HV_RIGHT_ROAD = "4320000000";

        /* BUILDINGS EMPTY TILES */
        static string TILE_BUIL_FORTRESS_P1   = "5020100000";
        static string TILE_BUIL_FORTRESS_P2   = "5020200000";
        static string TILE_BUIL_FARM_P1       = "5220100000";
        static string TILE_BUIL_FARM_P2       = "5220200000";

        /* WATER EMPTY TILES */
        static string TILE_EMPTY_WATER_MID    = "6020000000";
        static string TILE_EMPTY_WATER_LFT    = "6120000000";
        static string TILE_EMPTY_WATER_RGT    = "6220000000";
        static string TILE_EMPTY_WATER_UP     = "6320000000";
        static string TILE_EMPTY_WATER_DW     = "6420000000";
        static string TILE_EMPTY_WATER_UP_LFT = "6520000000";
        static string TILE_EMPTY_WATER_UP_RGT = "6620000000";
        static string TILE_EMPTY_WATER_DW_LFT = "6720000000";
        static string TILE_EMPTY_WATER_DW_RGT = "6820000000";
        static string TILE_EMPTY_WATER_CA_HZ  = "6920000000";
        static string TILE_EMPTY_WATER_CA_VT  = "7020000000";
        static string TILE_EMPTY_WATER_M_E_UP_LFT     = "7120000000";
        static string TILE_EMPTY_WATER_M_E_UP_DW_LFT  = "7220000000";
        static string TILE_EMPTY_WATER_L_E_UP_RGT     = "7320000000";
        static string TILE_EMPTY_WATER_UR_E_DW_LFT    = "7420000000";
        static string TILE_EMPTY_WATER_UL_E_DW_RGT    = "7520000000";
        static string TILE_EMPTY_WATER_UP_E_DL        = "7620000000";
        static string TILE_EMPTY_WATER_M_E_UP_R_L     = "7720000000";

        /* UNITS */
        static string TILE_UNIT_FAMRMER_GRASS_P1 = "1021101215";
        static string TILE_UNIT_FAMRMER_GRASS_P2 = "1021201215";


        public MainWindow()
        {
            InitializeComponent();        
        }

        public void GenerateMap()
        {
            List<string> MapTextFileContent = new List<string>();
            string[,] Map = new string[MAP_HEIGHT, MAP_WIDTH];

            // Add Map Name
            MapTextFileContent.Add(MAP_NAME);

            // Add Map Width and Height
            string mapWidth = MAP_WIDTH.ToString();
            string mapHeight = MAP_HEIGHT.ToString();

            MapTextFileContent.Add(mapWidth);
            MapTextFileContent.Add(mapHeight);

            // Add players names
            MapTextFileContent.Add("Kenji");
            MapTextFileContent.Add("Masuka");

            //Add tiles strings to Map
            for (int i = 0; i < MAP_HEIGHT; i++)
            {
                for (int j = 0; j < MAP_WIDTH; j++)
                {
                    Map[i, j] = TILE_EMPTY_GRASS;
                }
            }

            /* MAP 1 - Elisia's Plains */

            // Starting Units
            Map[1,1] = TILE_UNIT_FAMRMER_GRASS_P1;
            Map[1, 9] = TILE_UNIT_FAMRMER_GRASS_P2;

            // Water - Shore
            for (int i = 0; i < MAP_HEIGHT; i++)
                Map[i, MAP_WIDTH - 1] = TILE_EMPTY_WATER_LFT;

            for (int i = 0; i < MAP_WIDTH; i++)
                Map[0, i] = TILE_EMPTY_WATER_UP;

            // Island 
            Map[1, MAP_WIDTH - 6] = TILE_EMPTY_WATER_CA_VT;
            Map[2, MAP_WIDTH - 6] = TILE_EMPTY_WATER_CA_VT;
            Map[3, MAP_WIDTH - 6] = TILE_EMPTY_WATER_UL_E_DW_RGT;
            Map[3, MAP_WIDTH - 5] = TILE_EMPTY_WATER_CA_HZ;
            Map[3, MAP_WIDTH - 4] = TILE_EMPTY_WATER_CA_HZ;
            Map[3, MAP_WIDTH - 3] = TILE_EMPTY_WATER_CA_HZ;
            Map[2, MAP_WIDTH - 2] = TILE_EMPTY_WATER_LFT;
            Map[1, MAP_WIDTH - 2] = TILE_EMPTY_WATER_LFT;

            //Surroundings - Island
            Map[0, MAP_WIDTH - 6] = TILE_EMPTY_WATER_M_E_UP_R_L;
            Map[3, MAP_WIDTH - 2] = TILE_EMPTY_WATER_UP_E_DL;       
            Map[0, MAP_WIDTH - 1] = TILE_EMPTY_WATER_MID;
            Map[1, MAP_WIDTH - 1] = TILE_EMPTY_WATER_MID;
            Map[2, MAP_WIDTH - 1] = TILE_EMPTY_WATER_MID;
            Map[0, MAP_WIDTH - 2] = TILE_EMPTY_WATER_M_E_UP_LFT;
            Map[3, MAP_WIDTH - 1] = TILE_EMPTY_WATER_M_E_UP_LFT;

            // Mountains at top
            Map[MAP_HEIGHT - 1, 0] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 1, 1] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 1, 2] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 2, 0] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 2, 1] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 2, 4] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 2, 5] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 3, 6] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 2, 6] = TILE_EMPTY_HILL;
            Map[MAP_HEIGHT - 2, 7] = TILE_EMPTY_HILL;

            // Forest neat top mountains
            Map[MAP_HEIGHT - 3, 5] = TILE_EMPTY_TREES_1;
            Map[MAP_HEIGHT - 3, 7] = TILE_EMPTY_TREES_1;
            Map[MAP_HEIGHT - 4, 6] = TILE_EMPTY_TREES_1;
            Map[MAP_HEIGHT - 2, 2] = TILE_EMPTY_TREES_1;
            Map[MAP_HEIGHT - 1, 3] = TILE_EMPTY_TREES_1;
            Map[MAP_HEIGHT - 2, 8] = TILE_EMPTY_TREES_1;

            // Kenji Starting village
            Map[4, 1] = TILE_BUIL_FORTRESS_P1;

            // Masuka Starting Village
            Map[8, 13] = TILE_BUIL_FORTRESS_P2;



            // Write Map Content to MapFile
            string rowMessage = "# Row ";
            for (int i = 0; i < MAP_HEIGHT; i++)
            {
                MapTextFileContent.Add(rowMessage + i.ToString());
                for (int j = 0; j < MAP_WIDTH; j++)
                {
                    MapTextFileContent.Add(Map[i, j]);
                }
            }

            try
            {
                System.IO.File.WriteAllLines(MAP_NAME + ".txt", MapTextFileContent);
                this.Text_Status.Text += "Success!";
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GenerateMap();
        }
    }
}
