using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using RGB.NET.Core;
using RGB.NET.Groups;

using AlexaComp.Core.Controllers;

namespace AlexaComp.Core {
    public class Hardware : AlexaCompCore {

        #region Properties
        public string Manufacturer;
        public string Name;
        public string Type;
        public string GenericName;

        public Dictionary<string, Sensor_> Sensors = new Dictionary<string, Sensor_>();
        public IRGBDevice RGBDevice = null;
        public ListLedGroup ledGroup = null;
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

        public Hardware(string Type, string GenericName, IRGBDevice origin) {
            this.Type = Type;
            this.GenericName = GenericName;
            this.RGBDevice = origin;
            this.ledGroup = new ListLedGroup(origin);
        }

        public Hardware(string Type, string Manufacturer, string Name, string GenericName, IRGBDevice origin) {
            this.Type = Type;
            this.Manufacturer = Manufacturer;
            this.Name = Name;
            this.GenericName = GenericName;
            this.RGBDevice = origin;
            this.ledGroup = new ListLedGroup(origin);
        }
        #endregion

        #region Methods
        public void RefreshSensors() {
            SensorDiscovery.RefreshHardware(this);
        }

        public string GetSensorValue(string sensorName) {
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

        public bool HasRGBDevice() {
            return RGBDevice != null ? true : false;
        }

        public bool HasLEDGroup() {
            return ledGroup != null ? true : false;
        }

        public int SensorsLength() => Sensors.Count;

        public override string ToString() {
            return string.Format("Hardware Object -- Name: {0}, Type: {1}, Sensor Length: {2}, RGB Device Present: {3}", 
                Name, Type, SensorsLength(), RGBDevice != null ? true : false);
        }
        #endregion
    }
}