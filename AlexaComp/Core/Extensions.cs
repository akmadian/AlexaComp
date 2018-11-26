using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AlexaComp.Core {

    public static class StringExtensions {

        #region Methods
        public static string multString(this string str, int n) {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= n; i++) { sb.Append(str); }
            return sb.ToString();
        }

        public static string stripSpaces(this string str) {
            return str.Replace(" ", "");
        }

        public static string[] splitEveryN(this string toSplit, int n) {
            List<string> returnArr = new List<string>();
            for (int i = 0; i < toSplit.Length; i += n) {
                returnArr.Add(toSplit.Substring(i, Math.Min(n, toSplit.Length - i)));
            }
            return returnArr.ToArray();
        }
        #endregion
    }
}