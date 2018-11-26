import subprocess
import time
from os import path
from sys import argv

def build():
    print('Building...')
    p = subprocess.Popen("dotnet build AlexaComp.csproj", stdout = subprocess.PIPE, shell = True)

    (output, err) = p.communicate()
    p_status = p.wait()
    print("Command Output - \n", output.decode('utf-8'))
    print("Command Exit Status/ Return Code - ", p_status, "OK" if p_status == 0 else "FAILED")
    return p_status

def run():
    print("Running")
    p = subprocess.Popen("start bin/Debug/AlexaComp.exe", stdout = subprocess.PIPE, shell = True)

    (output, err) = p.communicate()
    p_status = p.wait()
    print("Command Output - \n", output.decode('utf-8'))
    print("Command Exit Status/ Return Code - ", p_status, "OK" if p_status == 0 else "FAILED")
                

startTime = time.time()
if argv and '-buildonly' in argv:
    build()
else:
    if build() == 0: 
        run()
print('Completed in ' + str(time.time() - startTime)[:6] + ' seconds.')
