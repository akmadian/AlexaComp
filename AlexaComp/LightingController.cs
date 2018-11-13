using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using RGB.NET.Core;
using RGB.NET.Devices.Logitech;
using RGB.NET.Devices.Asus;
using RGB.NET.Devices.CoolerMaster;
using RGB.NET.Devices.Corsair;
using RGB.NET.Devices.Razer;
using RGB.NET.Groups;
using RGB.NET.Brushes;

namespace AlexaComp {
    class LightingController {
        /// Method Ideas
        // Set all to color
        // Set effect
        // Set profiles
        // Set speed
        // Set effect direction

        // Inventory devices, find and map brand and device -> Ojbect

        public static RGBSurface surface = RGBSurface.Instance;
        public static Dictionary<string, string> deviceMap = new Dictionary<string, string>() {
            {"Mouse", "" }, {"Keyboard", ""}, {"CaseStrips", ""}, {"CaseFans", ""}, {"RAM", ""}
        };
        
        
        public LightingController() {
            surface.Exception += args => Console.WriteLine(args.Exception.Message);
            surface.UpdateMode = UpdateMode.Continuous;

            loadAllDevices();


            foreach (var device in surface.Devices) {
                AlexaComp._log.Info(String.Format("RGB Device Found - {0} {1} - Type: {2}", 
                    device.DeviceInfo.Manufacturer, 
                    device.DeviceInfo.Model, 
                    device.DeviceInfo.DeviceType));
            }

            for (int i = 0; i <= 255; i++) {
                setAllToColor(0, i / 2, i);
                Thread.Sleep(10);
            }
            for (int i = 255; i >= 0; i--) {
                setAllToColor(0, i / 2, i);
                Thread.Sleep(10);
            }
        }

        public void loadAllDevices() {
            surface.LoadDevices(CorsairDeviceProvider.Instance);
            surface.LoadDevices(LogitechDeviceProvider.Instance);
            surface.LoadDevices(CoolerMasterDeviceProvider.Instance);
        }

        public void setAllToColor(int r, int g, int b) {
            ILedGroup ledGroup = new ListLedGroup(surface.Leds);
            ledGroup.Brush = new SolidColorBrush(new Color(r, g, b));
        }

        public void logDevices() {
            foreach (var thing in surface.Devices) {
                Console.WriteLine(thing);
                Console.WriteLine(thing.DeviceInfo);
                Console.WriteLine("");
                Console.WriteLine("    DeviceType - " + thing.DeviceInfo.DeviceType);
                Console.WriteLine("    Manufacturer - " + thing.DeviceInfo.Manufacturer);
                Console.WriteLine("    Model - " + thing.DeviceInfo.Model);
                Console.WriteLine("    Lighting - " + thing.DeviceInfo.Lighting);
                Console.WriteLine("    SupportsSyncBack - " + thing.DeviceInfo.SupportsSyncBack);
                Console.WriteLine(thing.Location);
                Console.WriteLine(thing.Size);
                Console.WriteLine(thing.UpdateMode);
            }

        }

        public class RGBDevice : LightingController {

        }

        public class LogitechController : RGBDevice {

            public LogitechPerDeviceRGBDevice device;
            public LogitechRGBDeviceInfo deviceInfo;

            public LogitechController(LogitechPerDeviceRGBDevice device_) {
                device = device_;
                deviceInfo = device_.DeviceInfo;
            }

            public static void setAllToColor(int r, int g, int b) {

            }
        }
        

        public class NZXTController : LightingController {

        }

        public class CorsairController : LightingController {
            // Mapping Dictionaries Needed
            //    Part name -> Device Object
            //    Effect name -> Effect Object
        }

        public class RazerController : LightingController {

        }

        public class CoolerMasterController : LightingController {

        }

        public class AsusAuraController : LightingController {

        }

        public class MSIMysticLightController : LightingController {

        }

    }
}
