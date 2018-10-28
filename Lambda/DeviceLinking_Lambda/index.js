var crypto = require('crypto');
var config = require('./config.json');
var doc = require('dynamodb-doc');
var dynamodb = new doc.DynamoDB();

console.log('v2')

function decrypt(text){
    if (text == undefined || text == null){
        return text;
    }
  var decipher = crypto.createDecipher('aes-256-cbc',config.ENCRYPTION.KEY)
  var dec = decipher.update(text,'hex','utf8')
  dec += decipher.final('utf8');
  return dec;
}

/*
function putToDDB(params){
    console.log('Pushing to DDB')
    var dynamodb = new doc.DynamoDB();
    dynamodb.putItem({
        TableName: "AlexaCompIPTable",
        Item: {
            Pair: {S: "testtesttest"},
            UserIP: {S: "teststring123"},
            testField:{S: "teststring456"}
        }
    },
    function(err, data) {
      console.log('In Update')
      if (err) console.log(err);
      else console.log(data);
    });
}*/

exports.handler = async (event) => {
    console.log(event);
    const queryParams = event['queryStringParameters'];
    const password = decrypt(queryParams['password']);
    const deviceId = decrypt(queryParams['deviceId']);
    const userIp = event['requestContext']['identity']['sourceIp'];

    if (key == undefined){
        console.log("No Password Provided In Request");
        const response = {
            statusCode: 403,
            body: "<p>No password provided in request. <br/>\
                      Please contact the maker of AlexaComp at akmadian@gmail.com.</p>"
        };
        console.log('Response Defined')
        return response;
    } else {
        if (key != config.APIPASS){
            console.log("Invalid Password");
            const response = {
                statusCode: 403,
                body: "<p>Invalid password provided in request. <br/>\
                          Please contact the maker of AlexaComp at akmadian@gmail.com.</p>"
            };
            console.log('Response Defined')
            return response;
        }
    }

    var htmlv2 = '\
                <h1>AlexaComp Device Linking</h1>\
                <p>Your devices have been linked! You may now close this tab.</p>\
                <p>IP - '+ userIp + '</p>\
                <p>Device ID - ' + deviceId + '</p>\
                <p>User ID   - ' + 'n/a' + '</p><br/>\
                <p>CRYPTED</p><br/>\
                <p>Crypted DeviceId - ' + queryParams['deviceId'] + '</p>\
                <p>Crypted UserId   - ' + 'n/a' +'</p>'
    console.log('HTML Defined')


    var params = {
        TableName: config.DYNAMODB.TABLENAME,
        Key: {Pair: 'test123321'},
        UpdateExpression: "SET #IpValue = :IP",
        ExpressionAttributeNames: { "#IpValue" : "UserIp" },
        ExpressionAttributeValues: { ":IP" :  'test456789'},
        ReturnValues: "UPDATED_NEW"
    };

    /*
    const params2 = {
        TableName: "AlexaCompIPTable",
        Item: {
            "Pair": {"S": "testtesttest"},
            "UserI": {"S": "teststring123"},
            "testField":{"S": "teststring456"}
        }
    }*/

    // putToDDB(params);

    console.log('Pushing to DDB')
    dynamodb.updateItem({
        TableName: config.DYNAMODB.TABLENAME,
        Item: {
            "Pair": {"S": "testtesttest"},
            "UserI": {"S": "teststring123"},
            "testField":{"S": "teststring456"}
        }
    ,
    function(err, data) {
      console.log('In Update')
      if (err) console.log(err);
      else console.log(data);
    });

    const response = {
        statusCode: 200,
        body: htmlv2
    };
    console.log('Response Defined')
    return response;
};
