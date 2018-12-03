const crypto = require('crypto');
const config = require('./config.json');
const MongoClient = require('mongodb').MongoClient;

const URI_CONN_STRING = config.MONGODB.URI;
const sessID = makeSessionId();
console.log(sessID);

function decrypt(text){
    console.time('Decrypt');
    if (text == undefined || text == null){
        return text;
    }
    var decipher = crypto.createDecipher('aes-256-cbc',config.ENCRYPTION.KEY)
    var dec = decipher.update(text,'hex','utf8')
    dec += decipher.final('utf8');
    console.timeEnd('Decrypt');
    return dec;
}

function makeSessionId() {
    var text = "";
    const possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 8; i++){
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }

    return text;
}

function responseString(message, err){
    var response = "Device Linking Unsuccessful -- " + message +
                   " Please contact the maker of AlexaComp about this at akmadian@gmail.com -- Session ID: " + sessID +
                   " In your email, please provide the session ID and the following error message, otherwise we won't be able to resolve your issue."
    if (err != undefined && err != null){
        response += " ==Begin Error== " + err + "==End Error=="
    }
    return JSON.stringify(response);
}

function pushToMDB(deviceID, userIP){
    return new Promise(function(resolve, reject){
        console.time('MDB-Write');
        console.time('MDB-Connect');
        console.log('In MDB Write');
        MongoClient.connect(URI_CONN_STRING, {useNewUrlParser: true}, function(err, client) {
            console.log('In MDB Connect.');
            if (err){
                console.log(err);
            } else {
                console.log('No error during connection to MongoDB.')
            }
            console.timeEnd('MDB-Connect');
            const collection = client.db('IPTable').collection("IPTable");
            var document = {deviceID:deviceID, userIP:userIP, sessionID:sessID};
            collection.insert(document, {w: 1}, function(err, records){
                console.log('In MDB Insert.');
                if (err){
                    console.log(err);
                } else {
                    console.log('No error during write to MongoDB.');
                }
            });
            console.log("Connected and wrote to MongoDB successfully.");
            client.close();
            console.timeEnd('MDB-Write');
            resolve();
        });
    });
}

exports.handler = async (event, context) => {
    console.time('LogContext')
    console.log("==BEGIN EVENT==")
    console.log(event);
    console.log("==END EVENT==")
    console.log("==BEGIN CONTEXT==")
    console.log(context);
    console.log("==END CONTEXT==")
    console.time('LogContext')

    const response = {
        statusCode: 500,
        body: JSON.stringify("Default Body")
    };

    console.time('GetParams');
    var queryParams;
    var deviceId;
    var key;
    var userIp;

    try {
        queryParams = event['queryStringParameters'];
    } catch (err) {
        console.log("Error occurred when defining queryParams");
        console.log(err);

        response.body = responseString("Event or queryParams undefined.", err);
        context.succeed(response);
    }
    try {
        deviceId = decrypt(queryParams['deviceId']);
        key = decrypt(queryParams['alexacomp-api-key']);
        userIp = event['requestContext']['identity']['sourceIp'];
    } catch (err) {
        console.log("Error occurred when defining param");
        console.log(err);

        response.body = responseString("Error occurred when defining queryParams", err);
        context.succeed(response);
    }
    console.timeEnd('GetParams');

    console.time('Body');
    if (key == undefined) {
        console.log("No Password Provided In Request");
        response.statusCode = 403;
        response.body = responseString("No auth key provided in request");

        context.succeed(response);
    } else if (key != config.APIPASS){
        console.log("Invalid Password");
        response.statusCode = 403;
        response.body = responseString("Invalid auth key provided in request.")

        context.succeed(respnse);
    } else if (key == config.APIPASS){
        console.log('Auth key good')

        await pushToMDB(deviceId, userIp);

        response.statusCode = 200;
        response.body = JSON.stringify("Device Linking completed successfully. You can now close this tab. Session ID: " + sessID)

        context.succeed(response);
    }
    console.timeEnd('Body');
};
