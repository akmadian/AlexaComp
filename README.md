<h1 align="center">AlexaComp</h1>
<br>
AlexaComp is an open source Alexa skill that allows you to interact with your computer using your Alexa device!

## Installation
Installation and setup instructions can be found in the [Setup.md][0] file.

If you would like to help test AlexaComp, you can [email me][5] and request beta access.
I should reply relatively soon with a link to the skill and the next steps to take. 

Please keep in mind that during beta testing, the skill, and all of its components are in active development, 
and certain functionality may stop working at any time. If something stops working, please [submit an issue.][6]

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

You can also help out by trying to fix [issues marked as `help wanted`.][7]

## Privacy
AlexaComp **_does_** collect some information about users. However, it only collects enough to make the skill work.
The data that is gathered is anonymized, and never sold. See the [AlexaComp privacy policy][2] for more information.

___
###### Hardware monitoring package made by [openhardwaremonitor][3]
###### RGB Lighting package made by [RGB.NET][4]

[0]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/Setup.md
[1]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/CompatibilityTool.md
[2]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/Privacy-Policy.md
[3]: https://github.com/openhardwaremonitor/openhardwaremonitor
[4]: https://github.com/DarthAffe/RGB.NET
[5]: https://mail.google.com/mail/u/0/?view=cm&fs=1&tf=1&source=mailto&su=Beta%20Access%20Request&to=akmadian@gmail.com
[6]: https://github.com/akmadian/AlexaComp/issues/new
[7]: https://github.com/akmadian/AlexaComp/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22
