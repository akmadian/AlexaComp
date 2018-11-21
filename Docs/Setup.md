<h1 align="center">Setup</h1>

This document outlines the process of setting up AlexaComp on your computer.

The AlexaComp setup process has three parts:

 - Adding the skill to your Alexa device.
 - Installing the client software on your computer.
 - Linking your Alexa device(s) and your computer.

___

### Adding The Skill to Your Device

The skill model is currently not publicly available yet. Work is still needed in the Main Lambda server before the model is submitted for certification again.
<br>If you want, you can help out by forking working on the main lambda and creating a pull request. You can find a To Do list for the main lambda [here.](https://github.com/akmadian/AlexaComp/projects/5)
<hr>

### Installing The Client Software

The client software installer is called `AlexaCompSetup.msi`, and can be found at the [AlexaComp releases page.](https://github.com/akmadian/AlexaComp/releases)
<br>All you need to do is download and install the AlexaComp client, and run it! Be sure to complete device linking before making any requests.
<hr>

### Linking Your Computer, and Alexa Device

Device linking is a process meant to link your computer's IP address to your Alexa device's unique ID. This is necessary to determine what computer to send the commands to.
<br>This means that if you want to be able to make requests to your computer from multiple devices, you will have to link each of those devices to your computer individually.

To start device linking, say to the Alexa device you would like to link to your computer: "Alexa, tell my computer to start device linking."
<br>You should then receive an email at the email address you used to register your Amazon account. In that email will be a link.
<br>All you need to do is click that link, and you're done!
<pre>
    Q: Why do I need to link my Amazon account to the skill?
    A: Account Linking is used solely to retreive your email address.
          Your email address is:
           - Only used in the device linking process.
           - Only retrieved when you start the device linking process.

    Q: How does device linking work?
    A: The purpose of device linking is to create a link between your Alexa device(s) unique ID, and your computer's public IP address.
          When you tell your Alexa device to start device linking, the <a href="https://github.com/akmadian/AlexaComp/tree/lambda-main/Lambda/Main_Lambda">main lambda</a> gets your Alexa device's ID from the request,
          and the email address you used to register your Amazon account with, and sends you an email containing a link that leads to
          the <a href="https://github.com/akmadian/AlexaComp/tree/lambda-deviceLinking/Lambda/DeviceLinking_Lambda">device linking lambda</a>, which then retreives the IP of your computer from the HTTP request. The device linking lambda
          then writes a key-value pair of your Alexa device's ID, and your computer's IP to a MongoDB database.

          Whenever a request is made to the skill, the main lambda will look for your Alexa device's ID in the database
          if it is found, the main lambda can then send the request to your computer.

    You can see the code behind device linking in the DeviceLinking intent in index.js of the main lambda <a href="https://github.com/akmadian/AlexaComp/blob/lambda-main/Lambda/Main_Lambda/index.js#L79">here.</a>
    As well as in the index.js file of the DeviceLinking lambda server <a href="https://github.com/akmadian/AlexaComp/blob/lambda-deviceLinking/Lambda/DeviceLinking_Lambda/index.js">here.</a>
</pre>

###### Hardware monitoring code made by [openhardwaremonitor](https://github.com/openhardwaremonitor/openhardwaremonitor)
