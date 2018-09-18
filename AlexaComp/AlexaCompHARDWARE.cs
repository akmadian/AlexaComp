using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using log4net;
using log4net.Config;

namespace AlexaComp{
    class AlexaCompHARDWARE{

        public static string partStat(string part, string stat){
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            partOneHot(computer, part);
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++){
                for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++){
                    if (computer.Hardware[i].Sensors[j].Name == stat) {
                        if (computer.Hardware[i].Sensors[j].SensorType.ToString() == "Temperature") {
                            return tempFormat(computer.Hardware[i].Sensors[j].Value).ToString() + "Degrees";
                        } else if (computer.Hardware[i].Sensors[j].SensorType.ToString() == "Load") {
                            return loadFormat(computer.Hardware[i].Sensors[j].Value).ToString() + "%";
                        } else if (computer.Hardware[i].Sensors[j].SensorType.ToString() == "Load"){
                            return computer.Hardware[i].Sensors[j].Value.ToString() + "MHz";
                        }
                        // string statValue = Math.Round(Convert.ToDecimal(computer.Hardware[i].Sensors[j].Value), 1).ToString();
                        // return statValue;
                    }
                }
                computer.Close();
                return "null";
            }
            return "null";
        }

        public static void partOneHot(Computer comp, string part){
            if (part == "GPU") { comp.GPUEnabled = true; }
            else if (part == "CPU") { comp.CPUEnabled = true; }
            else if (part == "RAM") { comp.RAMEnabled = true; }
            else if (part == "MAINBOARD") { comp.MainboardEnabled = true; }
            else if (part == "HDD") { comp.HDDEnabled = true; }
            else if (part == "FANCONTROLLER") { comp.FanControllerEnabled = true; }
        }

        public static void getAllSensors(string part){
            // List<string> partNames = new List<string>() {"GPU", "CPU", "RAM", "MAINBOARD"};
            // foreach (string part in partNames){
            ILog _SensorListLogger = LogManager.GetLogger("SensorListAppender");
            XmlConfigurator.Configure();
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            partOneHot(computer, part);
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++){
                _SensorListLogger.Info(part + " - " + computer.Hardware[i].Name + 
                                       " - Hardware Length " + computer.Hardware.Length.ToString());
                for (int k = 0; k < computer.Hardware.Length; k++) {
                    _SensorListLogger.Info("    " + computer.Hardware[k].Name);
                    _SensorListLogger.Info("    Sensors Length - " + computer.Hardware[i].Sensors.Length.ToString());
                    for (int j = 0; j < computer.Hardware[k].Sensors.Length; j++){
                        _SensorListLogger.Info("        " + computer.Hardware[k].Sensors[j].Name + " - " +
                                                            computer.Hardware[k].Sensors[j].SensorType + " - " +
                                                            computer.Hardware[k].Sensors[j].Value);
                    }
                }
                computer.Close();
            }
        }

        public static int loadFormat(float? loadFloat) { return (int)loadFloat; }
        public static int tempFormat(float? tempFloat) { return (int)tempFloat; }
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
