/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/

'use strict';
console.log('--START--')
const config = require('./config.json');
const Alexa = require('alexa-sdk');
const mqttBroker = ('mqtt');
const net = require('net');
const http = require('http');
console.log('Required Modules Imported')
const APP_ID = config.ASK.APP_ID;
const HOST = config.SOCKET.HOST;
const PORT = config.SOCKET.PORT;
const auth_key = config.SOCKET.AUTH;
console.log('Vars Imported From Config File')
var client = new net.Socket();
console.log('Socket Created')
var server = new net.Server();
console.log('Server Created')
const SKILL_NAME = 'ComputerInteract';

// MQTT
var mqttClient = mqttBroker.connect('https://m11.cloudmqtt.com:15387');

mqttClient.on('connect', function(){
    mqttClient.subscribe('AlexaComp/confirmations');
    console.log("Client subscribed")
    mqttClient.publish('AlexaComp/confirmations', '1');
    console.log("published")
    
})

mqttClient.on('message', (topic, message) => {
    console.log("new MQTT from - " + topic + " -- " + message)
})

const STOP_MESSAGE = 'Goodbye!';
const HELP_MESSAGE = 'You can ask me to launch a program, tell your computer to ' + 
                    'do something, or get some hardware information like CPU temp.'
const successful = 'Socket created and data has been sent'

function SendJson(js){
    console.info("Start Server");
    client.connect(PORT, HOST, function() {
        var js_ = JSON.stringify(js)
        console.info(js_)
        console.log('BEFORE WRITE')
        try{
            client.write(js_);
        } catch (TypeError) {
            console.warn('TypeError caught while sending json.')
        }
    })
}
// Code
const handlers = {
    'LaunchProgramIntent': function () {
        var programName = this.event.request.intent.slots.ProgramName.resolutions.resolutionsPerAuthority[0].values[0].value.id;

        console.info("ProgramName - " + programName);
        var j = {"AUTH": auth_key, "COMMAND": "LAUNCH", "PRIMARY": programName, "SECONDARY": "null"};
        SendJson(j);
    },
    
    'GetComputerStat': function () {
        var part = this.event.request.intent.slots;
        var stat = null;
        var j = {"AUTH": auth_key, "COMMAND": "GETCOMPSTAT", "PRIMARY": part, "SECONDARY": stat};
        SendJson(j);
        this.response.speak("This intent is not working yet");
        this.emit(':responseReady:');
    },
    
    'ComputerCommandIntent': function(){
      var command = null;
      var j = {"AUTH": auth_key, "COMMAND": "COMPUTERCOMMAND", "PRIMARY": command, "SECONDARY": "null"};
      SendJson(j);
      this.response.speak("Command Successfully Issued");
      this.emit(':responseReady:');
    },
    
    'GetNewFactIntent': function () {
        const speechOutput = successful;
        console.log('speechOutput Definition Successful');

        this.response.cardRenderer(SKILL_NAME, successful)
        console.log('cardRenderer executed');
        this.response.speak(speechOutput);
        console.log('Speak executed');
        this.emit(':responseReady');
    },
    
    'AMAZON.HelpIntent': function () {
        const speechOutput = 'HelpIntent has not been set yet.';
        const reprompt = 'Help reprompt has not been set yet.';

        this.response.speak(speechOutput).listen(reprompt);
        this.emit(':responseReady');
    },
    
    'AMAZON.CancelIntent': function () {
        this.response.speak(STOP_MESSAGE);
        this.emit(':responseReady');
    },
    
    'AMAZON.StopIntent': function () {
        this.response.speak(STOP_MESSAGE);
        this.emit(':responseReady');
    },
};

exports.handler = function (event, context, callback) {
    const alexa = Alexa.handler(event, context, callback);
    alexa.APP_ID = config.ASK.APP_ID;
    alexa.registerHandlers(handlers);
    alexa.execute();
};
