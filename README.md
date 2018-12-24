<h1 align="center">AlexaComp</h1>
<br>

AlexaComp is an open source Alexa skill that allows you to interact with your computer using your Alexa device!

For a comprehensive list of all the commands you can give to AlexaComp, take a look at [Commands.md][8].

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
         The email is only retreived and used when the user tells 
         AlexaComp to start device linking.
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
<br>If you would like to help with ongoing testing of features, and/ or the skill model, please join [the AlexaComp discord server!][10]
<br>Forks and pull requests are welcome! No real format guidelines, just fork, make changes, and submit a PR detailing changes made :)

You can also help out by trying to fix [issues marked as `help wanted`.][7]

If you are going to submit an issue, please include any and all relevant log files either as uploads, or links to a service like pastebin. Log files can be found by default in `C:\Program Files(x86)\AlexaComp\Logs`

## Privacy
AlexaComp **_does_** collect some information about users. However, it only collects enough to make the skill work.
The data that is gathered is anonymized, and never sold. See the [AlexaComp privacy policy][2] for more information.

##### Unstable Client Builds
If you want to see the latest builds, check [this Google Drive folder][9].
<br>There are a couple suffixes after the version number you should be aware of.
 - If there is a `WA`, that means that that installer should work with ASUS AuraSync devices.
 - If there is a `T`, that means that that installer will not open the gui, and will close when a key is pressed after loading.
 - If there is a `C`, that means that that installer will open with a console window, and the GUI. No `C` is just GUI.
___
###### Hardware monitoring package made by [openhardwaremonitor][3]
###### RGB Lighting package made by [RGB.NET][4]

###### </> With &#9829; by Ari Madian

[0]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/Setup.md
[1]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/CompatibilityTool.md
[2]: https://github.com/akmadian/AlexaComp/blob/docs/Docs/Privacy-Policy.md
[3]: https://github.com/openhardwaremonitor/openhardwaremonitor
[4]: https://github.com/DarthAffe/RGB.NET
[5]: https://mail.google.com/mail/u/0/?view=cm&fs=1&tf=1&source=mailto&su=Beta%20Access%20Request&to=akmadian@gmail.com
[6]: https://github.com/akmadian/AlexaComp/issues/new
[7]: https://github.com/akmadian/AlexaComp/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22
[8]: https://github.com/AlexaComp/AlexaComp/blob/docs/Docs/Commands.md
[9]: https://drive.google.com/drive/folders/1QpTgOJzYC0yAiqVobTtKqjMkA0yW8cnL?usp=sharing
[10]: https://discord.gg/rRvgfbX
