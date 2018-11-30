using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenHardwareMonitor;
using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor.Hardware;

namespace AlexaComp.Core {
    public class Sensor_ {

        static readonly Dictionary<SensorType, string> Units = new Dictionary<SensorType, string> {
            {SensorType.Voltage, "V"},
            {SensorType.Clock, "MHz"},
            {SensorType.Temperature, "°C"},
            {SensorType.Load, "%"},
            {SensorType.Fan, "RPM"},
            {SensorType.Flow, "L/h"},
            {SensorType.Control, "%"},
            {SensorType.Level, "%"},
            {SensorType.Factor, "1"},
            {SensorType.Power, "W"},
            {SensorType.Data, "GB"}
        };

        public string Name;

        public float Value;

        public string SensorPath;

        public string Type;

        public ISensor OriginSensor;

        public Sensor_(string Name_, string type_) {
            Name = Name_;
            Type = type_;
        }

        public Sensor_(string Name_, string type_, ISensor Origin) {
            Name = Name_;
            Type = type_;
            OriginSensor = Origin;
        }

        public string getUnit(Sensor_ sens) {
            return Units[sens.OriginSensor.SensorType];
        }
    }
}
