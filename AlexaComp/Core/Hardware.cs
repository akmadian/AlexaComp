using System;
using System.Collections.Generic;
using System.Management;
using Microsoft;
using System.Text;

using Microsoft.Win32;

using WmiLight;

// using HardwareProviders;
using HardwareProviders.CPU;
using HardwareProviders.Board;

using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;

namespace AlexaComp.Core {
    public class Hardware {

        public string Manufacturer;

        public string Name;

        public string Type;
        

        public Dictionary<string, Sensor_> Sensors = new Dictionary<string, Sensor_>();


        public Hardware(string type_) {
            Type = type_;
        }

        public void populateSensors() {
            
        }

        public void refreshSensors() {

        }

        /*
        public Sensor getSensor() {
            return null;
        }*/

        public string getSensorValue(string sensorName) {
            Sensor_ sensor = Sensors[sensorName];
            return sensor.Value.ToString();
        }
    }
}