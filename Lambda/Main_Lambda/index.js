/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/

'use strict';
console.log('--START--')
const Alexa = require('alexa-sdk');
const config = require('./config.json');
const net = require('net');
const Functions = require('./Functions.js');
console.log('Required Modules Imported')

const SKILL_NAME = 'AlexaComp';
const STOP_MESSAGE = 'Goodbye!';
const HELP_MESSAGE = 'You can ask me to launch a program, tell your computer to ' +
                    'do something, or get some hardware information like CPU temp.'

// Responses
const responsesSuccessful = ['Done!', 'Sent!', 'The request has been sent.'];
const responsesFailed = ['Sorry, that didn\'t work.', 'Something went wrong.', 'I wasn\'t able to complete that request.'];

function makeJson(COMMAND, PRIMARY, SECONDARY = "null", TERTIARY = "null"){
    const auth_key = require('./config.json').SOCKET.AUTH;
    return {"AUTH": auth_key, "COMMAND": COMMAND, "PRIMARY": PRIMARY,
            "SECONDARY": SECONDARY, "TERTIARY": TERTIARY};
}

function SendJson(ip, params){
    var client = new net.Socket();
    console.log('Socket Created')
    var server = new net.Server();
    console.log('Server Created')

    const HOST = config.SOCKET.HOST;
    const PORT = config.SOCKET.PORT;
    const auth_key = config.SOCKET.AUTH;

    client.connect("5406", "73.157.7.156", function() {
        client.write(JSON.stringify({"AUTH": auth_key, "COMMAND": "COMMAND", "PRIMARY": "LOCK", "SECONDARY": "TST", "TERTIARY": "TST2"}));
    });
    client.on('data', function(data){
        var response = JSON.parse(data);
        console.log(response);
        if (data.message == 'devicelinking'){
            writeToS3(params.responseObj.event.context.System.device.deviceId, response.primary);
        }
        params['responseObj'].emit(':tell', response.message);
    });
    client.on('error', function(ex){
        if (ex['code'] == 'ECONNREFUSED'){
            params['responseObj'].emit(':tell', 'Couldn\'t connect to your computer, please be sure alexa comp is running.');
        }
        console.log('Exception Caught: ' + ex);
    });
}

// Code
const handlers = {
    'LaunchProgramIntent': function () {
        console.log('LaunchProgramIntent');
        var programName = this.event.request.intent.slots.ProgramName.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var deviceID = this.event.context.System.device.deviceId;
        var j = makeJson("LAUNCH", programName);
        var options = {
            'sendParams': {
                'responseObj': this,
                'js': j
            }
        }
        // Get IP pair from MDB
        // Send Req
    },

    'GetComputerStatIntent': function () {
        console.log('GetStat Intent');
        var part = this.event.request.intent.slots.Part.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var stat = this.event.request.intent.slots.Stat.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var deviceID = this.event.context.System.device.deviceId;

        var j = makeJson("GETCOMPSTAT", part, stat);
        if (stat.includes('CLOCK')){
            j["TERTIARY"] = "Clock"
        }
        var params = {
            'sendParams':{
                'responseObj': this,
                'js': j
            }
        }
        // Get IP pair from MDB
        // Send req
    },

    'ComputerCommandIntent': function(){
        console.log('Command Intent')
        var command = this.event.request.intent.slots.ComputerCommandConfirm.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var deviceID = this.event.context.System.device.deviceId;

        var j = makeJson("COMPUTERCOMMAND", command);
        var params = {
            'sendParams':{
                'responseObj': this,
                'js': j
            }
        }
        // Get IP pair from MDB
        // Send req
    },

    'DeviceLinkingIntent': function(){
        var deviceID = this.event.context.System.device.deviceId;
        var userID = this.event.session.user.userId;

        Functions.sendEmail('akmadian@gmail.com', deviceID, userID)
    },

    'LaunchRequest' : function(){
        this.emit(':tell', 'Please try again and specify a command')
    },

    'AMAZON.HelpIntent': function () {
        this.emit(':tell', HELP_MESSAGE);
    },

    'AMAZON.CancelIntent': function () {
        this.emit(':tell', STOP_MESSAGE);
    },

    'AMAZON.StopIntent': function () {
        this.emit(':tell', STOP_MESSAGE);
    },
};

exports.handler = function (event, context, callback) {
    const alexa = Alexa.handler(event, context, callback);

    alexa.APP_ID = config.ASK.APP_ID;
    alexa.registerHandlers(handlers);
    alexa.execute();
};
