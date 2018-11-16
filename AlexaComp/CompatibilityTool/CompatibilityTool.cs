using System;
using System.Collections.Generic;

using log4net;
using log4net.Config;
using OpenHardwareMonitor.Hardware;
using RGB.NET.Core;
using RGB.NET.Devices.Asus;
using RGB.NET.Devices.CoolerMaster;
using RGB.NET.Devices.Corsair;
using RGB.NET.Devices.DMX;
using RGB.NET.Devices.Logitech;
using RGB.NET.Devices.Novation;
using RGB.NET.Devices.Razer;

namespace CompatibilityTool {
    class CompatibilityTool {

        public static readonly ILog _log = LogManager.GetLogger(typeof(CompatibilityTool));

        static void Main(string[] args) {
            XmlConfigurator.Configure(); // Configure Logger
            plog("Compatibility Tool v0.1");
            plog("=====BEGIN LOG=====");

            foreach (string arg in args) { // Check CLI args.
                if ("-rgbonly".Contains(arg)) {
                    plog("-rgbbonly in args");
                    RGBDeviceLogger RGBLogger = new RGBDeviceLogger();
                }
            }
            
            if (args.Length == 0) { // If no CLI args were provided, run both.
                RGBDeviceLogger RGBLogger = new RGBDeviceLogger();
                SensorInfoLogger SensorLogger = new SensorInfoLogger();
            }

            plog("=====END LOG=====");

            Console.WriteLine("Logging Complete... Press any key to continue.");
            Console.ReadLine();
        }

        /// <summary>
        /// Prints and logs the value given in the message param.
        /// </summary>
        /// <param name="message"></param>
        public static void plog(string message) {
            _log.Info(message);
            Console.WriteLine(message);
        }


        /// <summary>
        /// Logs all RGB devices and device attributes.
        /// Runs logging process on instantiation.
        /// </summary>
        public class RGBDeviceLogger {

            public static RGBSurface surface = RGBSurface.Instance;

            /// <summary>
            /// Loads devices, then for each one, logs all device and deviceinfo attributes.
            /// </summary>
            public RGBDeviceLogger() {
                plog("=====BEGIN RGB LOG=====");
                surface.Exception += args => plog(args.Exception.Message);
                surface.UpdateMode = UpdateMode.Continuous;

                try {
                    loadAllDevices();
                } catch (Exception) {
                    plog("EXCEPTION: Exception caught during loading devices.");
                }

                foreach (var device in surface.Devices) {
                    plog(String.Format("RGB Device Found - {0} {1} - Type: {2}",
                        device.DeviceInfo.Manufacturer,
                        device.DeviceInfo.Model,
                        device.DeviceInfo.DeviceType));

                    plog("    DEVICE");
                    plog("\tDeviceInfo Object - " + device.DeviceInfo.ToString());
                    plog("\tdevice.Location   - " + device.Location.ToString());
                    plog("\tdevice.Size       - " + device.Size.ToString());
                    plog("\tdevice.UpdateMode - " + device.UpdateMode.ToString());
                    plog("    DEVICE INFO");
                    try {
                        plog("        Image            - " + device.DeviceInfo.Image.ToString());
                    } catch (NullReferenceException) {
                        plog("        Image            - Image Object Returned Null");
                    }
                    plog("\tDeviceType       - " + device.DeviceInfo.DeviceType.ToString());
                    plog("\tLighting         - " + device.DeviceInfo.Lighting.ToString());
                    plog("\tManufacturer     - " + device.DeviceInfo.Manufacturer);
                    plog("\tModel            - " + device.DeviceInfo.Model);
                    plog("\tSupportsSyncBack - " + device.DeviceInfo.SupportsSyncBack.ToString());
                }
                plog("=====END GRB LOG=====");
            }

            /// <summary>
            /// Loads all detected devices into the RGB surface object
            /// </summary>
            public void loadAllDevices() {
                surface.LoadDevices(CorsairDeviceProvider.Instance);
                surface.LoadDevices(LogitechDeviceProvider.Instance);
                surface.LoadDevices(CoolerMasterDeviceProvider.Instance);
                surface.LoadDevices(RazerDeviceProvider.Instance);
                surface.LoadDevices(NovationDeviceProvider.Instance);
                surface.LoadDevices(DMXDeviceProvider.Instance);

                // Loading ASUS device is broken at the minute.
                try {
                    // surface.LoadDevices(AsusDeviceProvider.Instance);
                }
                catch (AccessViolationException) {
                    plog("EXCEPTION: AccessViolationException Caught when attempting to load ASUS devices.");
                }
                catch (Exception) {
                    plog("EXCEPTION: Generic Exception Caught when attempting to load ASUS devices.");
                }
            }

        }

        /// <summary>
        /// Logs all part, hardware, and sensor info and attributes.
        /// Runs logging process on instantiation.
        /// </summary>
        public class SensorInfoLogger {

            public static List<string> partNames = new List<string>() { "GPU", "CPU", "RAM", "MAINBOARD", "HDD", "FANCONTROLLER" };

            public SensorInfoLogger() {
                plog("=====BEGIN SENSOR LOG=====");
                getAllSensors();
                plog("=====END SENSOR LOG=====");
            }

            /// <summary>
            /// Switches one part's enabled value to true, and all others to false. 
            /// </summary>
            /// <param name="comp">The computer object containing the parts.</param>
            /// <param name="part">The part name to set to enabled.</param>
            public static void partOneHot(Computer comp, string part) {
                if (part == "GPU") { comp.GPUEnabled = true; }
                else if (part == "CPU") { comp.CPUEnabled = true; }
                else if (part == "RAM") { comp.RAMEnabled = true; }
                else if (part == "MAINBOARD") { comp.MainboardEnabled = true; }
                else if (part == "HDD") { comp.HDDEnabled = true; }
                else if (part == "FANCONTROLLER") { comp.FanControllerEnabled = true; }
            }


            /// <summary>
            /// Gets and logs all parts, hardware, and sensors
            /// </summary>
            public static void getAllSensors() {
                foreach (string part in partNames) {
                    UpdateVisitor updateVisitor = new UpdateVisitor();
                    Computer computer = new Computer();
                    computer.Open();
                    partOneHot(computer, part);
                    computer.Accept(updateVisitor);
                    for (int i = 0; i < computer.Hardware.Length; i++) { // For part
                        var part_ = computer.Hardware[i];
                        plog("PART");
                        plog("    Object Type - " + part_.GetType().ToString());
                        plog(part + " - Hardware Length " + computer.Hardware.Length.ToString());
                        for (int k = 0; k < computer.Hardware.Length; k++) { // For hardware in part
                            plog("    HARDWARE");
                            var currentHardware = computer.Hardware[k];
                            plog(string.Format("    Hardware {0}/ {1} - " + currentHardware.Name, k + 1, computer.Hardware.Length.ToString()));
                            plog("\tObject Type OBJ - " + currentHardware.GetType().ToString());
                            plog("\tSensors Length  - " + currentHardware.Sensors.Length.ToString());
                            plog("\tName            - " + currentHardware.Name);
                            plog("\tIdentifier      - " + currentHardware.Identifier);
                            plog("\tHardwareType    - " + currentHardware.HardwareType);
                            plog("\tParent          - " + currentHardware.Parent);
                            plog("\tSubHardware OBJ - " + currentHardware.SubHardware);
                            plog("\t    IsFixedSize    - " + currentHardware.SubHardware.IsFixedSize);
                            plog("\t    IsReadOnly     - " + currentHardware.SubHardware.IsReadOnly);
                            plog("\t    IsSynchronized - " + currentHardware.SubHardware.IsSynchronized);
                            plog("\t    Length         - " + currentHardware.SubHardware.Length);
                            plog("\t    Rank           - " + currentHardware.SubHardware.Rank);
                            plog("\t    SyncRoot       - " + currentHardware.SubHardware.SyncRoot);
                            plog("\tSensors     OBJ - " + currentHardware.Sensors);
                            plog("\t    IsFixedSize    - " + currentHardware.Sensors.IsFixedSize);
                            plog("\t    IsReadOnly     - " + currentHardware.Sensors.IsReadOnly);
                            plog("\t    IsSynchronized - " + currentHardware.Sensors.IsSynchronized);
                            plog("\t    Length         - " + currentHardware.Sensors.Length);
                            plog("\t    Rank           - " + currentHardware.Sensors.Rank);
                            plog("\t    SyncRoot   OBJ - " + currentHardware.Sensors.SyncRoot);
                            plog("\tSENSORS");
                            foreach (ISensor sensor in computer.Hardware[k].Sensors) {
                                plog("\tSENSOR");
                                plog("\t    Name            - " + sensor.Name);
                                plog("\t    SensorType      - " + sensor.SensorType);
                                plog("\t    Control         - " + sensor.Control);
                                plog("\t    Identifier      - " + sensor.Identifier);
                                plog("\t    Value           - " + sensor.Value);
                                plog("\t    Values      OBJ - " + sensor.Values);
                                plog("\t    Index           - " + sensor.Index);
                                plog("\t    Hardware    OBJ - " + sensor.Hardware);
                                plog("\t    IsDefaultHidden - " + sensor.IsDefaultHidden);
                                plog("\t    Parameters  OBJ - " + sensor.Parameters);
                            }
                        }
                        computer.Close();
                    }
                }
            }
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
