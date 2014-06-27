using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }   
}
