using System;
using System.Collections.Generic;
using System.Text;

using RGB.NET.Core;

namespace AlexaComp.Core {

    public static class StringExtensions {

        #region Methods
        public static string MultString(this string str, int n) {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= n; i++) { sb.Append(str); }
            return sb.ToString();
        }

        public static string StripSpaces(this string str) => str.Replace(" ", "");

        public static string[] SplitEveryN(this string toSplit, int n) {
            List<string> returnArr = new List<string>();
            for (int i = 0; i < toSplit.Length; i += n) {
                returnArr.Add(toSplit.Substring(i, Math.Min(n, toSplit.Length - i)));
            }
            return returnArr.ToArray();
        }
        #endregion
    }

    public static class ColorExtensions {

        public static int[] SliceColor(this Color color) => new int[3] { color.R, color.G, color.B };
        
    }

    public static class RGBSurfaceExtensions {

        public static void AddIRGBDevice(this RGBSurface surface, IRGBDevice device) {
        }

        public static void RemoveIRGBDevice(this RGBSurface surface, IRGBDevice device) {
        }


    }
}