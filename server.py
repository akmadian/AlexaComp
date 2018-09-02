import socket
import configparser
import json
from os import system

config = configparser.ConfigParser()
config.read('config.ini')

def make_connection():
    host = str(config['SOCKET']['IP'])
    port = int(config['SOCKET']['PORT'])
    s = socket.socket()
    s.bind((host, port))
    print('Bound to port ' + str(port))

    s.listen(1)
    print('Listening')
    c, addr = s.accept()
    print("Connection Made - " + str(addr))
    while True:
        data = c.recv(1024)
        print('RAW - ' + str(data))
        data = dict(json.loads(data))
        print('JSON LOAD - ' + str(data))
        if data['AUTH'] != str(config['AUTH']['AUTH']):
            print('Invalid Auth')
            try:
                s.send(bytes('Invalid Auth', 'utf-8'))
            except OSError as e:
                print('\n\nTried and failed to return invalid auth response \nERROR - ' + str(e))
                break
        else:
            print('Auth Valid')
            if data['TASK']['COMMAND'] == 'LAUNCH':
                if data['TASK']['PROGRAM'] == 'GOOGLECHROME':
                    system('start chrome.exe')
            print('Pre SEND')
            s.send(bytes('SUCCESS', 'utf-8'))
            print('Post SEND')

    c.close()
make_connection()
