var crypto = require('crypto');
var config = require('./config.json');
const MongoClient = require('mongodb').MongoClient;

const URI_CONN_STRING = config.MONGODB.URI;

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

exports.handler = (event) => {
    console.time('GetParams');
    const queryParams = event['queryStringParameters'];
    const password = decrypt(queryParams['password']);
    const deviceId = decrypt(queryParams['deviceId']);
    const key = decrypt(queryParams['alexacomp-api-key']);
    const userIp = event['requestContext']['identity']['sourceIp'];
    console.timeEnd('GetParams');

    console.time('Body');
    if (key == undefined){
        console.log("No Password Provided In Request");
        const response = {
            statusCode: 403,
            body: "<p>No password provided in request. <br/>\
                     Please contact the maker of AlexaComp at akmadian@gmail.com.</p>"
        };
        console.log('Response Defined')
        return response;
    } else if (key != config.APIPASS){
        console.log("Invalid Password");
        const response = {
            statusCode: 403,
            body: "<p>Invalid password provided in request. <br/>\
                     Please contact the maker of AlexaComp at akmadian@gmail.com.</p>"
        };
        console.log('Response Defined')
        return response;
    } else if (key == config.APIPASS){
        console.log('Auth key good')

        console.time('MDB-Write');
        MongoClient.connect(URI_CONN_STRING, {useNewUrlParser: true}, function(err, client) {
            console.log('In MDB Connect.')
            if (err){
                console.log(err);
            } else {
                console.log('No error during connection to MongoDB.')
            }
            const collection = client.db('IPTable').collection("IPTable");
            var document = {deviceID:deviceId, userIP:userIp};
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
        });
        console.timeEnd('MDB-Write');

        var htmlv2 ="<h1>AlexaComp Device Linking</h1>\
                    <p>Your devices have been linked! You may now close this tab.</p>\
                    <p>IP - '+ userIp + '</p>\
                    <p>Device ID - ' + deviceId + '</p>\
                    <p>User ID   - ' + 'n/a' + '</p><br/>\
                    <p>CRYPTED</p><br/>\
                    <p>Crypted DeviceId - ' + queryParams['deviceId'] + '</p>\
                    <p>Crypted UserId   - ' + 'n/a' +'</p>'"
        console.log('HTML Defined')

        const response = {
            statusCode: 200,
            body: event
        };
        console.log('Response Defined')
    }
    console.timeEnd('Body');
    return event;
};
