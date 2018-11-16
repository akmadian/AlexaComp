using System;
using System.Threading;

using RGB.NET.Core;
using RGB.NET.Devices;
using RGB.NET.Groups;
using RGB.NET.Brushes;
using RGB.NET.Brushes.Gradients;
using RGB.NET.Devices.Asus;
using RGB.NET.Devices.CoolerMaster;
using RGB.NET.Devices.Corsair;
using RGB.NET.Devices.DMX;
using RGB.NET.Devices.Logitech;
using RGB.NET.Devices.Novation;
using RGB.NET.Devices.Razer;

namespace AlexaComp.Controllers {

    // TODO : Method of assigning generic part names like "mouse" or "keyboard" to RGBDevice objects. 
    class LightingController : AlexaCompCore {

        public static RGBSurface surface = RGBSurface.Instance;

        /// <summary>
        /// Loads and logs all devices.
        /// </summary>
        public static void startLightingThread() {
            clog("RGB Devices Loaded, Printing...");
            loadDevices();
            foreach (var device in surface.Devices) {
                clog(String.Format("RGB Device Found - {0} {1} - Type: {2}",
                        device.DeviceInfo.Manufacturer,
                        device.DeviceInfo.Model,
                        device.DeviceInfo.DeviceType));
            }
            surface.UpdateMode = UpdateMode.Continuous;
            // You can run lighting effects here
        }

        /// <summary>
        /// Loads all devices into the surface object.
        /// </summary>
        public static void loadDevices() {
            surface.Exception += args => AlexaComp._log.Info(args.Exception.Message);
            clog("Loading RGB Devices");

            //surface.LoadDevices(AsusDeviceProvider.Instance);
            surface.LoadDevices(LogitechDeviceProvider.Instance);
            surface.LoadDevices(CorsairDeviceProvider.Instance);
            surface.LoadDevices(CoolerMasterDeviceProvider.Instance);
            surface.LoadDevices(DMXDeviceProvider.Instance);
            surface.LoadDevices(NovationDeviceProvider.Instance);
            surface.LoadDevices(RazerDeviceProvider.Instance);
        }

        /// <summary>
        /// Contains methods for lighting effects
        /// All speed parameters are an integer number of milliseconds to wait before each lighting tick.
        ///     The lower the value, the faster the effect goes.
        /// </summary>
        // TODO : Make param quidelines for all methods.
        // TODO : Implement more methods.
        public class LightingEffects : LightingController {

            /// <summary>
            /// Creates a static colored breathing effect.
            /// Increasing the speed param makes the effect go slower. At high values, the
            ///     lighting updates can look choppy. This choppiness can be offset by 
            ///     reducing the brightnessStep param.
            ///     
            /// Speed Values:
            ///     Fasted Reccomended  - Speed: 10 at brightnessStep: 0.05
            ///     Good Middle Speed   - Speed: 20 at brightnessStep: 0.01
            ///     Slowest Reccomended - Speed: 45 at brightnessStep: 0.01
            /// </summary>
            /// <param name="speed">A delay (in milliseconds) - How long to wait before each brightness step up or down</param>
            /// <param name="brightnessStep"></param>
            // TODO : Implement color cieling. Only default color values work.
            // If values are 255, 255, 255, they are added to and subtracted from an equal ammount.
            public static void breathingAnimation(int R = 1, int G = 1, int B = 1, int speed = 20, double brightnessStep = 0.01) {
                clog(String.Format("RGB -- Breathing Animation Set -- Color: {0}, {1}, {2} - Speed: {3} - Brightness Step: {4}",
                    R, G, B, speed, brightnessStep));
                while (true) {
                    ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                    for (double i = 0.01; i <= 1; i += brightnessStep) {
                        ledGroup.Brush = new SolidColorBrush(new Color(R * i, G * i, B * i));
                        Thread.Sleep(speed);
                    }
                    for (double i = 1.01; i >= 0; i -= brightnessStep) {
                        ledGroup.Brush = new SolidColorBrush(new Color(R * i, G * i, B * i));
                        Thread.Sleep(speed);
                    }
                }
            }

            /// <summary>
            /// Pulses a color at full brightness, then fades to darkness.
            /// </summary>
            /// <param name="effectSpeed">The delay (in milliseconds) between pulses</param>
            /// <param name="fadeSpeed">How quickly the color fades after the pulse</param>
            /// <param name="brightnessStep">The granularity of the brightness adjustments</param>
            // TODO : Fix. Has same color issue as breathingAnimation.
            public static void pulseEffect(int R, int G, int B, int effectSpeed = 100, int fadeSpeed = 10, double brightnessStep = 0.01) {
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    ledGroup.Brush = new SolidColorBrush(new Color(255, 255, 255));
                    for (double i = 1.0; i > 0; i -= brightnessStep) {
                        ledGroup.Brush = new SolidColorBrush(new Color(R * i, G * i, B * i));
                        Thread.Sleep(fadeSpeed);
                    }
                    Thread.Sleep(effectSpeed);
                }
            }

            /// <summary>
            /// Alternates between two provided colors.
            /// </summary>
            /// <param name="color1">The first color</param>
            /// <param name="color2">The second color</param>
            /// <param name="speed"></param>
            public static void alternatingEffect(RGBColor color1, RGBColor color2, int speed = 400) {
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    ledGroup.Brush = new SolidColorBrush(color1.toRGBNETColor());
                    Thread.Sleep(speed);
                    ledGroup.Brush = new SolidColorBrush(color2.toRGBNETColor());
                    Thread.Sleep(speed);
                }
            }

            /// <summary>
            /// Blinks a color on and off.
            /// </summary>
            /// <param name="color"></param>
            /// <param name="speed"></param>
            public static void blinkingEffect(RGBColor color, int speed = 400) {
                alternatingEffect(color, new RGBColor(0, 0, 0), speed);
            }

            /// <summary>
            /// Sets all leds to a static color
            /// </summary>
            public static void staticColor(int R, int G, int B) {
                clog(String.Format("RGB -- Setting Static Color -- Color: {0}, {1}, {2}", R, G, B));
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                ledGroup.Brush = new LinearGradientBrush(new RainbowGradient());
            }

            /// <summary>
            /// Creates a rainbow effect by iterating through hue values.
            /// How fast the effect goes depends on the speed param, and the hueStep param.
            ///     The higher the speed value, the lower the hueStep should be.
            ///     At high delay times, lighting updates can be seen. This can be offset 
            ///         by reducing how much the hue value is incremented each loop.
            /// 
            /// Speed Values:
            ///     Fastest Reccomended - Speed: 5 at hueStep: 0.01
            ///     Good Middle Speed   - Speed: 50 at hueStep: 0.01
            ///     Slowest Reccomended - Speed: 100 at hueStep: 0.001
            /// </summary>
            /// <param name="speed"></param>
            public static void fadingEffect(int speed = 50, double hueStep = 0.01) {
                clog(String.Format("Setting Rainbow Fade Effect -- Speed: {0} - Hue Step: {1}", speed, hueStep));
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    for (double j = 0; j < 1; j += hueStep) {
                        RGBColor c = colorMethods.HSLToRGB(j, 1.0, 0.5);
                        ledGroup.Brush = new SolidColorBrush(new Color(c.R, c.G, c.B));
                        Thread.Sleep(speed);
                    }
                }
            }

            /// <summary>
            /// Turns all leds off
            /// </summary>
            public static void allLedOff() {
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                ledGroup.Brush = new SolidColorBrush(new Color(0, 0, 0));
            }

            public static void errorEffect(int speed = 1) {
                clog("RGB Error Effect Started.");
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                for (int j = 0; j <= 2; j++) {
                    for (int i = 0; i <= 255; i++) {
                        ledGroup.Brush = new SolidColorBrush(new Color(i, 0, 0));
                        Thread.Sleep(speed);
                    }
                    for (int i = 255; i >= 0; i--) {
                        ledGroup.Brush = new SolidColorBrush(new Color(i, 0, 0));
                        Thread.Sleep(speed);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Methods that convert color values.
    /// </summary>
    class colorMethods : AlexaCompCore {

        /// <summary>
        /// From: https://geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        /// This function converts HSL to RGB, I don't know anything else about it.
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="sl">Saturation</param>
        /// <param name="l">Luminance</param>
        /// <returns>An RGBColor object</returns>
        public static RGBColor HSLToRGB(double h, double sl, double l) {
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
                return new RGBColor(Convert.ToByte(r * 255), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f));
            }
            catch (OverflowException) {
                clog("Overflow Exception caught during attempt to return RGB object from HSLToRGB.");
                return null;
            }
        }

        /// <summary>
        /// Converts a hex value to an RGBColor object
        /// Example hex string: FF00FF OR #FF00FF
        /// </summary>
        /// <param name="hex">A hex string</param>
        /// <returns>An RGBColor object</returns>
        public static RGBColor HexToRGB(string hex) {
            string hexString = hex;
            if (hexString.StartsWith("#")) { // Strip leading # if it exists
                hexString = hexString.Substring(1);
            }
            string[] splitHex = splitStringEveryN(hex, 2);
            return new RGBColor(Convert.ToInt32(splitHex[0], 16), Convert.ToInt32(splitHex[1], 16), Convert.ToInt32(splitHex[2], 16));
        }

        /// <summary>
        /// Converts an integer array with RGB values in it to an RGBColor object.
        /// Example array: [123, 255, 65] 
        /// </summary>
        /// <param name="array">The integer array to convert</param>
        /// <returns>An RGBColor object</returns>
        public static RGBColor ArrToRGB(int[] array) => new RGBColor(array[0], array[1], array[2]);

        /// <summary>
        /// Converts an RGBColor object to a hex string
        /// </summary>
        /// <param name="color">The RGB object to convert</param>
        /// <returns>A hex string</returns>
        public static string RGBToHex(RGBColor color) => color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

        public static int[] RGBToArr(RGBColor color) => new int[] { color.R, color.G, color.B };
    }

    public static class Extensions {
        public static int[] RGBAsIntArray(this Color thisColor, Color colorObj) => new int[3] {
            Convert.ToInt32(colorObj.R),
            Convert.ToInt32(colorObj.G),
            Convert.ToInt32(colorObj.B)
        };

        public static void printObj(this Color thisColor) {
            Console.WriteLine(String.Format("RGBColor Object -- R: {0}, G: {1}, B: {2}", thisColor.R, thisColor.G, thisColor.B));
        }
    }

    class RGBColor {

        public int R;
        public int G;
        public int B;

        public int A { get; }

        public RGBColor(int r = 0, int g = 0, int b = 0) {
            R = r;
            G = g;
            B = b;
        }

        public RGBColor RGBColorFromSystemDrawingColor(System.Drawing.Color color) {
            return new RGBColor(color.R, color.B, color.G);
        }

        public void setColorByIndex(int index, int value) {
            switch (index) {
                case 0: R = value; break;
                case 1: G = value; break;
                case 2: B = value; break;
            }
        }

        public Color toRGBNETColor() => new Color(R, G, B);

        public void printObj() {
            Console.WriteLine(String.Format("RGBColor Object -- R: {0}, G: {1}, B: {2}", R, G, B));
        }

        public int[] sliceColor() => new int[3] { R, G, B };
    }
}