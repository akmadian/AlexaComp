import smtplib
import configparser

from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText

config = configparser.ConfigParser()
config.read_file(open('config.ini'))


def getHtmlFromFile():
    with open('email.html', 'r') as htmlFile:
        data = htmlFile.read().replace('\n', '')
    return data


def sendEmail():
    msg = MIMEMultipart('alternative')
    msg['subject'] = 'AlexaComp Device Linking'
    msg['From'] = config['fromEmail']
    msg['To'] = config['fromEmail']


