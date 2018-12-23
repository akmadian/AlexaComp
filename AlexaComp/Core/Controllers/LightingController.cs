using System;
using System.Collections.Generic;
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

using AlexaComp.Core;
using AlexaComp.Core.Lighting;
using AlexaComp.Core.Helpers;

namespace AlexaComp.Controllers {

    // TODO : Method of assigning generic part names like "mouse" or "keyboard" to RGBDevice objects. 
    class LightingController : AlexaCompCore {

        public static RGBSurface surface = RGBSurface.Instance;
        public static Thread lightingThread;

        /// <summary>
        /// Loads and logs all devices.
        /// </summary>
        public static void StartLightingThread() {
            LoadDevices();
            Clog("RGB Devices Loaded, Printing...");
            foreach (var device in surface.Devices) {
                Clog(String.Format("RGB Device Found - {0} {1} - Type: {2}",
                        device.DeviceInfo.Manufacturer,
                        device.DeviceInfo.Model,
                        device.DeviceInfo.DeviceType));
            }
            surface.UpdateMode = UpdateMode.Continuous;
        }

        // Applies rainbow effect to all devices with an ledgroup
        public static void ApplyEffect() {
            int speed = 50;
            double hueStep = 0.01;
            while (true) {
                foreach (KeyValuePair<string, Hardware> pair in Devices) {
                    Hardware device = pair.Value;
                    if (device.HasLEDGroup()) {
                        for (double j = 0; j < 1; j += hueStep) {
                            Color c = (Color)ColorConversions.HSLToRGB(j, 1.0, 0.5);
                            device.ledGroup.Brush = new SolidColorBrush(c);
                            Thread.Sleep(speed);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads all devices into the surface object.
        /// </summary>
        public static void LoadDevices() {
            surface.Exception += args => AlexaComp._log.Info(args.Exception.Message);
            Clog("Loading RGB Devices");

            //surface.LoadDevices(AsusDeviceProvider.Instance);
            surface.LoadDevices(LogitechDeviceProvider.Instance);
            surface.LoadDevices(CorsairDeviceProvider.Instance);
            surface.LoadDevices(CoolerMasterDeviceProvider.Instance);
            surface.LoadDevices(DMXDeviceProvider.Instance);
            surface.LoadDevices(NovationDeviceProvider.Instance);
            surface.LoadDevices(RazerDeviceProvider.Instance);
        }

        public static void LightingProcess(HexColor color1 = null, HexColor color2 = null, string effect = "", int speed_ = -20, double granularity_ = -0.1, int speed2 = -1) {
            color1 = color1 ?? new HexColor("ffffff");
            color2 = color2 ?? new HexColor("ffffff");

            Clog(String.Format("Setting {0} effect - Color1: {1}, Color2: {2}, Speed: {3}, Speed2: {4}, Granularity: {5}", 
                effect, color1.ToString(), color2.ToString(), speed_, speed2, granularity_));

            switch (effect) {
                case "": break;
                /* TO TEST */case "STATICCOLOR":       lightingThread = new Thread(unused => LightingEffects.StaticColor(color1)); break;
                /* TO TEST */case "ERROREFFECT":       lightingThread = new Thread(unused => LightingEffects.ErrorEffect(speed: speed_)); break;
                /* TO TEST */case "BREATHINGEFFECT":   lightingThread = new Thread(unused => LightingEffects.BreathingEffect(color1, speed: speed_, brightnessStep: granularity_)); break;
                /* WORKING */case "RAINBOWFADEEFFECT": lightingThread = new Thread(unused => LightingEffects.FadingEffect()); break;
                /* TO TEST */case "PULSEEFFECT":       lightingThread = new Thread(unused => LightingEffects.PulseEffect(color1, effectSpeed: speed_, fadeSpeed: speed2, brightnessStep: granularity_)); break;
                // case "ALTERNATINGEFFECT": lightingThread = new Thread(unused => LightingEffects.alternatingEffect()); break;
                // case "BLINKINGEFFECT": lightingThread = new Thread(unused => LightingEffects.blinkingEffect(color: color1, speed: speed_)); break;
                /* WORKING */case "ALLLEDOFF":         lightingThread = new Thread(unused => LightingEffects.AllLedOff()); break;
            }
            lightingThread.Name = "LightingEffectThread";
            lightingThread.Start();
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
            public static void BreathingEffect(HexColor color, int speed = 20, double brightnessStep = 0.01) {
                HexColor col_ = new HexColor("#000000");
                Clog(col_.ToString());
                Color col = col_.ToRGB();
                if (speed < 0) { speed = 20; }
                if (brightnessStep < 0) { brightnessStep = 0.01; }

                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    // Increase Brightness
                    for (double i = 0.01; i <= 1; i += brightnessStep) {
                        ledGroup.Brush = new SolidColorBrush(new Color(col.R * i, col.G * i, col.B * i));
                        Thread.Sleep(speed);
                    }

                    // Decrease Brightness
                    for (double i = 1.01; i >= 0; i -= brightnessStep) {
                        ledGroup.Brush = new SolidColorBrush(new Color(col.R * i, col.G * i, col.B * i));
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
            public static void PulseEffect(HexColor color, int effectSpeed = 100, int fadeSpeed = 10, double brightnessStep = 0.01) {
                Color col = color.ToRGB();
                if (effectSpeed < 0) { effectSpeed = 100; }
                if (fadeSpeed < 0) { fadeSpeed = 10; }
                if (brightnessStep < 0) { brightnessStep = 0.01; }
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    ledGroup.Brush = new SolidColorBrush(new Color(255, 255, 255));
                    for (double i = 1.0; i > 0; i -= brightnessStep) {
                        ledGroup.Brush = new SolidColorBrush(new Color(col.R * i, col.G * i, col.B * i));
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
            /// Broken at the moment...
            /*
            public static void alternatingEffect(R1 = 255, RGBColor color2 , int speed = 400) {
                if (color1 == null) { color1 = new RGBColor(255, 0, 0); }
                if (color2 == null) { color2 = new RGBColor(0, 0, 255); }
                if (speed < 0) { speed = 400; }
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    ledGroup.Brush = new SolidColorBrush(color1.toRGBNETColor());
                    Thread.Sleep(speed);
                    ledGroup.Brush = new SolidColorBrush(color2.toRGBNETColor());
                    Thread.Sleep(speed);
                }
            }*/

            /// <summary>
            /// Blinks a color on and off.
            /// </summary>
            /// <param name="color"></param>
            /// <param name="speed"></param>
            public static void BlinkingEffect(HexColor color, int speed = 400) {
                Color col = color.ToRGB();
                if (speed < 0) { speed = 400; }
                // alternatingEffect(color, new RGBColor(0, 0, 0), speed);
            }

            /// <summary>
            /// Sets all leds to a static color
            /// </summary>
            public static void StaticColor(HexColor color) {
                Color col = color.ToRGB();
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    ledGroup.Brush = new  SolidColorBrush(new Color(col.R, col.G, col.B));
                }
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
            public static void FadingEffect(int speed = 50, double hueStep = 0.01) {
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                while (true) {
                    for (double j = 0; j < 1; j += hueStep) {
                        Color c = (Color)ColorConversions.HSLToRGB(j, 1.0, 0.5);
                        ledGroup.Brush = new SolidColorBrush(c);
                        Thread.Sleep(speed);
                    }
                }
            }

            public static void ErrorEffect(int speed = 1) {
                if (speed < 0) { speed = 1; }
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

            /// <summary>
            /// Turns all leds off
            /// </summary>
            public static void AllLedOff() {
                ILedGroup ledGroup = new ListLedGroup(surface.Leds);
                ledGroup.Brush = new SolidColorBrush(new Color(0, 0, 0));
            }
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

        public RGBColor (System.Drawing.Color color) => new RGBColor(color.R, color.B, color.G);
        public RGBColor (string[] color) => new RGBColor(Convert.ToInt32(color[0]), Convert.ToInt32(color[1]), Convert.ToInt32(color[2]));

        public void SetColorByIndex(int index, int value) {
            switch (index) {
                case 0: R = value; break;
                case 1: G = value; break;
                case 2: B = value; break;
            }
        }

        public Color ToRGBNETColor() => new Color(R, G, B);

        public void PrintObj() {
            Console.WriteLine(String.Format("RGBColor Object -- R: {0}, G: {1}, B: {2}", R, G, B));
        }

        public int[] SliceColor() => new int[3] { R, G, B };
    }
}