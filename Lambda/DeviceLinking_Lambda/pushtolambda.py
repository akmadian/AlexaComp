import subprocess
import os
import zipfile
import time
from sys import argv



def zipFiles():
    print('Zipping Files')
    filesToZip = ['index.js', 'package-lock.json', 'config.json']
    packageFile = zipfile.ZipFile("package.zip", "w")

    for root, dirs, files in os.walk("node_modules/"):
        for file_ in files:
            filesToZip.append(os.path.join(root, file_))

    for file in filesToZip:
        packageFile.write(file, compress_type=zipfile.ZIP_DEFLATED)

    packageFile.close()
    print('Files Zipped')


def pushToLambda():
    p = subprocess.Popen("aws lambda update-function-code --function-name AlexaCompDeviceLinking --zip-file fileb://" +
                         "D:/Programming/1.GitHub_Repos/AlexaComp/Lambda/DeviceLinking_Lambda/package.zip",
                         stdout=subprocess.PIPE,
                         shell=True)

    (output, err) = p.communicate()
    p_status = p.wait()
    print("Command Output - \n", output.decode('utf-8'))
    print("Command Exit Status/ Return Code - ", p_status)


startTime = time.time()
if argv and '-ziponly' in argv[0]:
    zipFiles()
else:
    zipFiles()
    pushToLambda()
print('Pushed to lambda in ' + str(time.time() - startTime)[:6] + ' seconds.')
