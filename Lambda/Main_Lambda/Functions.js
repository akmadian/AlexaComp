/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/

'use strict';
const config = require('./config.json');
const net = require('net');
const nodemailer = require('nodemailer');
const crypto = require('crypto');
const fs = require('fs');

const aws = require('aws-sdk');

const APP_ID = config.ASK.APP_ID;

function convertToJson(stringtoconv){
    return JSON.parse(stringtoconv);
}

function encrypt(text){
    var cipher = crypto.createCipher('aes-256-cbc',config.ENCRYPTION.KEY)
    var crypted = cipher.update(text,'utf8','hex')
    crypted += cipher.final('hex');
    return crypted;
}

/*
function SendJson(ip, params){
    var client = new net.Socket();
    console.log('Socket Created')
    var server = new net.Server();
    console.log('Server Created')

    const HOST = config.SOCKET.HOST;
    const PORT = config.SOCKET.PORT;
    const auth_key = config.SOCKET.AUTH;

    client.connect(PORT, HOST, function() {
        var js_ = params;
        client.write(js_);
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
}*/

module.exports = {
    'sendEmail': function sendEmail(toAddr, deviceID, userID){
        aws.config.update({region: 'us-west-2'})
        const ses = new aws.SES();

        console.log('Sending email to user - ' + deviceID + ' - ' + userID)
        var eParams = {}
        var paramString = require('util').format('{\"DEVICEIDHERE\":\"%s\", \"ALEXACOMPKEY\": \"%s\", \"APIKEY\":\"%s\"}', encrypt(deviceID), encrypt(config.ENCRYPTION.APIPASS), config.SES.APIKEY);

        fs.readFile('Device_Linking_Email_email.html', 'utf8', function(err, data_){
          console.log('in read html')
          eParams = {
              Destinatin: {
                ToAddresses: ["akmadian@gmail.com"]
              },
              Template: "DeviceLinkingTemplatev9_1_1",
              ConfigurationSetName: "AlexaCompSES",
              Source: "alexacompdevicelinking@gmail.com",
              TemplateData: paramString
          };

            console.log('eparams defined')
            console.log('===SENDING EMAIL===');
            var email = ses.sendTemplatedEmail(eParams, function(err, data){
                if(err) console.log(err);
                else {
                    console.log("===EMAIL SENT===");
                    console.log(data)
                }
            });
        console.log("EMAIL CODE END");
        });
        console.log(eParams)

    },
};
