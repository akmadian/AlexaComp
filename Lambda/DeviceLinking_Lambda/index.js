var crypto = require('crypto');
var config = require('./config.json');
const MongoClient = require('mongodb').MongoClient;

const URI_CONN_STRING = config.MONGODB.URI;

function pushtoMDB(deviceID, userIP){
    MongoClient.connect(URI_CONN_STRING, {useNewUrlParser: true}, function(err, client) {
        if (err){
            console.log(err);
        } else {
            console.log('No error during connection to MongoDB.')
        }
        const collection = client.db('IPTable').collection("IPTable");
        var document = {deviceID:deviceID, userIP:userIP};
        collection.insert(document, {w: 1}, function(err, records){
            if (err){
                console.log(err);
            } else {
                console.log('No error during write to MongoDB.');
            }
        });
        console.log("Connected and wrote to MongoDB successfully.");
        client.close();
    });
}

function decrypt(text){
    if (text == undefined || text == null){
        return text;
    }
    var decipher = crypto.createDecipher('aes-256-cbc',config.ENCRYPTION.KEY)
    var dec = decipher.update(text,'hex','utf8')
    dec += decipher.final('utf8');
    return dec;
}

exports.handler = async (event) => {
    console.log(event);
    const queryParams = event['queryStringParameters'];
    const password = decrypt(queryParams['password']);
    const deviceId = decrypt(queryParams['deviceId']);
    const userIp = event['requestContext']['identity']['sourceIp'];

    console.log(event);

    if (key == undefined){
        console.log("No Password Provided In Request");
        const response = {
            statusCode: 403,
            body: "<p>No password provided in request. <br/>"\
                     "Please contact the maker of AlexaComp at akmadian@gmail.com.</p>"
        };
        console.log('Response Defined')
        return response;
    } else if (key != config.APIPASS){
        console.log("Invalid Password");
        const response = {
            statusCode: 403,
            body: "<p>Invalid password provided in request. <br/>"\
                     "Please contact the maker of AlexaComp at akmadian@gmail.com.</p>"
        };
        console.log('Response Defined')
        return response;
    } else if (key == config.APIPASS){
        pushtoMDB(deviceId, userIp);
        var htmlv2 ="<h1>AlexaComp Device Linking</h1>"\
                    "<p>Your devices have been linked! You may now close this tab.</p>"\
                    "<p>IP - '+ userIp + '</p>"\
                    "<p>Device ID - ' + deviceId + '</p>"\
                    "<p>User ID   - ' + 'n/a' + '</p><br/>"\
                    "<p>CRYPTED</p><br/>"\
                    "<p>Crypted DeviceId - ' + queryParams['deviceId'] + '</p>"\
                    "<p>Crypted UserId   - ' + 'n/a' +'</p>'"
        console.log('HTML Defined')

        const response = {
            statusCode: 200,
            body: event
        };
        console.log('Response Defined')
        return response;
    }
    return event;
};
