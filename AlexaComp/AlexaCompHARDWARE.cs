using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenHardwareMonitor.Hardware;
using log4net;
using log4net.Config;

using AlexaComp.Core;

namespace AlexaComp{
    class AlexaCompHARDWARE : AlexaCompCore{

        #region Properties
        private const string failMessage = "There was an error, please check the Alexa Comp log file.";
        public static List<string> partNames = new List<string>() { "GPU", "CPU", "RAM", "MAINBOARD", "HDD", "FANCONTROLLER" };
        public static Dictionary<string, Dictionary<string, string>> partDict = new Dictionary<string, Dictionary<string, string>>() {
            { "GPU", new Dictionary<string, string> {{"TEMP", ""}, {"LOAD", ""}, {"CLOCK_CORE", ""}, {"CLOCK_RAM", ""}}},
            { "CPU", new Dictionary<string, string> {{"TEMP", ""}, {"LOAD", ""}, {"CLOCK_CORE", ""}}},
            { "RAM", new Dictionary<string, string> {{"USED", "Used Memory"}, {"AVAILABLE", "Available Memory"}}},
            { "MAINBOARD", new Dictionary<string, string> {}}
        };
        #endregion

        #region Methods
        public static string partStat(string part, string stat, string tertiary){
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            partOneHot(computer, part);
            computer.Accept(updateVisitor);
            try {
                Console.WriteLine(partDict[part][stat]);
            } catch (KeyNotFoundException e) {
                clog("KeyNotFoundException during attempt to get part stat " + e.Message);
                Response res = new Response(false, failMessage, "", "");
                return "null";
            }

            int hwLength = computer.Hardware.Length;
            for (int i = 0; i < hwLength; i++){
                for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++){
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
        }

        public static string getSensorType(ISensor sensor) {
            return sensor.SensorType.ToString();
        }

        public static void assignSensors() {
            foreach (string part in partNames) {
                UpdateVisitor updateVisitor = new UpdateVisitor();
                Computer computer = new Computer();
                computer.Open();
                partOneHot(computer, part);
                computer.Accept(updateVisitor);
                for (int i = 0; i < computer.Hardware.Length; i++) {
                    for (int k = 0; k < computer.Hardware.Length; k++) {
                        for (int j = 0; j < computer.Hardware[k].Sensors.Length; j++) {
                            var currentSensor = computer.Hardware[k].Sensors[j];
                            if (currentSensor.SensorType == SensorType.Temperature) { // If sensor is a temp sensor
                                try {
                                    partDict[part]["TEMP"] = currentSensor.Name;
                                } catch (KeyNotFoundException) {}
                            }

                            // TODO: Implement switch case
                            if (currentSensor.SensorType == SensorType.Clock) {
                                Console.WriteLine("Clock Sensor");
                                if (currentSensor.Name.Contains("Core")) {
                                    Console.WriteLine("Core Clock Sensor");
                                    partDict[part]["CLOCK_CORE"] = currentSensor.Name;
                                } else if (currentSensor.Name.Contains("Memory")) {
                                    Console.WriteLine("Ram Clock Sensor");
                                    partDict[part]["CLOCK_RAM"] = currentSensor.Name;
                                }
                            }

                            if (currentSensor.SensorType == SensorType.Load) { // If sensor is a load sensor
                                try {
                                    partDict[part]["LOAD"] = currentSensor.Name;
                                }
                                catch (KeyNotFoundException) {}
                            } else if (currentSensor.SensorType == SensorType.Load && // Else if the hardware is a CPU get total load, not core loads.
                                part == "CPU" && "Total".Contains(currentSensor.Name)) {
                                    partDict[part]["LOAD"] = currentSensor.Name;
                            }
                        }
                    }
                    computer.Close();
                }
            }
            foreach(KeyValuePair<string, Dictionary<string, string>> pair in partDict) {
                Console.WriteLine("Part - " + pair.Key);
                foreach (KeyValuePair<string, string> pair2 in pair.Value) {
                    Console.WriteLine("    " + pair2.Key + " - " + pair2.Value);
                }
            }
        }

        // Clicking log all sensors button quickly triggers nullreferenceexception in this method
        public static void partOneHot(Computer comp, string part){
            if (part == "GPU") { comp.GPUEnabled = true; }
            else if (part == "CPU") { comp.CPUEnabled = true; }
            else if (part == "RAM") { comp.RAMEnabled = true; }
            else if (part == "MAINBOARD") { comp.MainboardEnabled = true; }
            else if (part == "HDD") { comp.HDDEnabled = true; }
            else if (part == "FANCONTROLLER") { comp.FanControllerEnabled = true; }
        }

        public static void getAllSensors(object openOnComplete){
            bool openOnCompleteBool = (bool)openOnComplete;
            ILog _SensorListLogger = LogManager.GetLogger("SensorListAppender");
            XmlConfigurator.Configure();
            foreach (string part in partNames){
                UpdateVisitor updateVisitor = new UpdateVisitor();
                Computer computer = new Computer();
                computer.Open();
                partOneHot(computer, part);
                computer.Accept(updateVisitor);
                for (int i = 0; i < computer.Hardware.Length; i++){ // For part
                    _SensorListLogger.Info(part + " - " + computer.Hardware[i].Name +
                                           " - Hardware Length " + computer.Hardware.Length.ToString());
                    for (int k = 0; k < computer.Hardware.Length; k++) { // For hardware in part
                        var currentHardware = computer.Hardware[k];
                        _SensorListLogger.Info(string.Format("    Hardware {0}/ {1} - " + currentHardware.Name, k + 1, computer.Hardware.Length.ToString()));
                        _SensorListLogger.Info("    Sensors Length - " + currentHardware.Sensors.Length.ToString());
                        for (int j = 0; j < computer.Hardware[k].Sensors.Length; j++){ // For sensor in hardware
                            _SensorListLogger.Info("\t" + computer.Hardware[k].Sensors[j].Name + " - " +
                                                            computer.Hardware[k].Sensors[j].SensorType + " - " +
                                                            computer.Hardware[k].Sensors[j].Value);
                        }
                    }
                    computer.Close();
                }
            }
            if (openOnCompleteBool){
                System.Diagnostics.Process.Start(AlexaComp.pathToDebug + "\\SensorList.txt");
            }
        }

        public static int loadFormat(float? loadFloat) { return (int)loadFloat; }
        public static int tempFormat(float? tempFloat) { return (int)tempFloat; }
        #endregion
    }

    public class UpdateVisitor : IVisitor{
        public void VisitComputer(IComputer computer){
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware){
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
