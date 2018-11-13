<h1 align="center">Compatibility Tool</h1>
<br/>
<br/>

If you would like to help contribute to AlexaComp without writing any code, a great way to do this is by running the Compatibility Tool.

## What Is The Compatibility Tool?
When interacting with the RGB lighting and sensors of various pieces of hardware, there can be edge cases.
<br>Different hardware may have different sensors or RGB lights available, and it helps to know about what may or may not be available to any given user.

The tool gathers information about any RGB enabled devices connected to your system, as well as information about your computer's hardware and sensors.
Any and all hardware information is retrieved via openhardwaremonitor, and RGB lighting information is retrieved via [RGB.NET](https://github.com/DarthAffe/RGB.NET)

For hardware sensors, no more information is available to the tool at any time than is available to software like openhardwaremonitor, CPU-Z, GPU-Z, etc.
<br>For RGB, no more information is available to the tool at any time than is available to proprietary lighting software like ASUS Aura Sync, MSI Mystic Light, Corsair iCue, etc.


## How do I use the tool?
Q: Why do I need to link my Amazon account to use AlexaComp?<br/>
A: Account linking is used only to get the user's email address. The email is only retreived and used when the user tells AlexaComp to start device linking.

## Planned Features
#### Launching various applications <br/>
ex. "Alexa, tell my computer to launch google chrome"

#### Retrieving Hardware Info <br/>
ex. "Alexa, ask my computer what my gpu temp is"

#### Issuing System Commands <br/>
ex. "Alexa, tell my computer to shut down"

<br/>

___
###### Hardware monitoring code made by [openhardwaremonitor](https://github.com/openhardwaremonitor/openhardwaremonitor)