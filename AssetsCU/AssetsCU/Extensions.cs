using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetsCU;
namespace AssetsCU
{
    public static class Extensions
    {
        private static Random r = new Random();
        public static T RandomElement<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            return list[r.Next(list.Count)];
        }
        public static T RandomElement<T>(this T[,] mat)
        {
            if (mat.Length == 0)
                return default(T);

            return mat[r.Next(mat.GetLength(0)), r.Next(mat.GetLength(1))];
        }
        public static PlusVoxels.UnitInfo RandomFactionUnit(this PlusVoxels.UnitInfo[,] mat, int color)
        {
            if (mat.Length == 0)
                return new PlusVoxels.UnitInfo();
            PlusVoxels.UnitInfo u = new PlusVoxels.UnitInfo();
            List<PlusVoxels.UnitInfo> units = new List<PlusVoxels.UnitInfo>();
            for (int i = 0; i < mat.GetLength(0); i++ )
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (mat[i, j] != null && mat[i, j].color == color)
                    {
                        units.Add(mat[i, j]);
                    }
                }
            }
            return units.RandomElement();
        }
    }   
}
