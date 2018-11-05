import subprocess
import zipfile
import time
from os import path, walk
from sys import argv

base_path = path.os.path.dirname(path.realpath(argv[0]))

def zipFiles():
    print('Zipping Files')
    filesToZip = ['index.js', 'Functions.js', 'package-lock.json', 'config.json']
    packageFile = zipfile.ZipFile("package.zip", "w")

    for root, dirs, files in walk("node_modules/"):
        for file_ in files:
            filesToZip.append(path.join(root, file_))

    for file in filesToZip:
        packageFile.write(file, compress_type=zipfile.ZIP_DEFLATED)

    packageFile.close()
    print('Files Zipped')


def pushToLambda():
    print('Pushing To Lambda')
    p = subprocess.Popen("aws lambda update-function-code --function-name ComputerCommands --zip-file fileb://" + base_path +"/package.zip",
                         stdout=subprocess.PIPE,
                         shell=True)

    (output, err) = p.communicate()
    p_status = p.wait()
    print("Command Output - \n", output.decode('utf-8'))
    print("Command Exit Status/ Return Code -", p_status, "OK" if p_status == 0 else "FAILED")


startTime = time.time()
if argv and '-ziponly' in argv:
    zipFiles()
else:
    zipFiles()
    pushToLambda()
print('Completed in ' + str(time.time() - startTime)[:6] + ' seconds.')
