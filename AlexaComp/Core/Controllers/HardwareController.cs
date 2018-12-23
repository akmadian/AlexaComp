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

        public static void InitAllHardware() {
            Stopwatch totalHWInitTime = new Stopwatch();
            totalHWInitTime.Start();
            SensorDiscovery.InitSensors();
            RGBDiscovery.StartLightingThread();
            totalHWInitTime.Stop();
            Clog(String.Format("Initialized all hardware devices in {0} ms.", totalHWInitTime.ElapsedMilliseconds.ToString()));
            LogHardware();
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
            foreach (KeyValuePair<string, Hardware> pair in Devices) {
                Clog(pair.Value.ToString());
                string[] sensorLines = pair.Value.SensorsToString("    ").Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );
                foreach (string sensor in sensorLines) {
                    Clog(sensor);
                }
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
                            nHardware.Sensors[currentSensor.Name] = nSensor;
                        }

                        Devices[GenKey(currentHardware.HardwareType.ToString())] = nHardware;
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
            foreach (IRGBDevice device in surface.Devices) {
                Clog(String.Format("RGB Device Found - {0} {1} - Type: {2}",
                        device.DeviceInfo.Manufacturer,
                        device.DeviceInfo.Model,
                        device.DeviceInfo.DeviceType));
                var deviceinfo = device.DeviceInfo;
                if (Devices.ContainsKey(deviceinfo.DeviceType.ToString())) {
                    Devices[deviceinfo.DeviceType.ToString()].RGBDevice = device;
                } else {
                    Hardware nHardware = new Hardware(deviceinfo.DeviceType.ToString(), deviceinfo.Manufacturer,
                        deviceinfo.Model, deviceinfo.DeviceType.ToString(), device);
                    Devices[GenKey(deviceinfo.DeviceType.ToString())] = nHardware;
                }
            }
            Clog("RGB Device Intialization Complete - Found " + surface.Devices.Count().ToString() + " devices.");
            surface.UpdateMode = UpdateMode.Continuous;
        }

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
