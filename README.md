<h1 align="center">AlexaComp</h1>
<br>

AlexaComp is an open source Alexa skill that allows you to interact with your computer using your Alexa device!

## Installation
Installation and setup instructions can be found in the [Setup.md][0] file.

<pre>
    <b>Account Linking</b>
    Q: Why do I need to link my Amazon account to use AlexaComp?
    A: Account linking is used only to get the user's email address. 
         The email is only retreived and used when the user tells AlexaComp to start device linking.
</pre>

## Planned Features
##### Launching various applications
- Launch Applications like Google Chrome, Spotify, etc.

##### Retrieving Hardware Info
- Get Temperature/ Load/ Clock Speed of a Given Part

##### Issuing System Commands
- Shut Down/ Restart/ Sleep/ Hibernate
- Lock
- Log Out

##### Audio Control
- Play/Pause
- Previous/ Next Track
- Increment/ Set Volume (to percent)

##### RGB Lighting Control
- Set All/ Individual Parts To Color
- Set All/ Individual Parts To Effect

###### Google Home support might be on the way, I'm not sure yet...

<br>

## Contributing
A great way to contribute without spending much time or writing any code is to help hardware compatibility by running the [compatibility tool][1] on your computer.
<br>Forks and pull requests are welcome! No real format guidelines, just fork, make changes, and submit a PR detailing changes made :)

## Privacy
AlexaComp **_does_** collect some information about users. However, it only collects enough to make the skill work.
The data that is gathered is anonymized, and never sold. See the [AlexaComp privacy policy][2] for more information.

___
###### Hardware monitoring package made by [openhardwaremonitor][5]
###### RGB Lighting package made by [RGB.NET][4]

[0]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/Setup.md
[1]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/CompatibilityTool.md
[2]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/Privacy-Policy.md
[3]: https://github.com/openhardwaremonitor/openhardwaremonitor
[4]: https://github.com/DarthAffe/RGB.NET
