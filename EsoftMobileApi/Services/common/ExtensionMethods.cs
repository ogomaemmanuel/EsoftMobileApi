using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ESoft.Web.Services.Common
{
    public static class ExtensionMethods
    {
        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }
    }

    public static class IntegerExtensions
    {
        public static int ConvertNullToInteger(this int? amount)
        {
            return (int)(amount == null ? 0 : amount);
        }
    }
    public static class DoubleExtensions
    {
        public static double ConvertDecimaltoDouble(this decimal p)
        {           
            return Convert.ToDouble(p);
        }
        public static double ConvertDecimaltoDouble(this decimal? p)
        {
            p = p.ConvertNullToDecimal();
            return Convert.ToDouble(p);
        }
        public static double ConvertNullToDouble(this double? amount)
        {
            return (double)(amount == null ? 0 : amount);
        }
        public static string AmountToWords(this double number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + AmountToWords(Math.Abs(number));

            string words = "";
            int wholeNumber = (int)Math.Floor(number);

            if ((wholeNumber / 1000000) > 0)
            {
                words += AmountToWords(wholeNumber / 1000000) + " Million ";
                wholeNumber %= 1000000;
            }

            if ((wholeNumber / 1000) > 0)
            {
                words += AmountToWords(wholeNumber / 1000) + " Thousand ";
                wholeNumber %= 1000;
            }

            if ((wholeNumber / 100) > 0)
            {
                words += AmountToWords(wholeNumber / 100) + " Hundred ";
                wholeNumber %= 100;
            }

            if (wholeNumber > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (wholeNumber < 20)
                    words += unitsMap[wholeNumber];
                else
                {
                    words += tensMap[wholeNumber / 10];
                    if ((wholeNumber % 10) > 0)
                        words += " " + unitsMap[wholeNumber % 10];
                }
            }
            return words;
        }

        public static string Amount2Words(this double amount)
        {
            string words = AmountToWords(amount);
            double cents = amount - Math.Floor(amount); ;
            if (cents > 0.0)
            {
                words = words + " and " + Math.Round(cents, 2).ToString().PadLeft(4,'0').Substring(2, 2).Replace('.',' ') + " Cents ";
            }
            return words;
        }
    }

    public static class DecimalExtensions
    {
        public static decimal ConvertNullToDecimal(this decimal? amount)
        {
            return (decimal)(amount == null ? 0 : amount);
        }
        public static decimal ConvertDoubletoDecimal(this double? p)
        {
            p = p.ConvertNullToDouble();
            return Convert.ToDecimal(p);
        }
        public static decimal ConvertDoubletoDecimal(this double p)
        {            
            return Convert.ToDecimal(p);
        }
    }
    public static class DateTimeExtensions
    {
        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }

        public static DateTime ConvertNullToDatetime(this DateTime? date)
        {
            return (DateTime)((date == null || (date == DateTime.MinValue)) ? new DateTime(1900, 01, 01) : date);
        }
    }

    public static class BooleanExtensions
    {
        public static bool ConvertNullToBool(this bool? boolItem)
        {
            return (bool)(boolItem == null ? false : boolItem);
        }
        public static string ConvertBoolToSqlString(this bool? boolItem)
        {
            boolItem = boolItem == null ? false : boolItem;
            return boolItem == true ? "1" : "0";
        }
        public static string ConvertBoolToSqlString(this bool boolItem)
        {
            boolItem = boolItem == null ? false : boolItem;
            return boolItem == true ? "1" : "0";
        }
    }

    public static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            System.Globalization.TextInfo myTI = new System.Globalization.CultureInfo("en-US", false).TextInfo;

            return myTI.ToTitleCase(value);
        }
        public static string Format_Sql_String(this string _mstring)
        {
            _mstring = string.IsNullOrWhiteSpace(_mstring) ? string.Empty : _mstring;
            _mstring = _mstring.Replace("'", "");
            _mstring = _mstring.Replace("[", "");
            _mstring = _mstring.Replace("]", "");
            _mstring = _mstring.Replace(";", "");
            _mstring = _mstring.Replace("--", "");
            _mstring = _mstring.Replace("/*", "");
            _mstring = _mstring.Replace("*/", "");
            _mstring = _mstring.Replace("xp_", "");
            return _mstring.Trim();
        }
        public static String ConvertNullToEmptyString(this string str)
        {
            return (string.IsNullOrWhiteSpace(str) ? string.Empty : str).Trim();
        }

    }

    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid value)
        {
            return value == null || value == Guid.Empty;
        }
        public static bool IsNullOrEmpty(this Guid? value)
        {
            return value == null || value == Guid.Empty;
        }
    }

    #region License and Terms
    // MoreLINQ - Extensions to LINQ to Objects
    // Copyright (c) 2008 Jonathan Skeet. All rights reserved.
    // 
    // Licensed under the Apache License, Version 2.0 (the "License");
    // you may not use this file except in compliance with the License.
    // You may obtain a copy of the License at
    // 
    //     http://www.apache.org/licenses/LICENSE-2.0
    // 
    // Unless required by applicable law or agreed to in writing, software
    // distributed under the License is distributed on an "AS IS" BASIS,
    // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    // See the License for the specific language governing permissions and
    // limitations under the License.
    #endregion
    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the maximal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        /// <summary>
        /// Returns the maximal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values. 
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first
        /// one encountered will be returned. This operator uses immediate execution, but
        /// only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }
    }
}
