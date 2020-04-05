using System;
using System.Collections.Generic;

namespace MahjongBuddy.Application.Extensions
{
    public static class ShuffleExtensions
    {
        static readonly Random Random = new Random();
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
