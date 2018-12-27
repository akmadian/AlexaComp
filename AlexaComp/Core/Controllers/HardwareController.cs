using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;

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

namespace AlexaComp.Core.Controllers {
    public class HardwareController : AlexaCompCore {

        public static List<string> partNames = new List<string>() { "GPU", "CPU", "RAM", "MAINBOARD", "HDD", "FANCONTROLLER" };

        public static Dictionary<string, Dictionary<string, string>> partDict = new Dictionary<string, Dictionary<string, string>>() {
            { "GPU", new Dictionary<string, string> {{"TEMP", ""}, {"LOAD", ""}, {"CLOCK_CORE", ""}, {"CLOCK_RAM", ""}}},
            { "CPU", new Dictionary<string, string> {{"TEMP", ""}, {"LOAD", ""}, {"CLOCK_CORE", ""}}},
            { "RAM", new Dictionary<string, string> {{"USED", "Used Memory"}, {"AVAILABLE", "Available Memory"}}},
            { "MAINBOARD", new Dictionary<string, string> {}}
        };

        public static Dictionary<string, Hardware> tempDevices = new Dictionary<string, Hardware>();
        public static List<string> toRemove = new List<string>();


        // TODO : Make method of combining multiple RGB DRAM devices under one ILedGroup.
        //          Also for fans and strips
        public static void InitAllHardware() {
            Stopwatch totalHWInitTime = new Stopwatch();
            totalHWInitTime.Start();
            SensorDiscovery.InitSensors();
            RGBDiscovery.StartLightingThread();
            totalHWInitTime.Stop();
            Clog(String.Format("Initialized all hardware devices in {0} ms.", totalHWInitTime.ElapsedMilliseconds.ToString()));
            Clog("====BEFORE MERGE TEMPDEVICES====");
            LogHardware(tempDevices);
            MergeHardware();
            RemoveDuplicates();
            Devices = tempDevices;
            LogHardware(Devices);
            tempDevices.Clear();
            Clog("tempDevices Dict Cleared.");
        }

        public static string GenKey(string hwType) {
            int outKey = 0;
            while (true) {
                if (Devices.ContainsKey(hwType + outKey.ToString())) {
                    outKey++;
                }
                else {
                    break;
                }
            }
            return hwType + outKey.ToString();
        }

        public static void LogHardware() {
            foreach (KeyValuePair<string, Hardware> pair in tempDevices) {
                Clog(pair.Value.ToString());
                if (pair.Value.SensorsLength() > 0) {
                    string[] sensorLines = pair.Value.SensorsToString("    ").Split(
                        new[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );
                    foreach (string sensor in sensorLines) {
                        Clog(sensor);
                    }
                }
            }
        }

        public static void LogHardware(Dictionary<string, Hardware> dict) {
            foreach (KeyValuePair<string, Hardware> pair in dict) {
                Clog(pair.Value.ToString());
                if (pair.Value.SensorsLength() > 0) {
                    string[] sensorLines = pair.Value.SensorsToString("    ").Split(
                        new[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );
                    foreach (string sensor in sensorLines) {
                        Clog(sensor);
                    }
                }
            }
        }

        public static void MergeHardware() {
            Clog("Merging Hardware");
            Stopwatch mergeTimer = new Stopwatch();
            mergeTimer.Start();
            foreach (KeyValuePair<string, Hardware> pair in tempDevices) {
                Hardware device = pair.Value;
                bool matchFound = false;
                if (device.HasLEDGroup() || device.HasRGBDevice()) {
                    Clog("    RGB Device Found In Devices.");
                    foreach (KeyValuePair<string, Hardware> pair2 in tempDevices) {
                        Hardware device2 = pair2.Value;
                        if ((device.Type == device2.Type) && !device2.HasLEDGroupOrRGBDevice()) {
                            PerformMerge(ref matchFound, device, device2);
                        }
                        else if (device.Type == "DRAM" && device2.Type == "RAM") {
                            PerformMerge(ref matchFound, device, device2);
                        }
                        else if (device.Type == "GraphicsCard" && pair2.Key.IndexOf("gpu", StringComparison.OrdinalIgnoreCase) >= 0) {
                            Clog("    GPU Match Found");
                            PerformMerge(ref matchFound, device, device2);
                        }
                    }

                    if (!matchFound) {
                        Clog("        No Matches Found.");
                    }
                }
            }
            mergeTimer.Stop();
            Clog("Merged Hardware in " + mergeTimer.ElapsedMilliseconds.ToString() + " ms.");
        }

        public static void PerformMerge(ref bool flag, Hardware originDevice, Hardware destinationDevice) {
            flag = true;
            Clog("    Device Type Match Found For " + originDevice.ToString());
            Clog("        Merging With " + destinationDevice.ToString());
            destinationDevice.MergeRGB(originDevice);
            toRemove.Add(originDevice.DictName);
        }

        public static void RemoveDuplicates() {
            Clog("Removing Merged Devices");

            // Print toRemove devices
            StringBuilder sb = new StringBuilder();
            sb.Append("    toRemove List: [");
            foreach (string key in toRemove) {
                sb.Append(key + ", ");
            }
            sb.Append("]");
            Clog(sb.ToString());

            foreach (string key in toRemove) {
                tempDevices.Remove(key);
            }

            // Sometimes a ghost "none" device appears
            tempDevices.Remove("none0");
            tempDevices.Remove("none1");
            tempDevices.Remove("none2");
            tempDevices.Remove("none3");

            Clog("    Removal Operation Complete");
            toRemove.Clear();
            Clog("    toRemove List Cleared.");
        }

        public static void GroupMultipleDevices() {
            Clog("Starting Multiple Device Grouping.");
            Hardware fanHardware = new Hardware("Fan", "Fan");
            Hardware ledStripHardware = new Hardware("LedStripe", "LedStripe");

            // Group DRAM, Fan, and Strip LEDS into one object.
            Clog("    Beginning Grouping.");
            foreach (KeyValuePair<string, Hardware> pair in tempDevices) {
                if (pair.Value.HasLEDGroupOrRGBDevice()) {
                    if (pair.Value.Type == "LedStripe" || pair.Value.Type == "DRAM" || pair.Value.Type == "Fan") {
                        Clog("        Device to group found.");
                        Hardware device = pair.Value;

                        if (device.Type == "LedStripe") {
                            ledStripHardware.Manufacturer = device.Manufacturer; ledStripHardware.Name = device.Name;
                            ledStripHardware.ledGroup.AddLeds(device.RGBDevice);
                        }
                        else if (device.Type == "DRAM") {
                            tempDevices["ram0"].ledGroup.AddLeds(device.RGBDevice);
                        }
                        else if (device.Type == "Fan") {
                            fanHardware.Manufacturer = device.Manufacturer; fanHardware.Name = device.Name;
                            fanHardware.ledGroup.AddLeds(device.RGBDevice);
                        }
                    }
                }
            }

            // Remove old devices
            Clog("    Beginning Old Multiple Device Removal");
            foreach (KeyValuePair<string, Hardware> pair in tempDevices) {
                if (pair.Value.Type == "LedStripe" || pair.Value.Type == "Fan" || pair.Value.Type == "DRAM") {
                    tempDevices.Remove(pair.Key);
                }
            }

            // Add fanHardware if defined
            if (fanHardware.Manufacturer != null) {
                Clog("    Adding fanHardware");
                string key = GenKey(fanHardware.Type.ToString().ToLower());
                tempDevices[key] = fanHardware;
            }

            // Add ledStripHardware if defined
            if (ledStripHardware.Manufacturer != null) {
                Clog("    Adding ledStripHardware");
                string key = GenKey(ledStripHardware.Type.ToString().ToLower());
                tempDevices[key] = ledStripHardware;
            }
        }

        public static int LoadFormat(float? loadFloat) { return (int)loadFloat; }
        public static int TempFormat(float? tempFloat) { return (int)tempFloat; }

        /*
        public static string partStat(string part, string stat, string tertiary) {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            partOneHot(computer, part);
            computer.Accept(updateVisitor);
            try {
                Console.WriteLine(partDict[part][stat]);
            }
            catch (KeyNotFoundException e) {
                clog("KeyNotFoundException during attempt to get part stat " + e.Message);
                Response res = new Response(false, failMessage, "", "");
                return "null";
            }

            int hwLength = computer.Hardware.Length;
            for (int i = 0; i < hwLength; i++) {
                for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++) {
                    foreach (ISensor sensor in computer.Hardware[i].Sensors) {
                        if (sensor.Name == partDict[part][stat]) {
                            if (getSensorType(sensor) == tertiary) {
                                return sensor.Value.ToString() + " Mega Hertz";
                            }
                        }
                    }
                }
                computer.Close();
                return "null";
            }
            return "null";
        }*/
    }

    public class SensorDiscovery : HardwareController {
        public static void InitSensors() {
            Stopwatch loop = new Stopwatch();
            Stopwatch method = new Stopwatch();
            method.Start();
            Devices.Clear();
            foreach (string part in partNames) {
                loop.Start();
                UpdateVisitor updateVisitor = new UpdateVisitor();
                Computer computer = new Computer();
                computer.Open();
                PartOneHot(computer, part);
                computer.Accept(updateVisitor);
                for (int i = 0; i < computer.Hardware.Length; i++) {
                    for (int k = 0; k < computer.Hardware.Length; k++) {
                        var currentHardware = computer.Hardware[k];

                        Hardware nHardware = new Hardware(currentHardware.HardwareType.ToString(), part) {
                            Name = currentHardware.Name
                        };

                        for (int j = 0; j < computer.Hardware[k].Sensors.Length; j++) {
                            var currentSensor = computer.Hardware[k].Sensors[j];
                            // Console.WriteLine("    nSensor -- " + currentSensor.Name + " - " + currentSensor.SensorType.ToString() + " - " + currentSensor.Value.ToString());
                            Sensor_ nSensor = new Sensor_(currentSensor.Name, currentSensor.SensorType.ToString(), currentSensor) {
                                Value = (float)currentSensor.Value
                            };
                            nHardware.Sensors[currentSensor.Name.ToLower()] = nSensor;
                        }
                        string key = GenKey(currentHardware.HardwareType.ToString().ToLower());
                        nHardware.DictName = key;
                        tempDevices[key] = nHardware;
                        loop.Stop();
                        Clog(String.Format("HW Initialization -- {0} initialized in {1} ms. {2} Sensors found.", part, loop.ElapsedMilliseconds, nHardware.SensorsLength()));
                        loop.Reset();
                    }
                    computer.Close();
                }
            }
            method.Stop();
            Clog(String.Format("Hardware Initialized in {0} ms.", method.ElapsedMilliseconds));
        }
        public static void RefreshHardware(Hardware obj) {
            obj.Sensors.Clear();
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            PartOneHot(computer, obj.GenericName);
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++) {
                for (int k = 0; k < computer.Hardware.Length; k++) {
                    var currentHardware = computer.Hardware[k];

                    for (int j = 0; j < currentHardware.Sensors.Length; j++) {
                        var currentSensor = currentHardware.Sensors[j];
                        Sensor_ nSensor = new Sensor_(currentSensor.Name, currentSensor.SensorType.ToString(), currentSensor) {
                            Value = (float)currentSensor.Value
                        };
                        obj.Sensors[currentSensor.Name] = nSensor;
                    }
                }
                computer.Close();
            }
        }

        public static void PartOneHot(Computer comp, string part) {
            if (part == "GPU") { comp.GPUEnabled = true; }
            else if (part == "CPU") { comp.CPUEnabled = true; }
            else if (part == "RAM") { comp.RAMEnabled = true; }
            else if (part == "MAINBOARD") { comp.MainboardEnabled = true; }
            else if (part == "HDD") { comp.HDDEnabled = true; }
            else if (part == "FANCONTROLLER") { comp.FanControllerEnabled = true; }
        }
    }

    public class RGBDiscovery : HardwareController {
        private static RGBSurface surface = RGBSurface.Instance;


        public static void StartLightingThread() {
            LoadDevices();
            Clog("    RGB Devices Loaded Into Surface");
            foreach (IRGBDevice device in surface.Devices) {
                Clog(String.Format("RGB Device Found - {0} {1} - Type: {2}",
                        device.DeviceInfo.Manufacturer,
                        device.DeviceInfo.Model,
                        device.DeviceInfo.DeviceType));
                var deviceinfo = device.DeviceInfo;
                Clog("    Type - " + deviceinfo.DeviceType.ToString());

                if (Devices.ContainsKey(deviceinfo.DeviceType.ToString())) {
                    Clog("    Adding to existing device.");
                    tempDevices[deviceinfo.DeviceType.ToString().ToLower()].RGBDevice = device;
                } else {
                    Clog("    Creating New Device");
                    string key = GenKey(deviceinfo.DeviceType.ToString().ToLower());
                    Hardware nHardware = new Hardware(deviceinfo.DeviceType.ToString(), deviceinfo.Manufacturer,
                        deviceinfo.Model, deviceinfo.DeviceType.ToString(), device);
                    nHardware.DictName = key;
                    tempDevices[key] = nHardware;
                }
            }
            Clog("RGB Device Intialization Complete - Found " + surface.Devices.Count().ToString() + " devices.");
            
        }

        public static void LoadDevices() {
            surface.Exception += args => AlexaComp._log.Info(args.Exception.Message);
            Clog("Loading RGB Devices");

            try {
                //surface.LoadDevices(AsusDeviceProvider.Instance);
                Clog("    Loaded Devices From AsusDeviceProvider.");
            } catch (AccessViolationException e) {
                Clog("Caught AccessViolationException when loading AsusDeviceProvider. Skipping...");
            }
            surface.LoadDevices(LogitechDeviceProvider.Instance);
            Clog("    Loaded Devices From LogitechDeviceProvider.");
            surface.LoadDevices(CorsairDeviceProvider.Instance);
            Clog("    Loaded Devices From CorsairDeviceProvider.");
            surface.LoadDevices(CoolerMasterDeviceProvider.Instance);
            Clog("    Loaded Devices From CoolerMasterDeviceProvider.");
            surface.LoadDevices(DMXDeviceProvider.Instance);
            Clog("    Loaded Devices From DMXDeviceProvider.");
            surface.LoadDevices(NovationDeviceProvider.Instance);
            Clog("    Loaded Devices From NovationDeviceProvider.");
            surface.LoadDevices(RazerDeviceProvider.Instance);
            Clog("    Loaded Devices From RazerDeviceProvider.");
        }
    }

    public class UpdateVisitor : IVisitor {
        public void VisitComputer(IComputer computer) {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware) {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
