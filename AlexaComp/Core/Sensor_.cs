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

        #region Properties
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
        #endregion

        #region Constructors
        public Sensor_(string Name, string Type) {
            this.Name = Name;
            this.Type = Type;
        }

        public Sensor_(string Name, string Type, ISensor Origin) {
            this.Name = Name;
            this.Type = Type;
            OriginSensor = Origin;
        }
        #endregion

        #region Methods
        public string getUnit() {
            return Units[OriginSensor.SensorType];
        }

        public static string GetSensorType(ISensor sensor) {
            return sensor.SensorType.ToString();
        }

        public override string ToString() {
            return string.Format("Sensor Object -- Name: {0}, Type: {1}, Value: {2}", Name, Type, Value.ToString());
        }
        #endregion
    }
}
