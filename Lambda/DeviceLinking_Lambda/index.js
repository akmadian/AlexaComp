var nodemailer = require('nodemailer');

exports.handler = async (event) => {
    // TODO implement
    const queryParams = event['queryStringParameters'];
    var userId = queryParams['userId']
    var deviceId = queryParams['deviceId']
    var userIp = event['requestContext']['identity']['sourceIp']
    const response = {
        statusCode: 200,
        body: JSON.stringify(userIp + deviceId + userId)
    };
    return response;
};
