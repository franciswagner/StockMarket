using StockMarket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PriceMonitor
{
    public static class Extensions
    {
        public static bool IsAnyOfThese(this int value, params int[] nunmbers)
        {
            return nunmbers.Any(text => value == text);
        }

        public static IEnumerable<T> TakeLastObject<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }

        public static decimal ToDecimal(this string value)
        {
            value = string.IsNullOrEmpty(value) ? "0" : value;
            return Convert.ToDecimal(value, new CultureInfo("pt-BR", false));
        }

        public static Acao ReturnAcaoWithOldestDate(this IEnumerable<Acao> list, DateTime minDate, DateTime maxDate)
        {
            return list.FirstOrDefault(x => x.RequestedDate <= maxDate && x.RequestedDate > minDate);
        }
    }
}
