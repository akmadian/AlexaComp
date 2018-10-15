/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/

'use strict';
console.log('--START--')
const Alexa = require('alexa-sdk');
const config = require('./config.json');
const Functions = require('./Functions.js');
console.log('Required Modules Imported')

Functions.sendEmail('', 'test123', 'test456')


// Functions.readFromS3("", {'initialize': true});
// console.log(Functions.IPMap);

const SKILL_NAME = 'AlexaComp';
const STOP_MESSAGE = 'Goodbye!';
const HELP_MESSAGE = 'You can ask me to launch a program, tell your computer to ' +
                    'do something, or get some hardware information like CPU temp.'

// Responses
const responsesSuccessful = ['Done!', 'Sent!', 'The request has been sent.'];
const responsesFailed = ['Sorry, that didn\'t work.', 'Something went wrong.', 'I wasn\t able to complete that request.'];


function makeId() {
  var text = "";
  var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

  for (var i = 0; i < 4; i++)
    text += possible.charAt(Math.floor(Math.random() * possible.length));

  return text;
}

function makeJson(COMMAND, PRIMARY, SECONDARY = "null", TERTIARY = "null"){
    const auth_key = require('./config.json').SOCKET.AUTH;
    return {"AUTH": auth_key, "COMMAND": COMMAND, "PRIMARY": PRIMARY,
            "SECONDARY": SECONDARY, "TERTIARY": TERTIARY};
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
        Functions.readFromS3(deviceID, options);
    },

    'GetComputerStatIntent': function () {
        console.log('GetStat Intent');
        var part = this.event.request.intent.slots.Part.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var stat = this.event.request.intent.slots.Stat.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var deviceID = this.event.context.System.device.deviceId;
        Functions.readFromS3(deviceID);

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
        Functions.readFromS3(deviceID, params);
    },

    'ComputerCommandIntent': function(){
        console.log('Command Intent')
        var command = this.event.request.intent.slots.ProgramName.resolutions.resolutionsPerAuthority[0].values[0].value.id;
        var deviceID = this.event.context.System.device.deviceId;
        Functions.readFromS3(deviceID);

        var j = makeJson("COMPUTERCOMMAND", command);
        var params = {
            'sendParams':{
                'responseObj': this,
                'js': j
            }
        }
        Functions.readFromS3(deviceID, params);
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
