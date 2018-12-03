using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using AlexaComp.Core.Controllers;

namespace AlexaComp.Core {
    public class Hardware : AlexaCompCore {

        #region Properties
        public string Manufacturer;
        public string Name;
        public string Type;
        public string GenericName;

        public Dictionary<string, Sensor_> Sensors = new Dictionary<string, Sensor_>();
        #endregion

        #region Constructors
        public Hardware(string Type, string GenericName) {
            this.Type = Type;
            this.GenericName = GenericName;
        }

        public Hardware(string Type, string Manufacturer, string Name, string GenericName) {
            this.Type = Type;
            this.Manufacturer = Manufacturer;
            this.Name = Name;
            this.GenericName = GenericName;
        }
        #endregion

        #region Methods
        public void RefreshSensors() {
            HardwareController.RefreshHardware(this);
        }

        public string getSensorValue(string sensorName) {
            Sensor_ sensor = Sensors[sensorName];
            return sensor.Value.ToString();
        }

        public string SensorsToString(string prefix = "") {
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<string, Sensor_> pair in Sensors) {
                sb.Append(prefix);
                sb.Append(pair.Value.ToString() + "\n");
            }
            try {
                sb.Length--; // Remove last \n
            } catch (ArgumentOutOfRangeException) { }
            return sb.ToString();
        }

        public int SensorsLength() => Sensors.Count;

        public override string ToString() {
            return string.Format("Hardware Object -- Name: {0}, Type: {1}, Sensor Length: {2}", Name, Type, SensorsLength());
        }
        #endregion
    }
}