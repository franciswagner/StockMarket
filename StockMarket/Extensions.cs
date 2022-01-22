using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceMonitor
{
    public static class Extensions
    {
        public static bool IsAnyOfThese(this int value, params int[] nunmbers)
        {
            return nunmbers.Any(text => value == text);
        }

        public static string RemoveLineBreaks(this string value)
        {
            value = value.Replace('\n', ' ').Replace('\r', ' ');

            while (value.Contains("  "))
                value = value.Replace("  ", " ");

            return value.Trim();
        }

        public static IEnumerable<T> TakeLastObject<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }
    }
}
