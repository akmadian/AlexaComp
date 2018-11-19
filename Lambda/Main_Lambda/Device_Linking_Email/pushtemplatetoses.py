import subprocess
import os
import time

def pushToLambda():
    print('Pushing To Lambda')
    p = subprocess.Popen("aws ses create-template --cli-input-json file://DeviceLinkingTemplatev8.json",
                         stdout=subprocess.PIPE,
                         shell=True)

    (output, err) = p.communicate()
    p_status = p.wait()
    print("Command Output - \n", output.decode('utf-8'))
    print("Command Exit Status/ Return Code - ", p_status)


startTime = time.time()
print('Pushed to lambda in ' + str(time.time() - startTime)[:6] + ' seconds.')
