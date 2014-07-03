using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace AssetsCU
{

    public enum MovementType
    {
        Foot, Treads, TreadsAmphi, Wheels, WheelsTraverse, Flight, Immobile
    }
    public class PlusVoxels
    {
        public static string[] Terrains = new string[]
        {"Plains","Forest","Desert","Jungle","Hills"
        ,"Mountains","Ruins","Tundra","Road","River"};
        public static string[] Directions = { "SE", "SW", "NW", "NE" };
        //foot 0-0, treads 1-5, wheels 6-8, flight 9-10
        public static string[] CurrentUnits = {
"Infantry", "Infantry_P", "Infantry_S", "Infantry_T",
"Artillery", "Artillery_P", "Artillery_S", "Artillery_T",
"Tank", "Tank_P", "Tank_S", "Tank_T",
"Plane", "Plane_P", "Plane_S", "Plane_T",
"Supply", "Supply_P", "Supply_S", "Supply_T",
"Copter", "Copter_P", "Copter_S", "Copter_T", 
"City", "Factory", "Airport", "Laboratory", "Castle", "Estate"};
        public static Dictionary<string, int> UnitLookup = new Dictionary<string, int>(30), TerrainLookup = new Dictionary<string, int>(10);
        public static Dictionary<MovementType, List<int>> MobilityToUnits = new Dictionary<MovementType, List<int>>(30), MobilityToTerrains = new Dictionary<MovementType, List<int>>();
        public static List<int>[] TerrainToUnits = new List<int>[30];
        public static Dictionary<int, List<MovementType>> TerrainToMobilities = new Dictionary<int, List<MovementType>>();
        public static int[] CurrentSpeeds = {
3, 3, 5, 3,
4, 3, 6, 4,
6, 4, 7, 6,
7, 5, 9, 8,
5, 5, 6, 6,
7, 5, 8, 7, 
0,0,0,0,0,0};
        public static MovementType[] CurrentMobilities = {
MovementType.Foot, MovementType.Foot, MovementType.WheelsTraverse, MovementType.Foot,
MovementType.Treads, MovementType.Treads, MovementType.Treads, MovementType.Wheels,
MovementType.Treads, MovementType.Treads, MovementType.Treads, MovementType.TreadsAmphi,
MovementType.Flight, MovementType.Flight, MovementType.Flight, MovementType.Flight,
MovementType.Wheels, MovementType.Treads, MovementType.TreadsAmphi, MovementType.Wheels,
MovementType.Flight, MovementType.Flight, MovementType.Flight, MovementType.Flight, 
MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, MovementType.Immobile, 
                                                         };

        public static void Initialize()
        {
            MovementType[] values = (MovementType[])Enum.GetValues(typeof(MovementType));
            foreach (MovementType v in values)
            {
                MobilityToUnits[v] = new List<int>();
            }
            for (int t = 0; t < Terrains.Length; t++)
            {
                TerrainLookup[Terrains[t]] = t;
                TerrainToMobilities[t] = new List<MovementType>();
                TerrainToUnits[t] = new List<int>();
            }
            for (int i = 0; i < CurrentUnits.Length; i++)
            {
                UnitLookup[CurrentUnits[i]] = i;
                MobilityToUnits[CurrentMobilities[i]].Add(i);
            }
            MobilityToTerrains[MovementType.Flight] =
                new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MobilityToTerrains[MovementType.Foot] =
                new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            MobilityToTerrains[MovementType.Treads] =
                new List<int>() { 0, 1, 2, 3, 6, 7, 8 };
            MobilityToTerrains[MovementType.TreadsAmphi] =
                new List<int>() { 0, 1, 2, 3, 6, 7, 8, 9 };
            MobilityToTerrains[MovementType.Wheels] =
                new List<int>() { 0, 2, 7, 8, };
            MobilityToTerrains[MovementType.WheelsTraverse] =
                new List<int>() { 0, 1, 2, 3, 6, 7, 8 };
            MobilityToTerrains[MovementType.Immobile] =
                new List<int>() { };

            foreach (var kv in MobilityToTerrains)
            {
                foreach (int t in kv.Value)
                {
                    TerrainToMobilities[t].Add(kv.Key);
                }
            }
            foreach (var kv in TerrainToMobilities)
            {
                foreach (MovementType m in kv.Value)
                    TerrainToUnits[kv.Key].AddRange(MobilityToUnits[m]);
                TerrainToUnits[kv.Key] = TerrainToUnits[kv.Key].Distinct().ToList();
            }
        }

        /*{ "Infantry", //foot 0 0
                               "Tank", "Artillery", "Artillery_P", "Artillery_S", "Supply_P", //treads 1 5
                               "Artillery_T", "Supply", "Supply_T", //wheels 6 8
                               "Helicopter", "Plane", //flight 9 10
                               "City", "Factory", "Castle", "Capital" //facility
                             };*/
        public static string[] final_units =
        {
            "Infantry",
"Bazooka",
"Bike",
"Sniper",
"Light Artillery",
"Defensive Artillery",
"AA Artillery",
"Stealth Artillery",
"Light Tank",
"Heavy Tank",
"AA Tank",
"Recon Tank",
"Prop Plane",
"Ground Bomber",
"Fighter Jet",
"Stealth Bomber",
"Supply Truck",
"Rig",
"Amphi Transport",
"Jammer",
"Transport Copter",
"Gunship Copter",
"Blitz Copter",
"Comm Copter",
};

        public class UnitInfo
        {
            public int unit;
            public string name;
            public int color;
            public int facing;

            public int speed;
            public MovementType mobility;
            public int x;
            public int y;
            public UnitInfo()
            {
                unit = 0;
                name = "Infantry";
                speed = 3;
                mobility = MovementType.Foot;
                color = 1;
                facing = 0;
                x = 3;
                y = 3;
            }
            public UnitInfo(string name, int color, int facing, int x, int y)
            {
                this.name = name;
                this.x = x;
                this.y = y;
                //                this.unit = index_matches[unit];
                this.unit = UnitLookup[name];
                this.color = color;
                this.facing = facing;
                this.speed = CurrentSpeeds[this.unit];
                this.mobility = CurrentMobilities[this.unit];
            }
            public UnitInfo(int unit, int color, int facing, int x, int y)
            {
                this.name = CurrentUnits[unit];
                this.x = x;
                this.y = y;
                //                this.unit = index_matches[unit];
                this.unit = unit;
                this.color = color;
                this.facing = facing;
                this.speed = CurrentSpeeds[this.unit];
                this.mobility = CurrentMobilities[this.unit];
            }
        }
        private static Random r = new Random(PlusPaletteDraw.r.Next());
        private static float flat_alpha = 0.9991F;
        private static float spin_alpha_0 = 0.9993F;
        private static float spin_alpha_1 = 0.9995F;
        public static float[][] colors = new float[][]
        {
            //0 tires, tread
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
            new float[] {0.23F,0.2F,0.2F,1F},
//            new float[] {0.3F,0.3F,0.33F,1F},
            
            //8 mud, wood
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.2F,0.4F,0.3F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            new float[] {0.4F,0.25F,0.15F,1F},
            //new float[] {0.4F,0.3F,0.2F,1F},
            
            //16 gun barrel
            new float[] {0.4F,0.35F,0.5F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            
            //24 gun peripheral (sights, trigger)
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.6F,0.8F,1F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            
            //32 main paint
            new float[] {0.3F,0.3F,0.3F,1F},     //black
            new float[] {0.79F,0.76F,0.7F,1F},  //white
            new float[] {0.86F,0.19F,0.1F,1F},         //red
            new float[] {1.1F,0.45F,0F,1F},      //orange
            new float[] {0.85F,0.71F,0.1F,1F},         //yellow
            new float[] {0.2F,0.7F,0.1F,1F},    //green
            new float[] {0.15F,0.25F,0.8F,1F},       //blue
            new float[] {0.4F,0.1F,0.45F,1F},       //purple
            
            //40 doors
            new float[] {0.6F,0.05F,-0.1F,1F},         //black
            new float[] {0.5F,0.6F,0.7F,1F},         //white
            new float[] {0.7F,-0.05F,-0.1F,1F},       //red
            new float[] {0.7F,0.25F,0.05F,1F},       //orange
            new float[] {0.63F,0.51F,-0.15F,1F},   //yellow
            new float[] {0.0F,0.45F,-0.1F,1F},     //green
            new float[] {0.1F,0.1F,0.5F,1F},       //blue
            new float[] {0.65F,0.15F,0.6F,1F},       //purple
            
            //48 cockpit
            new float[] {0.5F,0.5F,0.4F,1F},     //black
            new float[] {0.63F,0.55F,0.8F,1F},   //white
            new float[] {0.55F,0.36F,0.2F,1F}, // {0.9F,0.5F,0.4F,1F},     //red
            new float[] {0.87F,0.5F,0.2F,1F},    //orange
            new float[] {0.68F,0.61F,0.3F,1F},     //yellow
            new float[] {0.45F,0.25F,0.0F,1F},     //green
            new float[] {0.4F,0.45F,0.7F,1F},     //blue
            new float[] {0.55F,0.4F,0.65F,1F},   //purple

            //56 helmet
            new float[] {0.2F,0.15F,0.1F,1F},     //black
            new float[] {0.69F,0.66F,0.6F,1F},       //white
            new float[] {1F,0.15F,0.05F,1F},     //red
            new float[] {0.95F,0.35F,0.05F,1F},       //orange
            new float[] {0.68F,0.55F,0.2F,1F},         //yellow
            new float[] {0.29F,0.5F,0.2F,1F},       //green
            new float[] {0.2F,0.25F,0.5F,1F},       //blue
            new float[] {0.5F,0.3F,0.5F,1F},       //purple
            
            //64 flesh
            new float[] {0.87F,0.65F,0.3F,1F},  //black
            new float[] {0.7F,1.2F,-0.1F,1F},      //white
            new float[] {0.87F,0.65F,0.3F,1F},  //red
            new float[] {0.87F,0.65F,0.3F,1F},  //orange
            new float[] {0.87F,0.65F,0.3F,1F},  //yellow
            new float[] {0.87F,0.65F,0.3F,1F},  //green
            new float[] {0.87F,0.65F,0.3F,1F},  //blue
            new float[] {0.87F,0.65F,0.3F,1F},  //purple
            //OLD new float[] {1.1F,0.89F,0.55F,1F},  //normal
            //WEIRD new float[] {0.55F,0.8F,-0.3F,1F},      //white
            
            //72 exposed metal
            new float[] {0.69F,0.62F,0.56F,1F},     //black
            new float[] {0.75F,0.75F,0.85F,1F},     //white
            new float[] {0.69F,0.62F,0.56F,1F},     //red
            new float[] {0.69F,0.62F,0.56F,1F},     //orange
            new float[] {0.69F,0.62F,0.56F,1F},     //yellow
            new float[] {0.69F,0.62F,0.56F,1F},     //green
            new float[] {0.69F,0.62F,0.56F,1F},     //blue
            new float[] {0.69F,0.62F,0.56F,1F},     //purple

            //80 lights
            new float[] {1.0F,1.0F,0.5F,1F},        //black
            new float[] {0.5F,1.2F,0.6F,1F},         //white
            new float[] {1.1F,0.9F,0.5F,1F},        //red
            new float[] {1.1F,0.9F,0.5F,1F},        //orange
            new float[] {1.05F,0.65F,0.15F,1F},        //yellow
            new float[] {0.9F,1.1F,0.5F,1F},        //green
            new float[] {0.9F,0.8F,0.65F,1F},        //blue
            new float[] {0.95F,0.9F,0.45F,1F},        //purple

            //88 windows
            new float[] {0.3F,0.5F,0.5F,1F},
            new float[] {0.3F,1F,0.9F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},
            new float[] {0.35F,0.3F,0.25F,1F},
            new float[] {0.35F,0.3F,0.25F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},
            new float[] {0.45F,0.7F,0.7F,1F},

            //96 shadow (FLAT ALPHA)
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            new float[] {0.1F,0.1F,0.1F,flat_alpha},
            
            //104 whirling rotors FRAME 0(SPIN ALPHA FRAME 0)
            new float[] {0.65F,0.65F,0.65F,spin_alpha_0},     //black
            new float[] {0.65F,0.7F,0.7F,spin_alpha_0},  //white
            new float[] {0.8F,0.25F,0.25F,spin_alpha_0},         //red
            new float[] {0.8F,0.5F,0.1F,spin_alpha_0},      //orange
            new float[] {0.85F,0.85F,0.35F,spin_alpha_0},         //yellow
            new float[] {0.4F,0.6F,0.35F,spin_alpha_0},    //green
            new float[] {0.55F,0.5F,0.95F,spin_alpha_0},       //blue
            new float[] {0.65F,0.35F,0.65F,spin_alpha_0},       //purple

            //112 whirling rotors FRAME 1(SPIN ALPHA FRAME 1)
            new float[] {0.65F,0.65F,0.65F,spin_alpha_1},     //black
            new float[] {0.65F,0.7F,0.7F,spin_alpha_1},  //white
            new float[] {0.8F,0.25F,0.25F,spin_alpha_1},         //red
            new float[] {0.8F,0.5F,0.1F,spin_alpha_1},      //orange
            new float[] {0.85F,0.85F,0.35F,spin_alpha_1},         //yellow
            new float[] {0.4F,0.6F,0.35F,spin_alpha_1},    //green
            new float[] {0.55F,0.5F,0.95F,spin_alpha_1},       //blue
            new float[] {0.65F,0.35F,0.65F,spin_alpha_1},       //purple
            /*
            new float[] {0.6F,0.6F,0.6F,0.21F},     //black
            new float[] {1.25F,1.35F,1.45F,0.21F},  //white
            new float[] {1.3F,0.25F,0.3F,0.21F},         //red
            new float[] {1.2F,0.6F,0.3F,0.21F},      //orange
            new float[] {1.4F,1.4F,0.6F,0.21F},         //yellow
            new float[] {0.4F,1.5F,0.4F,0.21F},    //green
            new float[] {0.4F,0.4F,1.4F,0.21F},       //blue
            new float[] {1.1F,0.2F,1.1F,0.21F},       //purple
            */
            //120 inner shadow (NO ALPHA)
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            new float[] {0.13F,0.10F,0.04F,1F},
            
            //128 water splash (FLAT ALPHA)
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},
            new float[] {0.4F,0.6F,0.9F,flat_alpha},

            //junk, 136-255
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            //8 final garbage
/*            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},*/
        };
        public static float[][] colours = new float[256][];
        private struct MagicaVoxelData
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelData(BinaryReader stream, bool subsample)
            {
                x = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                y = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                z = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                color = stream.ReadByte();
            }
        }
        private static int sizex = 0, sizey = 0, sizez = 0;
        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <param name="overrideColors">Optional color lookup table for converting RGB values into my internal engine color format.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        private static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];

            MagicaVoxelData[] voxelData = null;

            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();

                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        voxelData = new MagicaVoxelData[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelData(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        //colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;

                    // use the voxColors array by default, or overrideColor if it is available
                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }
            }

            return voxelData;
        }


        private static bool PalettesInitialized = false;
        private static List<Color>[] SDPalettes = new List<Color>[9];
        public static void InitializePalettes()
        {
            Bitmap b;
            b = new Bitmap(256, 8, PixelFormat.Format32bppArgb);
            for (int c = 0; c < 9; c++)
            {
                Bitmap bmp;
                if (c == 8)
                    bmp = new Bitmap("PaletteCrazy.png");
                else
                    bmp = new Bitmap("PaletteColor" + c + ".png");
                SDPalettes[c] = new List<Color>();
                for (int i = 0; i < bmp.Width; i+=(i%3)+1)
                {
                    SDPalettes[c].Add(bmp.GetPixel(i, 0));
                    SDPalettes[c].Add(bmp.GetPixel(i, 3));
                    
                }
                if(c == 8)
                {
                    b.Save("Palettes/octa.png", ImageFormat.Png);
                    b.Dispose();
                    bmp.Dispose();
                    break;
                }
                for (int cl = 0; cl < SDPalettes[c].Count; cl++)
                {
                    b.SetPixel(cl, c, SDPalettes[c][cl]);
                }
                for (int cl = SDPalettes[c].Count; cl < 256; cl++)
                {
                    b.SetPixel(cl, c, Color.Black);
                }
                Console.WriteLine("Color "+ c + " has: " + SDPalettes[c].Count + " entries.");
                //SDPalettes[c] = colors;
                
                bmp.Dispose();
            }
            GC.Collect();
            PalettesInitialized = true;

        }
        public static void Madden()
        {
            for (int i = 0; i < 256; i++)
            {
                colours[i] = colors[i];
            }

            for (int i = 1; i < 18; i++)
            {
                float alpha = 1F;
                switch (i)
                {
                    case 17: alpha = flat_alpha; break;
                    case 13: alpha = flat_alpha; break;
                    case 14: alpha = spin_alpha_0; break;
                    case 15: alpha = spin_alpha_1; break;
                }
                if (i == 11)
                {
                    colors[(i - 1) * 8 + 0] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 1] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 2] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 3] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 4] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 5] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 6] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                    colors[(i - 1) * 8 + 7] = new float[] { 1.0F, 1.0F, 0.5F, 1F };
                }
                else
                {
                    colors[(i - 1) * 8 + 0] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 1] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 2] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 3] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 4] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 5] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 6] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                    colors[(i - 1) * 8 + 7] = new float[] { 1 / i, 1F / i * 1.5F, 0.9F / (18 - i), alpha };
                }
            }
            for (int i = 136; i < 256; i++)
            {
                colors[i] = new float[] { 0, 0, 0, 1 };
            }
        }
        public static void Awaken()
        {
            for (int i = 0; i < 256; i++)
            {
                colors[i] = colours[i];
            }
        }
        public static void CreateChannelBitmap(Bitmap bmp, string savename)
        {
            if (PalettesInitialized == false)
                return;
            Bitmap b = new Bitmap(bmp);
            //byte[] array = new byte[bmp.Width * bmp.Height];

            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    int cb = (SDPalettes[8].FindByIndex(bmp.GetPixel(x, y)));
                    if (cb != -1)
                        b.SetPixel(x, y, Color.FromArgb(cb, 0, 0));
                    else
                        Console.WriteLine("Color not found at: " + x + ", " + y);

                        //array[bmp.Width * y + x] = (byte)cb;
                }
            b.Save(savename, ImageFormat.Png);
            //BitmapSource bitmap = BitmapSource.Create(bmp.Width, bmp.Height, 96, 96, Media.PixelFormats.Indexed8, Palettes[palette], array, bmp.Width);
            /*
            using (FileStream stream = new FileStream(savename, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // encoder.Palette = Palettes[palette];
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }*/
        }

        private static Bitmap render(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            int xSize = 20, ySize = 20;
            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch (facing)
            {
                case 0:
                    vls = voxels;
                    break;
                case 1:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 2:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 3:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }
            int jitter = (frame % 3) + (frame / 3);
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                // Console.Write(current_color + "  ");
                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;

                if (current_color == 80) //lights
                {
                    float lightCalc = (0.5F - ((frame % 3) + (frame / 3))) * 0.12F;
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0] + lightCalc,  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1] + lightCalc,  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2] + lightCalc,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage((current_color == 80) ? spin :
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : jitter)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap renderOutline(MagicaVoxelData[] voxels, int facing, int faction, int frame, int maxFrames)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            int xSize = 20, ySize = 20;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch (facing)
            {
                case 0:
                    vls = voxels;
                    break;
                case 1:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 2:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case 3:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }

            int jitter = (frame % 3) + (frame / 3);
            if (maxFrames != 4)
                jitter = 0;
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if ((frame % 2 != 0) && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if ((frame % 2 != 1) && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - jitter, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }
        private static Bitmap renderOnlyColors()
        {
            Bitmap b = new Bitmap(128, 4, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube_soft.png");
            //            Image gray = new Bitmap("cube_gray_soft.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            float[][] flatcolors = PlusPaletteDraw.flatcolors;
            for (int color = 0; color < 11; color++)
            {
                //g.DrawImage(image, 10, 10, width, height);
                float merged = (flatcolors[color][0] + flatcolors[color][1] + flatcolors[color][2]) * 0.45F;


                ColorMatrix colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {(merged + flatcolors[color][0]) * 0.5F,  0,  0,  0, 0},
   new float[] {0,  (merged + flatcolors[color][1]) * 0.5F,  0,  0, 0},
   new float[] {0,  0,  (merged + flatcolors[color][2]) * 0.5F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                ColorMatrix colorMatrixDark = new ColorMatrix(new float[][]{ 
   new float[] {merged*0.3F + flatcolors[color][0] * 0.5F,  0,  0,  0, 0},
   new float[] {0,  merged*0.3F + flatcolors[color][1] * 0.52F,  0,  0, 0},
   new float[] {0,  0,  merged*0.3F + flatcolors[color][2] * 0.58F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                ColorMatrix colorMatrixBright = new ColorMatrix(new float[][]{ 
   new float[] {merged*0.55F + flatcolors[color][0] * 0.85F,  0,  0,  0, 0},
   new float[] {0,  merged*0.55F + flatcolors[color][1] * 0.85F,  0,  0, 0},
   new float[] {0,  0,  merged*0.55F + flatcolors[color][2] * 0.85F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                  image,
                   new Rectangle(1 + 9 * color, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                imageAttributes.SetColorMatrix(
                   colorMatrixDark,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                  image,
                   new Rectangle(4 + 9 * color, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                imageAttributes.SetColorMatrix(
                   colorMatrixBright,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                  image,
                   new Rectangle(7 + 9 * color, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }
        private static Bitmap renderOnlyColors(int faction)
        {
            Bitmap b = new Bitmap(128, 4, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            Image outline = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;
            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            //            if (maxFrames != 4)
            //              jitter = 0;
            for (int v = 0; v < 10; v++)
            {
                int current_color = v * 8;

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle(1+3*v, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            float[] lightCalcs = {
                                             (0.5F - (0 - 0)) * 0.12F,
                                             (0.5F - (1 - 0)) * 0.12F,
                                             (0.5F - (2 - 0)) * 0.12F,
                                             (0.5F - (0 + 1)) * 0.12F,};
            for(int i = 0; i < 4; i++)
            {
                        colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[80 + faction][0] + lightCalcs[i],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[80 + faction][1] + lightCalcs[i],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[80 + faction][2] + lightCalcs[i],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                        imageAttributes.SetColorMatrix(
                           colorMatrix,
                           ColorMatrixFlag.Default,
                           ColorAdjustType.Bitmap);
                        g.DrawImage(spin, new Rectangle(1 + 3 * (10 + i), 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes);
            }
            for (int v = 11; v < 17; v++)
            {
                int current_color = v * 8;

                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle(1 + 3 * (v+3), 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}});
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            g.DrawImage(outline,
               new Rectangle(1 + 3 * 20, 0,
                   8, 8),  // destination rectangle 
                //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
               1, 1,        // upper-left corner of source rectangle 
               8,       // width of source rectangle
               8,      // height of source rectangle
               GraphicsUnit.Pixel,
               imageAttributes);

            return b;
        }



        private static Bitmap drawPixelsSE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsSW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(80, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_soft.png");
            Image flat = new Bitmap("flat_soft.png");
            Image spin = new Bitmap("spin_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color + faction][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color + faction][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color + faction][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   (colors[current_color + faction][3] == 1F) ? image : (colors[current_color + faction][3] == flat_alpha) ? flat : spin,
                   new Rectangle((vx.x + vx.y) * 2, 100 - 24 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                       , width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }







        private static Bitmap drawOutlineSE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }
        private static Bitmap drawOutlineSW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            } foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }

        private static Bitmap drawOutlineNE(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            } foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2)
                           , width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }


        private static Bitmap drawOutlineNW(MagicaVoxelData[] voxels, int faction, int frame)
        {
            Bitmap b = new Bitmap(88, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_soft.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;

                if (frame != 0 && colors[current_color + faction][3] == spin_alpha_0)
                    continue;
                else if (frame != 1 && colors[current_color + faction][3] == spin_alpha_1)
                    continue;
                if (colors[current_color + faction][3] != flat_alpha)
                {
                    /*colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[56 + idx][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[56 + idx][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[56 + idx][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});*/

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);
                    g.DrawImage(
                       image,
                       new Rectangle((vx.x + vx.y) * 2 + 2, 100 - 20 - 2 - vx.y + vx.x - vx.z * 3 - ((colors[current_color + faction][3] == flat_alpha) ? -2 : frame * 2), width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
                }
            }
            return b;
        }


        private static void processUnitBasic(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

            for (int i = 0; i < 8; i++)
            {
                System.IO.Directory.CreateDirectory(u);
                for (int face = 0; face < 4; face++)
                {
                    Bitmap b = render(parsed, face, i, 0, 4);
                    b.Save(u + "/color" + i + "_face" + face + "_" + Directions[face] + "_frame" + 0 + "_.png", ImageFormat.Png);
                }
            }
            bin.Close();

        }
        private static Bitmap processSingleOutlined(MagicaVoxelData[] parsed, int color, string dir, int frame, int maxFrames)
        {
            Graphics g;
            Bitmap b, o;
            int d = 0;
            switch (dir)
            {
                case "SE":
                    break;
                case "SW": d = 1;
                    break;
                case "NW": d = 2;
                    break;
                case "NE": d = 3;
                    break;
                default:
                    break;
            }

            b = render(parsed, d, color, frame, maxFrames);
            o = renderOutline(parsed, d, color, frame, maxFrames);
            g = Graphics.FromImage(o);
            g.DrawImage(b, 4, 4);
            return o;
        }
        private static void processUnitOutlined(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);
            int framelimit = 4;
            if (!UnitLookup.ContainsKey(u))
            {
                framelimit = 4;
                Bitmap b = new Bitmap(56, 108);
                Graphics g = Graphics.FromImage(b);
                for (int i = 0; i < 8; i++)
                {
                    for (int f = 0; f < framelimit; f++)
                    {
                        g.DrawImage(processSingleOutlined(parsed, i, "SE", f, framelimit), -16, (f - 2) * 26);
                    }
                    b.Save("color" + i + "_" + u + ".png", ImageFormat.Png);
                }
                bin.Close();
                return;
            }
            else if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

            for (int i = 0; i < 8; i++)
            {
                string folder = ("color" + i);//"color" + i;
                System.IO.Directory.CreateDirectory(folder); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"

                    processSingleOutlined(parsed, i, "SE", f, framelimit).Save(folder + "/" + u + "_face0" + "_" + f + ".png", ImageFormat.Png); //se
                    processSingleOutlined(parsed, i, "SW", f, framelimit).Save(folder + "/" + u + "_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                    processSingleOutlined(parsed, i, "NW", f, framelimit).Save(folder + "/" + u + "_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                    processSingleOutlined(parsed, i, "NE", f, framelimit).Save(folder + "/" + u + "_face3" + "_" + f + ".png", ImageFormat.Png); //ne

                }

            }
            /*
            System.IO.Directory.CreateDirectory("gifs");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "-dispose background -delay 25 -loop 0 images/"+u+"_color* gifs/" + u + "_animated.gif";
            Process.Start(startInfo).WaitForExit();
            */
            bin.Close();

            if (File.Exists(u + "_Firing_X.vox"))
            {
                Console.WriteLine("Processing: " + u + " Firing");
                bin = new BinaryReader(File.Open(u + "_Firing_X.vox", FileMode.Open));
                parsed = FromMagica(bin);
                framelimit = 4;
                if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
                {
                    framelimit = 2;
                }
                for (int i = 0; i < 1; i++)
                {

                    string folder = "color" + i; ;//
                    //System.IO.Directory.CreateDirectory(u);
                    for (int f = 0; f < framelimit; f++)
                    {

                        processSingleOutlined(parsed, i, "SE", f, framelimit).Save(folder + "/" + u + "_Firing" + "_face0" + "_" + f + ".png", ImageFormat.Png); //se
                        processSingleOutlined(parsed, i, "SW", f, framelimit).Save(folder + "/" + u + "_Firing" + "_face1" + "_" + f + ".png", ImageFormat.Png); //sw
                        processSingleOutlined(parsed, i, "NW", f, framelimit).Save(folder + "/" + u + "_Firing" + "_face2" + "_" + f + ".png", ImageFormat.Png); //nw
                        processSingleOutlined(parsed, i, "NE", f, framelimit).Save(folder + "/" + u + "_Firing" + "_face3" + "_" + f + ".png", ImageFormat.Png); //ne

                    }

                }
                /*
                System.IO.Directory.CreateDirectory("gifs");
                startInfo.Arguments = "-dispose background -delay 25 -loop 0 images/" + u + "_color* gifs/" + u + "_animated.gif";
                Process.Start(startInfo).WaitForExit();
                */
                bin.Close();
            }
        }

        private static void processUnitChannel(string u)
        {

            Console.WriteLine("Processing: " + u);
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);
            int framelimit = 4;
            if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
            {
                framelimit = 2;
            }

                System.IO.Directory.CreateDirectory("indexed"); //("color" + i);
                for (int f = 0; f < framelimit; f++)
                { //"color" + i + "/"

                    CreateChannelBitmap(processSingleOutlined(parsed, 0, "SE", f, framelimit), "indexed/" + u + "_face0" + "_" + f + ".png");
                    CreateChannelBitmap(processSingleOutlined(parsed, 0, "SW", f, framelimit), "indexed/" + u + "_face1" + "_" + f + ".png");
                    CreateChannelBitmap(processSingleOutlined(parsed, 0, "NW", f, framelimit), "indexed/" + u + "_face2" + "_" + f + ".png");
                    CreateChannelBitmap(processSingleOutlined(parsed, 0, "NE", f, framelimit), "indexed/" + u + "_face3" + "_" + f + ".png");

                }
            bin.Close();

            if (File.Exists(u + "_Firing_X.vox"))
            {
                Console.WriteLine("Processing: " + u + " Firing");
                bin = new BinaryReader(File.Open(u + "_Firing_X.vox", FileMode.Open));
                parsed = FromMagica(bin);
                framelimit = 4;
                if (CurrentMobilities[UnitLookup[u]] == MovementType.Immobile)
                {
                    framelimit = 2;
                }

                    //System.IO.Directory.CreateDirectory(u);
                    for (int f = 0; f < framelimit; f++)
                    {
                        CreateChannelBitmap(processSingleOutlined(parsed, 0, "SE", f, framelimit), "indexed/" + u + "_Firing" + "_face0" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 0, "SW", f, framelimit), "indexed/" + u + "_Firing" + "_face1" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 0, "NW", f, framelimit), "indexed/" + u + "_Firing" + "_face2" + "_" + f + ".png");
                        CreateChannelBitmap(processSingleOutlined(parsed, 0, "NE", f, framelimit), "indexed/" + u + "_Firing" + "_face3" + "_" + f + ".png");

                    }

                
                bin.Close();
            }
        }
        /*        private static void processBases()
                {
                    BinaryReader[] powers = new BinaryReader[8];
                    BinaryReader[] speeds = new BinaryReader[8];
                    BinaryReader[] techniques = new BinaryReader[8];


                    MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

                    for (int i = 0; i < 8; i++)
                    {
                        powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                        basepowers[i] = FromMagica(powers[i]);
                        speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                        basespeeds[i] = FromMagica(speeds[i]);
                        techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                        basetechniques[i] = FromMagica(techniques[i]);

                    }

                    System.IO.Directory.CreateDirectory("Power");
                    System.IO.Directory.CreateDirectory("Speed");
                    System.IO.Directory.CreateDirectory("Technique");
                    Graphics g;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {


                            Bitmap power = drawPixelsSE(basepowers[j], i, 0), oPower = drawOutlineSE(basepowers[j], i, 0);
                            g = Graphics.FromImage(oPower);
                            g.DrawImage(power, 2, 2);
                            oPower.Save("Power/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, 0), oSpeed = drawOutlineSE(basespeeds[j], i, 0);
                            g = Graphics.FromImage(oSpeed);
                            g.DrawImage(speed, 2, 2);
                            oSpeed.Save("Speed/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, 0), oTechnique = drawOutlineSE(basetechniques[j], i, 0);
                            g = Graphics.FromImage(oTechnique);
                            g.DrawImage(technique, 2, 2);
                            oTechnique.Save("Technique/color" + i + "_frame_" + j + ".png", ImageFormat.Png);
                        }
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        powers[i].Close();
                        speeds[i].Close();
                        techniques[i].Close();
                    }
                }
                private static void processUnit(string u)
                {
                    BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
                    MagicaVoxelData[] parsed = FromMagica(bin);

                    BinaryReader[] bases = {
                                           new BinaryReader(File.Open("Base_Power.vox", FileMode.Open)),
                                           new BinaryReader(File.Open("Base_Speed.vox", FileMode.Open)),
                                           new BinaryReader(File.Open("Base_Technique.vox", FileMode.Open))
                    };
                    BinaryReader[] powers = new BinaryReader[8];
                    BinaryReader[] speeds = new BinaryReader[8];
                    BinaryReader[] techniques = new BinaryReader[8];


                    MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
                    MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

                    for (int i = 0; i < 8; i++)
                    {
                        powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                        basepowers[i] = FromMagica(powers[i]);
                        speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                        basespeeds[i] = FromMagica(speeds[i]);
                        techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                        basetechniques[i] = FromMagica(techniques[i]);

                    }
                    for (int i = 0; i < 8; i++)
                    {
                        System.IO.Directory.CreateDirectory(u);
                        System.IO.Directory.CreateDirectory(u + "color" + i);
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SE");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NW");
                        System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NE");
                        Bitmap bSE = drawPixelsSE(parsed, i, 0);
                        bSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);
                        Bitmap bSW = drawPixelsSW(parsed, i, 0);
                        bSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);
                        Bitmap bNW = drawPixelsNW(parsed, i, 0);
                        bNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
                        Bitmap bNE = drawPixelsNE(parsed, i, 0);
                        bNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bSE, 0, 0);
                            power.Save(u + "color" + i + "/power/SE/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bSE, 0, 0);
                            speed.Save(u + "color" + i + "/speed/SE/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bSE, 0, 0);
                            technique.Save(u + "color" + i + "/technique/SE/" + j + ".png", ImageFormat.Png);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bSW, 0, 0);
                            power.Save(u + "color" + i + "/power/SW/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bSW, 0, 0);
                            speed.Save(u + "color" + i + "/speed/SW/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bSW, 0, 0);
                            technique.Save(u + "color" + i + "/technique/SW/" + j + ".png", ImageFormat.Png);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bNE, 0, 0);
                            power.Save(u + "color" + i + "/power/NE/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bNE, 0, 0);
                            speed.Save(u + "color" + i + "/speed/NE/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bNE, 0, 0);
                            technique.Save(u + "color" + i + "/technique/NE/" + j + ".png", ImageFormat.Png);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Bitmap power = drawPixelsSE(basepowers[j], i, j % 2);
                            Graphics g = Graphics.FromImage(power);
                            g.DrawImage(bNW, 0, 0);
                            power.Save(u + "color" + i + "/power/NW/" + j + ".png", ImageFormat.Png);

                            Bitmap speed = drawPixelsSE(basespeeds[j], i, j % 2);
                            g = Graphics.FromImage(speed);
                            g.DrawImage(bNW, 0, 0);
                            speed.Save(u + "color" + i + "/speed/NW/" + j + ".png", ImageFormat.Png);

                            Bitmap technique = drawPixelsSE(basetechniques[j], i, j % 2);
                            g = Graphics.FromImage(technique);
                            g.DrawImage(bNW, 0, 0);
                            technique.Save(u + "color" + i + "/technique/NW/" + j + ".png", ImageFormat.Png);
                        }
                        ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
                        startInfo.UseShellExecute = false;
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SE/* " + u + "/color" + i + "_" + u + "_power_SE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SE/* " + u + "/color" + i + "_" + u + "_speed_SE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SE/* " + u + "/color" + i + "_" + u + "_technique_SE.gif";
                        Process.Start(startInfo).WaitForExit();

                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SW/* " + u + "/color" + i + "_" + u + "_power_SW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SW/* " + u + "/color" + i + "_" + u + "_speed_SW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SW/* " + u + "/color" + i + "_" + u + "_technique_SW.gif";
                        Process.Start(startInfo).WaitForExit();

                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NW/* " + u + "/color" + i + "_" + u + "_power_NW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NW/* " + u + "/color" + i + "_" + u + "_speed_NW.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NW/* " + u + "/color" + i + "_" + u + "_technique_NW.gif";
                        Process.Start(startInfo).WaitForExit();

                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NE/* " + u + "/color" + i + "_" + u + "_power_NE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NE/* " + u + "/color" + i + "_" + u + "_speed_NE.gif";
                        Process.Start(startInfo).WaitForExit();
                        startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NE/* " + u + "/color" + i + "_" + u + "_technique_NE.gif";
                        Process.Start(startInfo).WaitForExit();

                    }
                    bin.Close();
                    bases[0].Close();
                    bases[1].Close();
                    bases[2].Close();
                    for (int i = 0; i < 8; i++)
                    {
                        powers[i].Close();
                        speeds[i].Close();
                        techniques[i].Close();
                    }
                }
                */
        private static Bitmap[] processFloor(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_P.vox", FileMode.Open));
            PlusPaletteDraw.MagicaVoxelDataPaletted[] parsed = PlusPaletteDraw.FromMagica(bin);


            System.IO.Directory.CreateDirectory(u);
            Bitmap[] bits = new Bitmap[] {
                PlusPaletteDraw.drawPixelsSE(parsed),
                PlusPaletteDraw.drawPixelsSW(parsed),
                PlusPaletteDraw.drawPixelsNW(parsed),
                PlusPaletteDraw.drawPixelsNE(parsed)
            };
            /*Random r = new Random();
            Bitmap b = new Bitmap(80,40);
            Graphics tiling = Graphics.FromImage(b);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 0, 0);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, 20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, 20);*/
            bits[0].Save(u + "/" + u + "_default_SE" + ".png", ImageFormat.Png);
            bits[1].Save(u + "/" + u + "_default_SW" + ".png", ImageFormat.Png);
            bits[2].Save(u + "/" + u + "_default_NW" + ".png", ImageFormat.Png);
            bits[3].Save(u + "/" + u + "_default_NE" + ".png", ImageFormat.Png);
            //b.Save(u + "/tiled.png", ImageFormat.Png);

            bin.Close();
            return bits;
        }

        public static List<Tuple<int, int>> getSoftPath(int width, int height)
        {
            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            int x = r.Next(3, width - 1);
            int y;
            int midpoint = height / 2;
            for (y = 0; y <= midpoint; y++)
            {
                if (y % 2 == 1)
                {
                    x += r.Next(2);
                }
                else
                {
                    x -= r.Next(2);
                }

                if (x < 1)
                {
                    x++;
                }
                else if (x > width - 1)
                {
                    x--;
                }
                path.Add(new Tuple<int, int>(x, y));
            }
            List<Tuple<int, int>> path2 = path.ToList();
            path2.Reverse();
            path2.RemoveAt(0);
            int iter = 0;
            foreach (Tuple<int, int> t in path2)
            {
                iter++;
                path.Add(new Tuple<int, int>(t.Item1, midpoint + iter));
            }
            return path;
        }

        public static List<Tuple<int, int>> getHardPath(int width, int height)
        {
            int[,] grid = new int[width, height];

            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            int x;
            int initial_y = (r.Next(6, (height * 2) - 4) / 2) + 1;
            int y = initial_y;
            int midpoint = width / 2;
            int dir = (r.Next(2) == 0) ? -1 : 1;
            for (x = 0; x < width * 0.75; )
            {
                path.Add(new Tuple<int, int>(x, y));
                grid[x, y] = 10;
                if (y % 2 == 0)
                {
                    x++;
                }

                if (r.Next(5) == 0) dir *= -1;
                y += dir;

                if (y < 3)
                {
                    y += 2;
                    dir *= -1;
                }
                else if (y >= height - 2)
                {
                    y -= 2;
                    dir *= -1;
                }
            }

            y = initial_y;
            for (x = width - 1; x >= width * 0.25; )
            {
                path.Add(new Tuple<int, int>(x, y));

                if (r.Next(6) == 0) dir *= -1;
                y += dir;

                if (y % 2 == 0)
                {
                    x--;
                }

                if (y < 2)
                {
                    y += 2;
                    dir *= -1;
                }
                else if (y >= height - 1)
                {
                    y -= 2;
                    dir *= -1;
                }
                //if (x > 1 && x < width - 1 && y > 2 && y < height - 2)
                //{
                //    int[] adj = { grid[x, y + 2], grid[x + 1, y], grid[x, y - 2], grid[x - 1, y] };
                //    {
                //        if (adj.Count(i => i == 10) > 1)
                //        {
                //            if (y % 2 == 0)
                //            {
                //                x++;
                //            }
                //            dir *= -1;
                //        }
                //    }
                //}
            }

            return path;
        }
        static Bitmap makeFlatTiling()
        {
            Bitmap b = new Bitmap(128 * 16, 32 * 32);
            Graphics g = Graphics.FromImage(b);

            Bitmap[] tilings = new Bitmap[10];
            for (int i = 0; i < 10; i++)
            {
                tilings[i] = PlusPaletteDraw.drawPixelsFlat(i);
            }
            int[,] grid = new int[17, 33];
            Random r = new Random();

            //tilings[0].Save("flatgrass.png", ImageFormat.Png);
            /*
            for (int i = 0; i < 9; i++)
            {
                grid[i, 0] = 0;
                grid[i, 1] = 0;
                grid[i, 16] = 0;
            }
            for (int i = 1; i < 16; i++)
            {
                grid[0, i] = 0;
                grid[8, i] = 0;
            }*/

            int[,] takenLocations = new int[17, 33];
            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 33; j++)
                {
                    takenLocations[i, j] = 0;
                    grid[i, j] = 0;
                }
            }
            List<Tuple<int, int>> p = getSoftPath(10, 33);
            foreach (Tuple<int, int> t in p)
            {
                grid[t.Item1 + 6, t.Item2] = 9;
                takenLocations[t.Item1 + 6, t.Item2] = 1;
            }
            int numMountains = r.Next(17, 30);
            int iter = 0;
            int rx = r.Next(15) + 1, ry = r.Next(30) + 2;
            do
            {
                if (takenLocations[rx, ry] < 1 && r.Next(6) > 0 && ((ry + 1) / 2 != ry))
                {
                    takenLocations[rx, ry] = 2;
                    grid[rx, ry] = r.Next(4, 6);
                    int ydir = ((ry + 1) / 2 > ry) ? 1 : -1;
                    int xdir = (ry % 2 == 0) ? rx + r.Next(2) : rx - r.Next(2);
                    if (xdir <= 1) xdir++;
                    if (xdir >= 15) xdir--;
                    rx = xdir;
                    ry = ry + ydir;

                }
                else
                {
                    rx = r.Next(15) + 1;
                    ry = r.Next(30) + 2;
                }
                iter++;
            } while (iter < numMountains);

            List<Tuple<int, int>> h = getHardPath(17, 13);
            foreach (Tuple<int, int> t in h)
            {
                grid[t.Item1, t.Item2 + 6] = 8;
                takenLocations[t.Item1, t.Item2 + 6] = 4;
            }

            int extreme = 0;
            switch (r.Next(5))
            {
                case 0: extreme = 7;
                    break;
                case 1: extreme = 2;
                    break;
                case 2: extreme = 2;
                    break;
                case 3: extreme = 1;
                    break;
                case 4: extreme = 1;
                    break;
            }
            for (int i = 1; i < 16; i++)
            {
                for (int j = 2; j < 31; j++)
                {
                    for (int v = 0; v < 3; v++)
                    {

                        int[] adj = { 0, 0, 0, 0,
                                        0,0,0,0,
                                    0, 0, 0, 0, };
                        adj[0] = grid[i, j + 1];
                        adj[1] = grid[i, j - 1];
                        if (j % 2 == 0)
                        {
                            adj[2] = grid[i + 1, j + 1];
                            adj[3] = grid[i + 1, j - 1];
                        }
                        else
                        {
                            adj[2] = grid[i - 1, j + 1];
                            adj[3] = grid[i - 1, j - 1];
                        }
                        adj[4] = grid[i, j + 2];
                        adj[5] = grid[i, j - 2];
                        adj[6] = grid[i + 1, j];
                        adj[7] = grid[i - 1, j];
                        int likeliest = 0;
                        if (!adj.Contains(1) && extreme == 2 && r.Next(5) > 1)
                            likeliest = extreme;
                        if ((adj.Contains(2) && r.Next(4) == 0))
                            likeliest = extreme;
                        if (extreme == 7 && (r.Next(4) == 0) || (adj.Contains(7) && r.Next(3) > 0))
                            likeliest = extreme;
                        if ((adj.Contains(1) && r.Next(5) > 1) || r.Next(4) == 0)
                            likeliest = r.Next(2) * 2 + 1;
                        if (adj.Contains(5) && r.Next(3) == 0)
                            likeliest = r.Next(4, 6);
                        if (r.Next(45) == 0)
                            likeliest = 6;
                        if (takenLocations[i, j] == 0)
                        {
                            grid[i, j] = likeliest;
                        }
                    }
                }
            }


            for (int j = 0; j < 33; j++)
            {
                for (int i = 0; i < 17; i++)
                {
                    g.DrawImageUnscaled(tilings[grid[i, j]], (128 * i) - ((j % 2 == 0) ? 0 : 64), (32 * j) - 35 - 32);
                }
            }
            return b;
        }
        static Bitmap makeTiling()
        {

            Bitmap[] tilings = new Bitmap[20];
            processFloor("Grass").CopyTo(tilings, 0);
            processFloor("Grass").CopyTo(tilings, 4);
            processFloor("Grass").CopyTo(tilings, 8);
            processFloor("Jungle").CopyTo(tilings, 12);
            processFloor("Forest").CopyTo(tilings, 16);


            Random r = new Random();
            Bitmap[] lines = new Bitmap[20];
            int showRoadsOrRivers = r.Next(1);
            if (showRoadsOrRivers == 0)
            {
                processFloor("Road_Cross").CopyTo(lines, 0);
                processFloor("Road_Curve").CopyTo(lines, 4);
                processFloor("Road_End").CopyTo(lines, 8);
                processFloor("Road_Straight").CopyTo(lines, 12);
                processFloor("Road_Tee").CopyTo(lines, 16);
            }
            else
            {
                processFloor("River_Cross").CopyTo(lines, 0);
                processFloor("River_Curve").CopyTo(lines, 4);
                processFloor("River_End").CopyTo(lines, 8);
                processFloor("River_Straight").CopyTo(lines, 12);
                processFloor("River_Tee").CopyTo(lines, 16);
            }
            Bitmap b = new Bitmap(88 * 9, 44 * 18);
            Graphics tiling = Graphics.FromImage(b);

            Bitmap[,] grid = new Bitmap[10, 19];
            Bitmap[,] midgrid = new Bitmap[9, 18];
            for (int i = 0; i < 10; i++)
            {
                grid[i, 0] = tilings[r.Next(4)];
                grid[i, 18] = tilings[r.Next(4)];
            }
            for (int i = 0; i < 19; i++)
            {
                grid[0, i] = tilings[r.Next(4)];
                grid[9, i] = tilings[r.Next(4)];
            }
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    grid[i, j] = tilings[r.Next(20)];
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    midgrid[i, j] = tilings[r.Next(20)];
                }
            }
            int rl = r.Next(2, 4), rw = r.Next(2, 5);
            int randNorthX = r.Next(rw + 1, 8 - rl);
            int randNorthY = r.Next(3, 15 - rl - rw);
            int randWestX = randNorthX - rw, randWestY = randNorthY + rw;
            int randEastX = randNorthX + rl, randEastY = randNorthY + rl;
            int randSouthX = randNorthX + rl - rw, randSouthY = randNorthY + rl + rw;

            for (int w = 0; w < rw; w++)
            {
                grid[randWestX + w, randWestY - w] = null;
                midgrid[randWestX + w, randWestY - w - 1] = null;
                grid[randSouthX + w, randSouthY - w] = null;
                midgrid[randSouthX + w, randSouthY - w - 1] = null;
            }
            grid[randWestX + rw, randWestY - rw] = null;
            grid[randSouthX + rw, randSouthY - rw] = null;
            for (int l = 0; l < rl; l++)
            {
                grid[randWestX + l, randWestY + l] = null;
                midgrid[randWestX + l, randWestY + l] = null;
                grid[randNorthX + l, randNorthY + l] = null;
                midgrid[randNorthX + l, randNorthY + l] = null;
            }
            grid[randWestX + rl, randWestY + rl] = null;
            grid[randNorthX + rl, randNorthY + rl] = null;
            rl = r.Next(2, 5);
            rw = r.Next(2, 4);

            randNorthX = r.Next(rw + 1, 8 - rl);
            randNorthY = r.Next(3, 16 - rl - rw);
            randWestX = randNorthX - rw;
            randWestY = randNorthY + rw;
            randEastX = randNorthX + rl;
            randEastY = randNorthY + rl;
            randSouthX = randNorthX + rl - rw;
            randSouthY = randNorthY + rl + rw;
            for (int w = 0; w < rw; w++)
            {
                grid[randWestX + w, randWestY - w] = null;
                midgrid[randWestX + w, randWestY - w - 1] = null;
                grid[randSouthX + w, randSouthY - w] = null;
                midgrid[randSouthX + w, randSouthY - w - 1] = null;
            }
            grid[randWestX + rw, randWestY - rw] = null;
            grid[randSouthX + rw, randSouthY - rw] = null;
            for (int l = 0; l < rl; l++)
            {
                grid[randWestX + l, randWestY + l] = null;
                midgrid[randWestX + l, randWestY + l] = null;
                grid[randNorthX + l, randNorthY + l] = null;
                midgrid[randNorthX + l, randNorthY + l] = null;
            }
            grid[randWestX + rl, randWestY + rl] = null;
            grid[randNorthX + rl, randNorthY + rl] = null;
            int[,] adjGrid = new int[10, 19], adjMidGrid = new int[9, 18];
            adjGrid.Initialize();
            adjMidGrid.Initialize();

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    /*if (grid[i, j] == null)
                    {
                        adjGrid[i, j] |= 1;
                    }*/
                    if (midgrid[i, j] == null) // southeast
                    {
                        adjGrid[i, j] |= 2;
                    }
                    if (midgrid[i - 1, j] == null) // southwest
                    {
                        adjGrid[i, j] |= 4;
                    }
                    if (midgrid[i - 1, j - 1] == null) // northwest
                    {
                        adjGrid[i, j] |= 8;
                    }
                    if (midgrid[i, j - 1] == null) // northeast
                    {
                        adjGrid[i, j] |= 16;
                    }
                }
            }

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    /*if (midgrid[i, j] == null)
                    {
                        adjMidGrid[i, j] |= 1;
                    }*/
                    if (grid[i + 1, j + 1] == null) // southeast
                    {
                        adjMidGrid[i, j] |= 2;
                    }
                    if (grid[i, j + 1] == null) // southwest
                    {
                        adjMidGrid[i, j] |= 4;
                    }
                    if (grid[i, j] == null) // northwest
                    {
                        adjMidGrid[i, j] |= 8;
                    }
                    if (grid[i + 1, j] == null) // northeast
                    {
                        adjMidGrid[i, j] |= 16;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    if (grid[i, j] == null)
                    {
                        switch (adjGrid[i, j])
                        {
                            case 2: grid[i, j] = lines[8]; //se
                                break;
                            case 4: grid[i, j] = lines[9]; //sw
                                break;
                            case 8: grid[i, j] = lines[10]; //nw
                                break;
                            case 16: grid[i, j] = lines[11]; //ne
                                break;
                            case 6: grid[i, j] = lines[4]; //se sw
                                break;
                            case 10: grid[i, j] = lines[12]; //se nw
                                break;
                            case 12: grid[i, j] = lines[5]; //sw nw
                                break;
                            case 18: grid[i, j] = lines[7]; //ne se
                                break;
                            case 20: grid[i, j] = lines[13]; //sw ne
                                break;
                            case 24: grid[i, j] = lines[6]; //nw ne
                                break;
                            case 14: grid[i, j] = lines[17]; //se sw nw
                                break;
                            case 22: grid[i, j] = lines[16]; //ne se sw
                                break;
                            case 26: grid[i, j] = lines[19]; //se ne nw
                                break;
                            case 28: grid[i, j] = lines[18]; //sw nw ne
                                break;
                            case 30: grid[i, j] = lines[0]; //sw nw ne
                                break;
                            default: grid[i, j] = tilings[0];
                                break;
                        }
                    }
                }
            }

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    if (midgrid[i, j] == null)
                    {
                        switch (adjMidGrid[i, j])
                        {
                            case 2: midgrid[i, j] = lines[8]; //se
                                break;
                            case 4: midgrid[i, j] = lines[9]; //sw
                                break;
                            case 8: midgrid[i, j] = lines[10]; //nw
                                break;
                            case 16: midgrid[i, j] = lines[11]; //ne
                                break;
                            case 6: midgrid[i, j] = lines[4]; //se sw
                                break;
                            case 10: midgrid[i, j] = lines[12]; //se nw
                                break;
                            case 12: midgrid[i, j] = lines[5]; //sw nw
                                break;
                            case 18: midgrid[i, j] = lines[7]; //ne se
                                break;
                            case 20: midgrid[i, j] = lines[13]; //sw ne
                                break;
                            case 24: midgrid[i, j] = lines[6]; //nw ne
                                break;
                            case 14: midgrid[i, j] = lines[17]; //se sw nw
                                break;
                            case 22: midgrid[i, j] = lines[16]; //ne se sw
                                break;
                            case 26: midgrid[i, j] = lines[19]; //se ne nw
                                break;
                            case 28: midgrid[i, j] = lines[18]; //sw nw ne
                                break;
                            case 30: midgrid[i, j] = lines[0]; //sw nw ne
                                break;
                            default: midgrid[i, j] = tilings[0];
                                break;
                        }
                    }
                }
            }
            for (int j = 0; j < 18; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    tiling.DrawImageUnscaled(grid[i, j], (88 * i) - 44, (44 * j) - 22 - 7);
                }
                for (int i = 0; i < 9; i++)
                {
                    tiling.DrawImageUnscaled(midgrid[i, j], (88 * i), (44 * j) - 7);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                tiling.DrawImageUnscaled(grid[i, 18], (88 * i) - 44, (44 * 18) - 22 - 7);
            }
            return b;
        }

        static float[,] dijkstra(UnitInfo self, int[,] grid, UnitInfo[,] placing, int goalX, int goalY)
        {
            /*
            (defn find-cells [^doubles a cell-kind]
                (persistent! (areduce ^doubles a i ret (transient {})
                                      (if (= (hiphip/aget ^doubles a i) cell-kind) (assoc! ret i cell-kind) ret))))

            (defn find-goals [^doubles a]
              (find-cells a GOAL))

            (defn find-walls [^doubles a]
                (persistent! (areduce ^doubles a i ret (transient {})
                                      (if (>= (hiphip/aget ^doubles a i) (double wall)) (assoc! ret i wall) ret))))

            (defn find-floors [^doubles a]
              (find-cells a floor))

            (defn find-lowest [^doubles a]
              (let [low-val (hiphip/amin a)]
                (find-cells a low-val)))

            (defn find-monsters [m]
                (into {} (for [mp (map #(:pos @%) m)] [mp 1.0])))

            (defn dijkstra
              ([a]
                 (dijkstra a (find-walls a) (find-lowest a)))
              ([dun _]
                 (dijkstra (:dungeon dun) (merge (find-walls (:dungeon dun)) (find-monsters @(:monsters dun))) (find-lowest (:dungeon dun))))
              ([a closed open-cells]
                 (loop [open open-cells]
                   (when (seq open)
                     (recur (reduce (fn [newly-open [^long i ^double v]]
                                      (reduce (fn [acc dir]
                                                (if (or (closed dir) (open dir)
                                                        (>= (+ 1.0 v) (hiphip/aget ^doubles a dir)))
                                                  acc
                                                  (do (hiphip/aset ^doubles a dir (+ 1.0 v))
                                                      (assoc acc dir (+ 1.0 v)))))
                                              newly-open, [(- i wide2)
                                                           (+ i wide2)
                                                           (- i 2)
                                                           (+ i 2)]))
                                    {}, open))))
                 a))*/
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            float wall = 222;
            float unexplored = 111;
            float goal = 0;
            float[] d = new float[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                    d[i + width * j] = unexplored;
            }
            for (int i = 0; i < width; i++)
            {
                d[i] = wall;
                d[i + width * (height - 1)] = wall;
            }
            for (int j = 1; j < height - 1; j += 2)
            {
                d[j * width] = wall;
                d[(j + 1) * width + (width - 1)] = wall;
            }
            d[goalX + width * goalY] = goal;
            int[] ability =
            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, };
            //plains forest desert jungle hills mountains ruins tundra road river
            Dictionary<MovementType, bool> pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, true},
                {MovementType.Treads, true},
                {MovementType.Wheels, true},
                {MovementType.TreadsAmphi, true},
                {MovementType.WheelsTraverse, true},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
            switch (self.mobility)
            {
                case MovementType.Foot:
                    ability =
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, true},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, true},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.Treads:
                    ability =
            new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.Wheels:
                    ability =
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.TreadsAmphi:
                    ability =
            new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.WheelsTraverse:
                    ability =
            new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, false},
                {MovementType.Treads, false},
                {MovementType.Wheels, false},
                {MovementType.TreadsAmphi, false},
                {MovementType.WheelsTraverse, false},
                {MovementType.Flight, true},
                {MovementType.Immobile, false},
            };
                    break;
                case MovementType.Flight:
                    ability =
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, };
                    pass = new Dictionary<MovementType, bool>
            {
                {MovementType.Foot, true},
                {MovementType.Treads, true},
                {MovementType.Wheels, true},
                {MovementType.TreadsAmphi, true},
                {MovementType.WheelsTraverse, true},
                {MovementType.Flight, false},
                {MovementType.Immobile, false},
            };
                    break;
            }
            Dictionary<int, int> open = new Dictionary<int, int> { { (goalX + width * goalY), 0 } },
                fringe = new Dictionary<int, int>(),
                closed = new Dictionary<int, int>();
            int[] oddmoves = { -1 + width, width, -1 + -width, -width };
            int[] evenmoves = { 1 + width, width, 1 + -width, -width };
            while (open.Count > 0)
            {
                foreach (var idx_dijk in open)
                {
                    foreach (int mov in ((idx_dijk.Key / width) % 2 == 0) ? evenmoves : oddmoves)
                        if (open.ContainsKey(idx_dijk.Key + mov) ||
                            closed.ContainsKey(idx_dijk.Key + mov) ||
                            d[idx_dijk.Key + mov] == wall ||
                            d[idx_dijk.Key + mov] <= idx_dijk.Value + 1)
                        {

                        }
                        else if (
                            ability[grid[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width]] == 1 &&
                            (placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width] == null ||
                            pass[placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width].mobility] ||
                            (placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width].color == self.color &&
                            placing[(idx_dijk.Key + mov) % width, (idx_dijk.Key + mov) / width].mobility != MovementType.Immobile)
                            //self.x + self.y * width == idx_dijk.Key + mov
                            ))
                        {
                            fringe[idx_dijk.Key + mov] = (idx_dijk.Value + 1);
                            d[idx_dijk.Key + mov] = idx_dijk.Value + 1;
                        }
                }
                foreach (var kv in open)
                {
                    closed[kv.Key] = (kv.Value);
                }
                open.Clear();
                foreach (var kv in fringe)
                {
                    open[kv.Key] = (kv.Value);
                }
                fringe.Clear();

            }
            /*
([a closed open-cells]
    (loop [open open-cells]
        (when (seq open)
            (recur
               (reduce
                   (fn [newly-open [^long i ^double v]]
                       (reduce
                           (fn [acc dir]
                               (if (or (closed dir) (open dir) (>= (+ 1.0 v) (hiphip/aget ^doubles a dir)))
                                   acc
                                   (do (hiphip/aset ^doubles a dir (+ 1.0 v))
                                       (assoc acc dir (+ 1.0 v))
                                   )
                                )
                            )
                            newly-open
                            [(- i wide2)
                             (+ i wide2)
                             (- i 2)
                             (+ i 2)]
                        )
                    )
                    {}
                    open))))
     a)*/
            d[goalX + width * goalY] = wall;
            float[,] n = new float[width, height];

            for (int j = 0; j < height; j++)
            {
                if (j % 2 == 0)
                    Console.Write("  ");
                for (int i = 0; i < width; i++)
                {
                    n[i, j] = d[i + j * width];
                    Console.Write(string.Format("{0,4}", n[i, j]));
                }
                Console.WriteLine();
            }
            return n;
        }
        static List<Tuple<int, int, int>> getDijkstraPath(UnitInfo active, int[,] grid, UnitInfo[,] placing, int targetX, int targetY)
        {
            int width = grid.GetLength(0);
            float[,] d_inv = dijkstra(active, grid, placing, targetX, targetY);
            List<Tuple<int, int, int>> path = new List<Tuple<int, int, int>>();
            int currentX = active.x, currentY = active.y, currentFacing = active.facing;

            for (int f = 0; f < active.speed; f++)
            {
                Dictionary<int, float> near = new Dictionary<int, float>();
                near[currentX + width * (currentY)] = d_inv[currentX, currentY];
                near[currentX + width * (currentY + 1)] = d_inv[currentX, currentY + 1];
                near[currentX + width * (currentY - 1)] = d_inv[currentX, currentY - 1];
                if (currentY % 2 == 0)
                {
                    near[currentX + 1 + width * (currentY + 1)] = d_inv[currentX + 1, currentY + 1];
                    near[currentX + 1 + width * (currentY - 1)] = d_inv[currentX + 1, currentY - 1];
                }
                else
                {
                    near[currentX - 1 + width * (currentY + 1)] = d_inv[currentX - 1, currentY + 1];
                    near[currentX - 1 + width * (currentY - 1)] = d_inv[currentX - 1, currentY - 1];
                }
                var ordered = near.OrderBy(kv => kv.Value); //.First().Key;;
                int newpos = ordered.TakeWhile(kv => kv.Value == ordered.First().Value).RandomElement().Key;
                if (near.All(e => e.Value == near[newpos]))
                {
                    return new List<Tuple<int, int, int>>();
                }
                int newX = newpos % width, newY = newpos / width;
                if (!(newX == currentX && newY == currentY))
                {
                    if (newY % 2 == 0)
                    {
                        if (newY > currentY)
                        {
                            //south
                            if (newX < currentX)
                                currentFacing = 1;
                            else
                                currentFacing = 0;
                        }
                        else
                        {
                            //north
                            if (newX < currentX)
                                currentFacing = 2;
                            else
                                currentFacing = 3;
                        }
                    }
                    else
                        if (newY > currentY)
                        {
                            //south
                            if (newX == currentX)
                                currentFacing = 1;
                            else
                                currentFacing = 0;
                        }
                        else
                        {
                            //north
                            if (newX == currentX)
                                currentFacing = 2;
                            else
                                currentFacing = 3;
                        }

                    currentX = newX;
                    currentY = newY;
                    /*if (placing[newX, newY] != null && newX == targetX && newY == targetY)
                    {
                        break;
                    }
                    if (d_inv[newX, newY] <= 1.1F && placing[newX, newY] == null)
                        break;
                    */
                }
                path.Add(new Tuple<int, int, int>(currentX, currentY, currentFacing));
            }
            while (path.Count > 0 && placing[path.Last().Item1, path.Last().Item2] != null)
            {
                path.RemoveAt(path.Count - 1);
            }
            return path;
        }
        private static int failCount = 0;
        static void retryGamePreview(int MapWidth, int MapHeight)
        {
            failCount++;
            Console.WriteLine("\n\n!!!!! P A T H F I N D I N G   F A I L U R E   " + failCount + " !!!!!\n\n");
            if (failCount > 10)
            {
                Console.WriteLine("Too many dijkstra failures.");
                Console.In.ReadLine();
                return;
            }
            makeGamePreview(MapWidth, MapHeight);
        }
        static void makeGamePreview(int MapWidth, int MapHeight)
        {

            System.IO.Directory.CreateDirectory("animation");
            foreach (var fil in System.IO.Directory.EnumerateFiles("animation"))
            {
                System.IO.File.Delete(fil);
            }
            int width = ((MapWidth / 2) * 2) + 1;
            int height = ((MapHeight / 2) * 2) + 1;
            Bitmap[] tilings = new Bitmap[10];
            for (int i = 0; i < 10; i++)
            {
                tilings[i] = PlusPaletteDraw.drawPixelsFlat(i);
            }
            int[] heights = new int[]
            {
                2, 3, 1, 4, 6, 8, 4, 2, 2, 1, 7
            };

            int[,] grid = new int[width, height];
            UnitInfo[,] placing = new UnitInfo[width, height];
            //foot 0-0, treads 1-5, wheels 6-8, flight 9-10
            /*string[] unitnames = { "Infantry", //foot 0 0
                                   "Tank", "Artillery", "Artillery_P", "Artillery_S", "Supply_P", //treads 1 5
                                   "Artillery_T", "Supply", "Supply_T", //wheels 6 8
                                   "Helicopter", "Plane", //flight 9 10
                                   "City", "Factory", "Castle", "Capital" //facility
                                 };*/
            //, City, Castle, Factory, Capital
            BinaryReader bin;
            List<MagicaVoxelData[]> unitps = new List<MagicaVoxelData[]>(), facilityps = new List<MagicaVoxelData[]>();
            foreach (string u in CurrentUnits)
            {
                bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
                unitps.Add(FromMagica(bin));
                bin.Close();
            }

            //tilings[0].Save("flatgrass.png", ImageFormat.Png);
            /*
            for (int i = 0; i < 9; i++)
            {
                grid[i, 0] = 0;
                grid[i, 1] = 0;
                grid[i, 16] = 0;
            }
            for (int i = 1; i < 16; i++)
            {
                grid[0, i] = 0;
                grid[8, i] = 0;
            }*/

            int[,] takenLocations = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    takenLocations[i, j] = 0;
                    grid[i, j] = 0;
                }
            }
            List<Tuple<int, int>> p = getSoftPath(width / 2, height);
            /*foreach (Tuple<int, int> t in p)
            {
                grid[t.Item1 + 3, t.Item2] = 9;
                takenLocations[t.Item1 + 3, t.Item2] = 1;
            }*/
            int numMountains = r.Next((int)(width * 0.25), (int)(width * 0.75));
            int iter = 0;
            int rx = r.Next(width - 2) + 1, ry = r.Next(height - 3) + 2;
            do
            {
                if (takenLocations[rx, ry] < 1 && r.Next(6) > 0 && ((ry + 1) / 2 != ry))
                {
                    takenLocations[rx, ry] = 2;
                    grid[rx, ry] = r.Next(4, 6);
                    int ydir = ((ry + 1) / 2 > ry) ? 1 : -1;
                    int xdir = (ry % 2 == 0) ? rx + r.Next(2) : rx - r.Next(2);
                    if (xdir <= 1) xdir++;
                    if (xdir >= width - 1) xdir--;
                    rx = xdir;
                    ry = ry + ydir;

                }
                else
                {
                    rx = r.Next(width - 2) + 1;
                    ry = r.Next(height - 3) + 2;
                }
                iter++;
            } while (iter < numMountains);

            List<Tuple<int, int>> h = getHardPath(width, height / 3);
            foreach (Tuple<int, int> t in h)
            {
                grid[t.Item1, t.Item2 + 2] = 8;
                takenLocations[t.Item1, t.Item2 + 2] = 4;
            }
            h = getHardPath(width, height / 3);
            foreach (Tuple<int, int> t in h)
            {
                grid[t.Item1, t.Item2 + 2 * ((height / 3 + 3) / 2)] = 8;
                takenLocations[t.Item1, t.Item2 + 2 * ((height / 3 + 3) / 2)] = 4;
            }
            h = getHardPath(width, height / 3);
            foreach (Tuple<int, int> t in h)
            {
                grid[t.Item1, t.Item2 + 2 * ((height / 6 + 6) / 2)] = 8;
                takenLocations[t.Item1, t.Item2 + 3 * ((height / 3 + 1) / 2)] = 4;
            }

            int extreme = 0;
            switch (r.Next(5))
            {
                case 0: extreme = 7;
                    break;
                case 1: extreme = 2;
                    break;
                case 2: extreme = 2;
                    break;
                case 3: extreme = 1;
                    break;
                case 4: extreme = 1;
                    break;
            }
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 2; j < height - 2; j++)
                {
                    for (int v = 0; v < 3; v++)
                    {

                        int[] adj = { 0, 0, 0, 0,
                                        0,0,0,0,
                                    0, 0, 0, 0, };
                        adj[0] = grid[i, j + 1];
                        adj[1] = grid[i, j - 1];
                        if (j % 2 == 0)
                        {
                            adj[2] = grid[i + 1, j + 1];
                            adj[3] = grid[i + 1, j - 1];
                        }
                        else
                        {
                            adj[2] = grid[i - 1, j + 1];
                            adj[3] = grid[i - 1, j - 1];
                        }
                        adj[4] = grid[i, j + 2];
                        adj[5] = grid[i, j - 2];
                        adj[6] = grid[i + 1, j];
                        adj[7] = grid[i - 1, j];
                        int likeliest = 0;
                        if (!adj.Contains(1) && extreme == 2 && r.Next(5) > 1)
                            likeliest = extreme;
                        if ((adj.Contains(2) && r.Next(4) == 0))
                            likeliest = extreme;
                        if (extreme == 7 && (r.Next(4) == 0) || (adj.Contains(7) && r.Next(3) > 0))
                            likeliest = extreme;
                        if ((adj.Contains(1) && r.Next(5) > 2) || r.Next(7) == 0)
                            likeliest = r.Next(2) * 2 + 1;
                        if (adj.Contains(5) && r.Next(3) == 0)
                            likeliest = r.Next(4, 6);
                        if (r.Next(45) == 0)
                            likeliest = 6;
                        if (takenLocations[i, j] == 0)
                        {
                            grid[i, j] = likeliest;
                        }
                    }
                }
            }
            int[] allcolors = { 1, 2, 3, 4, 5, 6, 7 }, colors = new int[4];
            bool[] taken = { false, false, false, false, false, false, false };
            for (int i = 1; i < 4; i++)
            {
                int col = r.Next(7);
                while (taken[col])
                    col = r.Next(7);
                colors[i] = allcolors[col];
                taken[col] = true;
            }
            colors[0] = 0;

            int[] targetX = { width / 4, width / 2, width / 4, width / 2, },
                  targetY = { height / 2, height / 4, height / 2, height / 4 };
            for (int section = 0; section < 2; section++)
            {
                rx = (width / 4) + (width / 2) * (section % 2);
                ry = 3 + (height / 6);
                //processSingleOutlined(facilityps[(colors[section] == 0) ? 3 : 2], colors[section], dirs[r.Next(4)])
                if (colors[section] == 0)
                {
                    placing[rx, ry] = new UnitInfo("Estate", colors[section], r.Next(4), rx, ry);
                    targetX[1] = rx;
                    targetY[1] = ry;
                    targetX[2] = rx;
                    targetY[2] = ry;
                    targetX[3] = rx;
                    targetY[3] = ry;
                }
                else
                {
                    placing[rx, ry] = new UnitInfo("Castle", colors[section], r.Next(4), rx, ry);
                    targetX[0] = rx;
                    targetY[0] = ry;
                }
                grid[rx, ry] = 0;
                for (int i = rx - (width / 8); i < rx + (width / 8); i++)
                {
                    for (int j = ry - (height / 8); j < ry + (height / 8); j++)
                    {
                        if (placing[i, j] != null)
                            continue;
                        if (r.Next(9) <= 1 && (grid[i, j] == 0 || grid[i, j] == 1 || grid[i, j] == 2 || grid[i, j] == 4 || grid[i, j] == 8))
                        {
                            //
                            placing[i, j] = new UnitInfo(r.Next(24, 28), colors[section], r.Next(4), i, j);
                            //processSingleOutlined(facilityps[r.Next(3) % 2], colors[section], dirs[r.Next(4)]);
                        }

                    }
                }
            }
            for (int section = 2; section < 4; section++)
            {
                rx = (width / 4) + (width / 2) * (section % 2);
                ry = height - 3 - (height / 6);
                placing[rx, ry] = new UnitInfo(((colors[section] == 0) ? UnitLookup["Estate"] : UnitLookup["Castle"]), colors[section], r.Next(4), rx, ry);
                grid[rx, ry] = 0;
                for (int i = rx - (width / 8); i < rx + (width / 8); i++)
                {
                    for (int j = ry - (height / 8); j < ry + (height / 8); j++)
                    {
                        if (placing[i, j] != null)
                            continue;
                        if (r.Next(9) <= 1 && (grid[i, j] == 0 || grid[i, j] == 1 || grid[i, j] == 2 || grid[i, j] == 4 || grid[i, j] == 8))
                        {
                            placing[i, j] = new UnitInfo(r.Next(24, 28), colors[section], r.Next(4), i, j);
                        }

                    }
                }
            }
            List<Tuple<int, int>> guarantee = new List<Tuple<int, int>>();
            for (int section = 0; section < 4; section++) // section < 4
            {
                for (int i = 2 + (width / 2) * (section % 2); i < (width / 2) - 2 + (width / 2) * (section % 2); i++)
                {
                    for (int j = (section / 2 == 0) ? 3 : height / 2 + 2; j < ((section / 2 == 0) ? height / 2 - 2 : height - 3); j++)
                    {
                        if (placing[i, j] != null)
                            continue;
                        int currentUnit = TerrainToUnits[grid[i, j]].RandomElement();
                        //foot 0-0, treads 1-5, wheels 6-8, flight 9-10
                        if (r.Next(16) <= 3)
                        {
                            placing[i, j] = new UnitInfo(currentUnit, colors[section], section, i, j);
                            if (guarantee.Count == section)
                                guarantee.Add(new Tuple<int, int>(i, j));
                        }

                    }
                }
                if (guarantee.Count == section)
                {
                    int rgx = r.Next(2 + (width / 2) * (section % 2), (width / 2) - 2 + (width / 2) * (section % 2));
                    int rgy = r.Next((section / 2 == 0) ? 3 : height / 2 + 2, ((section / 2 == 0) ? height / 2 - 2 : height - 3));
                    int problems = 0;
                    while (placing[rgx, rgy] != null)
                    {
                        rgx = r.Next(2 + (width / 2) * (section % 2), (width / 2) - 2 + (width / 2) * (section % 2));
                        rgy = r.Next((section / 2 == 0) ? 3 : height / 2 + 2, ((section / 2 == 0) ? height / 2 - 2 : height - 3));
                        if (placing[rgx, rgy] != null)
                            problems++;
                        if (problems > 10)
                        {
                            retryGamePreview(MapWidth, MapHeight);
                            return;
                        }
                    }
                    placing[rgx, rgy] = new UnitInfo(TerrainToUnits[grid[rgx, rgy]].RandomElement(), colors[section], section, rgx, rgy);
                    guarantee.Add(new Tuple<int, int>(rgx, rgy));
                }

            }
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 3; j < height - 3; j++)
                {
                    if (r.Next(18) <= 1 && placing[i, j] == null)
                    {
                        int rs = r.Next(2);
                        int currentUnit = TerrainToUnits[grid[i, j]].RandomElement();
                        placing[i, j] = new UnitInfo(currentUnit, colors[rs], rs, i, j);

                    }
                }
            }

            ColorMatrix normal = new ColorMatrix(new float[][]{ 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

            ColorMatrix[] swirl = {
                         new ColorMatrix(new float[][]{ 
   new float[] {1.4F,  0,  0,  0, 0},
   new float[] {0,  0.8F,  0,  0, 0},
   new float[] {0,  0,  0.8F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}}),
                         new ColorMatrix(new float[][]{ 
   new float[] {1.25F,  0,  0,  0, 0},
   new float[] {0,  1.25F,  0,  0, 0},
   new float[] {0,  0,  0.8F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}}),
                         new ColorMatrix(new float[][]{ 
   new float[] {0.8F,  0,  0,  0, 0},
   new float[] {0,  1.4F,  0,  0, 0},
   new float[] {0,  0,  0.8F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}}),
                         new ColorMatrix(new float[][]{ 
   new float[] {0.85F,  0,  0,  0, 0},
   new float[] {0,  0.85F,  0,  0, 0},
   new float[] {0,  0,  1.4F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}})},

   shine = {
               new ColorMatrix(new float[][]{ 
   new float[] {1.3F,  0,  0,  0, 0},
   new float[] {0,  1.3F,  0,  0, 0},
   new float[] {0,  0,  1.3F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}}),
               new ColorMatrix(new float[][]{ 
   new float[] {1.45F,  0,  0,  0, 0},
   new float[] {0,  1.45F,  0,  0, 0},
   new float[] {0,  0,  1.45F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}}),
               new ColorMatrix(new float[][]{ 
   new float[] {1.3F,  0,  0,  0, 0},
   new float[] {0,  1.3F,  0,  0, 0},
   new float[] {0,  0,  1.3F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}}),
               new ColorMatrix(new float[][]{ 
   new float[] {1.15F,  0,  0,  0, 0},
   new float[] {0,  1.15F,  0,  0, 0},
   new float[] {0,  0,  1.15F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}})};
            Bitmap b;
            Graphics g;
            int latest = 0;

            for (int n = 0; n < 2; n++)
            {
                for (int u = 0; u < 4; u++)
                {
                    UnitInfo active = new UnitInfo(
                        placing[guarantee[u].Item1, guarantee[u].Item2].unit,
                        placing[guarantee[u].Item1, guarantee[u].Item2].color,
                        placing[guarantee[u].Item1, guarantee[u].Item2].facing,
                        guarantee[u].Item1,
                        guarantee[u].Item2);
                    placing[active.x, active.y] = null;
                    float[,] d = dijkstra(active, grid, placing, active.x, active.y);
                    bool[,] highlight = new bool[width, height];
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            highlight[i, j] = d[i, j] > 0 && d[i, j] <= active.speed;
                        }
                    }

                    List<Tuple<int, int, int>> path = getDijkstraPath(active, grid, placing, targetX[u], targetY[u]);
                    int losses = 0;
                    while (path.Count == 0)
                    {
                        losses++;
                        if (losses > 5)
                        {
                            retryGamePreview(MapWidth, MapHeight);
                            return;
                        }
                        placing[active.x, active.y] = new UnitInfo(
                            active.unit,
                            active.color,
                            active.facing,
                            active.x, active.y);
                        UnitInfo temp = placing.RandomFactionUnit(active.color);
                        active = new UnitInfo(
                        temp.unit,
                        temp.color,
                        temp.facing,
                        temp.x, temp.y);
                        placing[active.x, active.y] = null;
                        path = getDijkstraPath(active, grid, placing, targetX[u], targetY[u]);
                    }
                    for (int f = -4; f < path.Count * 4; f++)
                    {
                        b = new Bitmap(128 * (width - 1), 32 * ((height / 2) * 2));
                        g = Graphics.FromImage(b);
                        if (f >= 0)
                        {
                            Tuple<int, int, int> node = path[f / 4];
                            active.x = node.Item1;
                            active.y = node.Item2;
                            active.facing = node.Item3;
                        }
                        for (int j = 0; j < height; j++)
                        {
                            for (int i = 0; i < width; i++)
                            {

                                ColorMatrix colorMatrix = normal;
                                if (f < 0)
                                {
                                    if (highlight[i, j])
                                    {
                                        colorMatrix = shine[(f * -1) - 1];
                                    }
                                    else if (active.x == i && active.y == j)
                                    {
                                        colorMatrix = swirl[(f * -1) - 1];
                                    }
                                }
                                else
                                {
                                    highlight[i, j] = false;
                                    colorMatrix = normal;
                                }
                                ImageAttributes imageAttributes = new ImageAttributes();
                                imageAttributes.SetColorMatrix(
                                   colorMatrix,
                                   ColorMatrixFlag.Default,
                                   ColorAdjustType.Bitmap);
                                g.DrawImage(
                                   tilings[grid[i, j]],
                                   new Rectangle((128 * i) - ((j % 2 == 0) ? 0 : 64), (32 * j) - 37 - 32, 128, 100),  // destination rectangle 
                                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                                   0, 0,        // upper-left corner of source rectangle 
                                   128,       // width of source rectangle
                                   100,      // height of source rectangle
                                   GraphicsUnit.Pixel,
                                   imageAttributes);
                                //g.DrawImage(tilings[grid[i, j]], (128 * i) - ((j % 2 == 0) ? 0 : 64), (32 * j) - 35 - 32);
                                if (placing[i, j] != null)
                                {
                                    g.DrawImageUnscaled(new Bitmap("color" + placing[i, j].color + "/" + placing[i, j].name +
                                        "_face" + placing[i, j].facing +
                                        "_" + ((placing[i, j].speed > 0) ? (f + 100) % 4 : (f + 100) % 2) + ".png"),
                                        (128 * i) - ((j % 2 == 0) ? 0 : 64) + 20,
                                        (32 * j) - 64 - 16 - heights[grid[i, j]] * 3);
                                }
                                /*g.DrawImageUnscaled(
                                    processSingleOutlined(
                                      unitps[placing[i, j].unit],
                                      placing[i, j].color,
                                      Directions[placing[i, j].facing],
                                    ((placing[i, j].speed > 0) ? (f + 100) % 2 : 0))
                                    , (128 * i) - ((j % 2 == 0) ? 0 : 64) + 20,
                                    (32 * j) - 64 - 16 - heights[grid[i, j]] * 3);*/
                                if (i == active.x && j == active.y)
                                    g.DrawImageUnscaled(new Bitmap("color" + active.color + "/" + active.name +
                                        "_face" + active.facing +
                                        "_" + ((active.speed > 0) ? (f + 100) % 4 : (f + 100) % 2) + ".png"),
                                        (128 * i) - ((j % 2 == 0) ? 0 : 64) + 20,
                                        (32 * j) - 64 - 16 - heights[grid[i, j]] * 3);
                                /*                                g.DrawImageUnscaled(
                                                                    processSingleOutlined(
                                                                      unitps[active.unit],
                                                                      active.color,
                                                                      Directions[active.facing],
                                                                    ((active.speed > 0) ? (f + 100) % 2 : 0))
                                                                    , (128 * i) - ((j % 2 == 0) ? 0 : 64) + 20,
                                                                    (32 * j) - 64 - 16 - heights[grid[i, j]] * 3);*/
                            }
                        }

                        if (f >= -4 && f <= -2)
                        {
                            b.Save("animation/" + string.Format("{0:000}", latest) + ".png", ImageFormat.Png);
                            b.Save("animation/" + string.Format("{0:000}", latest + 4) + ".png", ImageFormat.Png);
                            b.Save("animation/" + string.Format("{0:000}", latest + 8) + ".png", ImageFormat.Png);
                        }
                        else if (f == -1)
                        {

                            b.Save("animation/" + string.Format("{0:000}", latest) + ".png", ImageFormat.Png);
                            b.Save("animation/" + string.Format("{0:000}", latest + 4) + ".png", ImageFormat.Png);
                            b.Save("animation/" + string.Format("{0:000}", latest + 8) + ".png", ImageFormat.Png);
                            latest += 8;
                        }
                        else
                        {
                            b.Save("animation/" + string.Format("{0:000}", latest) + ".png", ImageFormat.Png);
                        }
                        latest++;
                        b.Dispose();
                    }
                    placing[active.x, active.y] = new UnitInfo(
                        active.unit,
                        active.color,
                        active.facing,
                        active.x, active.y);
                }
                for (int i = 0; i < 4; i++)
                {
                    UnitInfo uni = placing.RandomFactionUnit(colors[i]);
                    guarantee[i] = new Tuple<int, int>(uni.x, uni.y);
                }
            }
            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo(@"ffmpeg.exe");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.Arguments = " -r 8 -i animation\\%03d.png -vcodec libvpx -pass 1 -an -f rawvideo -r 6 -y NUL";
            // @" -i animation\%03d.png -vcodec rawvideo -pix_fmt yuv420p animation\preview.y4m";
            proc.Start();
            proc.WaitForExit();
            /* -i 1080/sintel_trailer_2k_%04d.png -vf crop=1920:816:0:132 -vcodec rawvideo -r 24 -pix_fmt yuv444p sintel_trailer.y4m
            proc.StartInfo = new ProcessStartInfo(@"yuvit.exe");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.Arguments = "-a -m 0:" + (3 - 1) + @" animation\###.png animation\preview.yuv";
           ffmpeg -i INPUTFILE -b 1500k -vcodec libx264 -vpre slow -vpre baseline -g 30 "OUTPUTFILE.mp4"
             * */

            Process p2 = new Process();
            p2.StartInfo = new ProcessStartInfo(@"ffmpeg.exe");//vpxenc.exe
            p2.StartInfo.UseShellExecute = false;
            p2.StartInfo.Arguments = " -r 8 -i animation\\%03d.png -b:v 2500K -vcodec libvpx -pass 2 -acodec libvorbis -crf 4 -f webm -g 30 -r 6 animation\\preview.webm";
            /*            @" --best --cpu-used=1 --auto-alt-ref=1 --lag-in-frames=16 --fps=8000/1001" +
            @" --end-usage=vbr --undershoot-pct=95 --buf-sz=6000 --buf-initial-sz=4000 --buf-optimal-sz=5000" +
            @" --passes=2 --threads=2 --target-bitrate=1000 -o animation\preview.webm animation\preview.y4m";*/
            p2.Start();
            p2.WaitForExit();

            proc = new Process();
            proc.StartInfo = new ProcessStartInfo(@"ffmpeg.exe");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.Arguments = "-r 8 -i animation\\%03d.png -b:v 2000k -vcodec libx264 -pix_fmt yuv420p -vprofile high -g 30 -r 6 animation\\preview.mp4";
            proc.Start();
            proc.WaitForExit();

            proc = new Process();
            proc.StartInfo = new ProcessStartInfo(@"ffmpeg.exe");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.Arguments = "-r 8 -i animation\\%03d.png -b:v 2000k -vcodec libtheora -acodec libvorbis -ab 160000 -g 30 -r 6 animation\\preview.ogv";
            proc.Start();
            proc.WaitForExit();

            //            Console.In.ReadLine();
            //@" -i animation\%03d.png -vcodec rawvideo -pix_fmt yuv420p animation\preview.y4m";

            /* ffmpeg -r 1 -pattern_type glob -i '*.jpg'
            ProcessStartInfo startInfo = new ProcessStartInfo(@"convert.exe");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "-dispose background -delay 22 -loop 0 animation/* animation/preview.gif";
            Process.Start(startInfo).WaitForExit();*/
        }

        /// <summary>
        /// This will take a long time to run.  It should produce a ton of assets and an animated gif preview.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Initialize();

            System.IO.Directory.CreateDirectory("Palettes");
            /*for (int c = 0; c < 8; c++)
            {
                List<string> ls = new List<string>(17);
                for (int i = 0; i < 17; i++)
                {
                    ls.Add(colors[i * 8 + c][0] + ", " + colors[i * 8 + c][1] + ", " + colors[i * 8 + c][2] + "\n");
                }
                File.WriteAllLines("Palettes/color" + c + ".txt", ls);
            }*/


            //InitializePalettes();

            for (int c = 0; c < 8; c++)
            {
                renderOnlyColors(c).Save("PaletteColor" + c + ".png", ImageFormat.Png);
            }
            renderOnlyColors().Save("PaletteTerrain" + ".png", ImageFormat.Png);
            Madden();
            renderOnlyColors(7).Save("PaletteCrazy.png", ImageFormat.Png);

           // CreateIndexedBitmap(new Bitmap("crazy/Infantry_face0_0.png"), "redefined_Infantry_color1.png", 1);


            /*for (int i = 0; i < 11; i++)
            {
                PlusPaletteDraw.drawPixelsFlat(i);
            }*/

            //processUnitOutlined("Block");
/*
            Madden();

            processUnitChannel("Infantry");
            processUnitChannel("Infantry_P");
            processUnitChannel("Infantry_S");
            processUnitChannel("Infantry_T");

            processUnitChannel("Artillery");
            processUnitChannel("Artillery_P");
            processUnitChannel("Artillery_S");
            processUnitChannel("Artillery_T");

            processUnitChannel("Tank");
            processUnitChannel("Tank_P");
            processUnitChannel("Tank_S");
            processUnitChannel("Tank_T");

            processUnitChannel("Supply");
            processUnitChannel("Supply_P");
            processUnitChannel("Supply_S");
            processUnitChannel("Supply_T");

            processUnitChannel("Plane");
            processUnitChannel("Plane_P");
            processUnitChannel("Plane_S");
            processUnitChannel("Plane_T");

            processUnitChannel("Copter");
            processUnitChannel("Copter_P");
            processUnitChannel("Copter_S");
            processUnitChannel("Copter_T");

            processUnitChannel("City");
            processUnitChannel("Factory");
            processUnitChannel("Airport");
            processUnitChannel("Laboratory");
            processUnitChannel("Castle");
            processUnitChannel("Estate");
            */


            /*
             
            processUnitOutlined("Infantry");
            processUnitOutlined("Infantry_P");
            processUnitOutlined("Infantry_S");
            processUnitOutlined("Infantry_T");

            processUnitOutlined("Artillery");
            processUnitOutlined("Artillery_P");
            processUnitOutlined("Artillery_S");
            processUnitOutlined("Artillery_T");

            processUnitOutlined("Tank");
            processUnitOutlined("Tank_P");
            processUnitOutlined("Tank_S");
            processUnitOutlined("Tank_T");
            
            processUnitOutlined("Supply");
            processUnitOutlined("Supply_P");
            processUnitOutlined("Supply_S");
            processUnitOutlined("Supply_T");
            
            processUnitOutlined("Plane");
            processUnitOutlined("Plane_P");
            processUnitOutlined("Plane_S");
            processUnitOutlined("Plane_T");
            
            processUnitOutlined("Copter");
            processUnitOutlined("Copter_P");
            processUnitOutlined("Copter_S");
            processUnitOutlined("Copter_T");
            
            processUnitOutlined("City");
            processUnitOutlined("Factory");
            processUnitOutlined("Airport");
            processUnitOutlined("Laboratory");
            processUnitOutlined("Castle");
            processUnitOutlined("Estate");
            
             */

            //       makeGamePreview(9, 18);


            /*
            processFloor("Grass");
            processFloor("Forest");
            processFloor("Jungle");
            
            processFloor("Road_Straight");
            processFloor("Road_Curve");
            processFloor("Road_Tee");
            processFloor("Road_End");
            processFloor("Road_Cross");

            processFloor("River_Straight");
            processFloor("River_Curve");
            processFloor("River_Tee");
            processFloor("River_End");
            processFloor("River_Cross");
            */
            //processBases();

            //            makeFlatTiling().Save("tiling_flat.png", ImageFormat.Png);
            /*for (int i = 0; i < 40; i++)
            {
                makeFlatTiling().Save("bg" + i + ".png", ImageFormat.Png);
            }*/
            /*
            Bitmap[] randomTilings = new Bitmap[18];
            for (int i = 0; i < 18; i++)
            {
                randomTilings[i] = makeTiling();
            }
            Bitmap b = new Bitmap(720 + 72, 720 + 72);
            Graphics g = Graphics.FromImage(b);
            for (int i = 0; i < 18; i++)
            {
                g.DrawImageUnscaled(randomTilings[i], 264 * (i % 3), 132 * (i / 3));
            }
            b.Save("tiling_large.png", ImageFormat.Png);*/
            /*
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\ImageMagick-6.8.9-Q16\convert.EXE");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "tiling_large.png -modulate 110,45 tiling_grayed.png";
            Process.Start(startInfo).WaitForExit();
            */

        }
    }
}
