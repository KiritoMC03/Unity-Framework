using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameKit.General.Extensions
{
    public static class IntExtensions
    {
        #region Fields

        private static readonly StringBuilder StringBuilder = new StringBuilder(capacity: 20);

        private static readonly Dictionary<int, string> ShortMoneySuffixes = new Dictionary<int, string>()
        {
            { 3, "k" },
            { 6, "m" },
            { 9, "b" },
            { 12, "s" },
        };

        #endregion
        
        #region Methods

        public static string FormatAsDefaultShortMoney(this int number, string prefix = default, Dictionary<int, string> shortMoneySuffixes = default)
        {
            if (number < 1000) return $"{prefix}{number}";
            StringBuilder.Clear();
            int digitsNumber = Mathf.CeilToInt(Mathf.Log10(number));
            digitsNumber = Mathf.FloorToInt((float) digitsNumber / 3) * 3;

            shortMoneySuffixes ??= ShortMoneySuffixes;
            float divider = Mathf.Pow(10, digitsNumber);

            int intPart = Mathf.FloorToInt(number / divider);
            StringBuilder.Append(prefix).Append(intPart).Append('.');
            int floatPart = Mathf.RoundToInt(number % divider / divider * 100);
            if (floatPart < 10) StringBuilder.Append('0');
            StringBuilder.Append(floatPart);
            if (shortMoneySuffixes.ContainsKey(digitsNumber)) 
                return StringBuilder.Append(shortMoneySuffixes[digitsNumber]).ToString();
            return number.ToString();
        }

        #endregion
    }
}