/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/

'use strict';
const config = require('./config.json');
const net = require('net');

const aws = require('aws-sdk');
const s3 = new aws.S3();

var client = new net.Socket();
console.log('Socket Created')
var server = new net.Server();
console.log('Server Created')

const APP_ID = config.ASK.APP_ID;
const HOST = config.SOCKET.HOST;
const PORT = config.SOCKET.PORT;
const auth_key = config.SOCKET.AUTH;

var IPMap = "";
var IPExists = false;

function convertToJson(stringtoconv){
    return JSON.parse(stringtoconv);
}


function SendJson(ip, params){
    client.connect(PORT, config.SOCKET.TESTING, function() {
        var js_ = JSON.stringify(params.js);
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
}

function checkIfInS3(deviceID){
    if (IPMap.hasOwnProperty(deviceID)){
        return true;
    } else {
        return false;
    }
}

function writeToS3(deviceID, IP){
    var jsonStr = '"\"' + deviceID + '":"' + IP + '\""';
    console.log(jsonStr);
    var params = {
        'Bucket': config.S3.BUCKET,
        'Key': 'config.S3.KEY,
        'Body': JSON.stringify(jsonStr)
    };

    s3.putObject(params, function(err, data){
        if (err){
            console.log('Error', err);
        } else {
            console.log('Wrote to S3');
        }
        console.log('inside function');
    });
    console.log('end write');
}


module.exports = {
    'readFromS3': function readFromS3(deviceID, options){
        console.log('In readFromS3');
        var sendParams = options.sendParams || undefined;
        console.log(1);
        var sendResults = options.sendParams || false;
        console.log(2);
        var initalize = options.initialize || false;
        console.log(3);
        
        var params = {
            'Bucket': config.S3.BUCKET,
            'Key': 'config.S3.KEY'
        };
        console.log(4);
    
        s3.getObject(params, function(err, data){
            console.log('object received');
            if (!err){
                console.log(data.Body.toString());
                console.log(5);
                if (initalize){
                    console.log(6);
                    IPMap = data.Body.toString();
                }
                else if (sendResults){
                    console.log(7);
                    var ip = convertToJson(data.Body.toString())[deviceID];
                    console.log(convertToJson(data.Body.toString())[deviceID]);
                    SendJson(ip, sendParams);
                } else {
                    return ip;
                }
            } else {
                console.log("Error ", err);
            }
        });
    },
    
    'writeToS3': function writeToS3(deviceID, IP){
        var jsonStr = '"' + "TESTTEST" + ':' + IP + '"';
        var params = {
            'Bucket': config.S3.BUCKET,
            'Key': config.S3.KEY,
            'Body': JSON.stringify(jsonStr)
        };
    
        s3.putObject(params, function(err, data){
            if (err){
                console.log('Error', err);
            } else {
                console.log('Wrote to S3');
            }
            console.log('inside function');
        });
        console.log('end write');
    },
};
