using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace AlexaComp {

    class AlexaCompMQTT {
        public static MqttClient mqttClient = new MqttClient(AlexaComp.GetConfigValue("mqttServer"),
                                                             Convert.ToInt32(AlexaComp.GetConfigValue("mqttPORT")), 
                                                             false, null, null, MqttSslProtocols.None);

        public static void makeMQTT() {
            string clientID = Guid.NewGuid().ToString();

            mqttClient.Connect(clientID,
                AlexaComp.GetConfigValue("mqttUSER"),
                AlexaComp.GetConfigValue("mqttPASS"));
            AlexaComp._log.Info("MQTT Connected");
        }
        
        public static void mqttPublish(string msgToPost){
            mqttClient.Publish("AlexaComp/confirmations",
                Encoding.UTF8.GetBytes(msgToPost));
            AlexaComp._log.Info("MQTT Confirmation Published");
        }
    }
}
