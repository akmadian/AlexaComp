using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RGB.NET.Core;

namespace AlexaComp.Core.Helpers {

    class ColorConversions : AlexaCompCore{

        /// <summary>
        /// From: https://geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        /// This function converts HSL to RGB, I don't know anything else about it.
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="sl">Saturation</param>
        /// <param name="l">Luminance</param>
        /// <returns>An RGBColor object</returns>
        public static Color? HSLToRGB(double h, double sl, double l) {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0) {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant) {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            try {
                return new Color(Convert.ToByte(r * 255), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f));
            }
            catch (OverflowException) {
                Clog("Overflow Exception caught during attempt to return RGB object from HSLToRGB.");
                return null;
            }
        }

        /// <summary>
        /// Converts a hex value to an RGBColor object
        /// Example hex string: FF00FF OR #FF00FF
        /// </summary>
        /// <param name="hex">A hex string</param>
        /// <returns>An RGBColor object</returns>
        public static Color HexToRGB(string hex) {
            string hexString = hex;
            if (hexString.StartsWith("#")) { // Strip leading # if it exists
                hexString = hexString.Substring(1);
            }
            string[] splitHex = hex.SplitEveryN(2);
            return new Color(Convert.ToInt32(splitHex[0], 16), Convert.ToInt32(splitHex[1], 16), Convert.ToInt32(splitHex[2], 16));
        }

        /// <summary>
        /// Converts an integer array with RGB values in it to an RGBColor object.
        /// Example array: [123, 255, 65] 
        /// </summary>
        /// <param name="array">The integer array to convert</param>
        /// <returns>An RGBColor object</returns>
        public static Color ArrToRGB(int[] array) => new Color(array[0], array[1], array[2]);

        /// <summary>
        /// Converts an RGBColor object to a hex string
        /// </summary>
        /// <param name="color">The RGB object to convert</param>
        /// <returns>A hex string</returns>
        public static string RGBToHex(Color color) => color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

        public static int[] RGBToArr(Color color) => new int[] { color.R, color.G, color.B };
    }
}
