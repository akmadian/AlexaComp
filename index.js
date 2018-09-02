/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/

'use strict';
console.log('--START--')
const config = require('./config.json');
const Alexa = require('alexa-sdk');
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
const STOP_MESSAGE = 'Goodbye!';

client.connect(PORT, HOST, function() {
    var j = {"AUTH": auth_key, "TASK": {"COMMAND": "LAUNCH", "PROGRAM": "GOOGLECHROME"},};
    var js_ = JSON.stringify(j)
    console.info(js_)
    console.log('BEFORE WRITE')
    try{
        client.write(js_);
    } catch (TypeError) {
        console.warn('TypeError caught while sending json.')
    }
    console.log('JSON SENT')
    
    console.log('Pre Server')
    net.createServer(function (socket){
        console.log('net.createServer Done')
        socket.on('data', function(data){
            console.log('Data Received')
            console.log(data)
        })
        
    }).listen(5000)
    console.log('Post Server')
})

client.on('data', function(data){
    console.log(data);
    client.destroy();
})

const successful = 'Socket created and data has been sent'

// Code
const handlers = {
    'LaunchRequest': function () {
        this.response.speak(successful)
    },
    'GetNewFactIntent': function () {
        const speechOutput = successful;

        this.response.cardRenderer(SKILL_NAME, successful);
        this.response.speak(speechOutput);
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
