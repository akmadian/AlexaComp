<h1 align="center">Commands</h1>

This file contains a comprehensive list of all of the commands, in all of their forms, that you can give AlexaComp. These commands may or may not work depending on what development work is happening.

Please precede all commands listed with your device's wake word (The default is `Alexa`), and AlexaComp's invocation phrase (`my computer`). This means that you should precede all commands with "Alexa, [tell, open] my computer [command]"

Any parts in commands that look like `this` are what are called "slots". Slots can take any one value from a list of options. The list of possible values will be documented under the commands, along with any synonyms.

Some of the command's implementations in the skill model are not very good, and some combinations of commands and slots will not work, but others may. If you are trying a specific combination and something isn't working properly, try modifying which command you are trying the slots in. Command categories known to have issues are marked with a `*`.

To see active issues with the skill model, check [issues marked with the `skill-model` tag][2] in the [AlexaComp repository][4].

If you discover an issue, please [submit an issue][5]. Include information about which command you are attempting to use, as well as any slot values. Pleas also include information about what exactly is happening when you make the request (does the device say anything back? does anything happen on your computer after you make the request? etc.)

## Launching various applications
- Launch Applications like Google Chrome, Spotify, etc.
    Most of these commands require some configuration to launch. Only Google Chrome and Steam work without configuration. Config automation is planned.

    <details><summary>Commands</summary>
    <p>
    <br>
        start up <code>ProgramName</code> <br>
        start <code>ProgramName</code> <br>
        run <code>ProgramName</code> <br>
        launch <code>ProgramName</code> <br>
    </p>
    </details>

    <details><summary>Slots</summary>
    <details><summary>&nbsp;&nbsp;&nbsp;&nbsp;ProgramName</summary>
    <pre>
        Python Shell
            Synonyms - python, python IDLE
        Minecraft
        UPLAY
            Synonyms - Ubisoft uplay
        Battle.Net
            Synonyms - Blizzard launcher, battle net, battle dot net
        Origin Launcher
            Synonyms - Origin
        Windows Settings
        Discord
        Command Prompt
            Synonyms - CMD, C M D
        Task Manager
        iTunes
            Synonyms - Apple i Tunes, Apple iTunes
        Spotify
        CAM
            Synonyms - NZXT CAM
        JetBrains Pycharm
            Synonyms - pie charm, jet brains pie charm
        ATOM
        Brackets
        JetBrains IntelliJ Idea
            Synonyms - intellij idea, in telli j idea, intellij, in telli j
        EVGA Precision XOC
            Synonyms - evga precision x o c, precision x o c
        Visual Studio
        Visual Studio Code
            Synonyms - vs code
        Steam
    </pre>
    </details>
    </details>

## Retrieving Hardware Info
Hardware sensors are gathered by the [OpenHardwareMonitor SDK][0]. The OHM SDK is outdated and may not detect ALL sensors available to other monitoring systems. I am looking into alternatives.

- Get Temperature/ Load/ Clock Speed of a Given Part

    <details><summary>Commands</summary>
    <p>
    <br>
        to get my <code>Part</code> <code>Stat</code> <br>
        what's the <code>Stat</code> of my <code>Part</code><br>
        what's my <code>Part</code> <code>Stat</code> <br>
        get the <code>Stat</code> of my <code>Part</code> <br>
        get my <code>Part</code> <code>Stat</code> <br>
        what is the <code>Stat</code> of my <code>Part</code>
    </p>
    </details>

    <details><summary>Slots</summary>
    <details><summary>&nbsp;&nbsp;&nbsp;&nbsp;Part</summary>
    <pre>
        RAM
            Synonyms - memory
        CPU
        GPU
            Synonyms - graphics card
    </pre>
    </details>
    <details><summary>&nbsp;&nbsp;&nbsp;&nbsp;Stat</summary>
    <pre>
        RAM Clock
            Synonyms - memory clock
        Core Clock
        Load
        Available
        Usage
        Temperature
            Synonyms - temp
    </pre>
    </details>
    </details>

## Issuing System Commands
Be aware that some specific combinations of slot values and commands may not work, as the Alexa language processor thinks that it is a command to the Alexa skill itself, and not part of an inent.

<details><summary>Commands</summary>
<p>
<br>
    and <code>Command</code> it <br>
    and tell it to <code>Command</code> <br>
    to <code>Command</code> <br>
    tell it to <code>Command</code> <br>
    and tell my computer to <code>Command</code> <br>
    my computer to <code>Command</code>
    tell my computer to <code>Command</code> <br>
</p>
</details>
<details><summary>Slots</summary>
<details><summary>&nbsp;&nbsp;&nbsp;&nbsp;Command</summary>
<pre>
    Log Off
        Synonyms - logoff, logout, log out
    Lock
        Synonyms - lock down, secure itself, lock itself
    Restart
    Shut Down
        Synonyms - turn itself off, turn off
    Sleep
        Synonyms - go to sleep
</pre>
</details>
</details>

## Audio Control *
This command's implementation in the skill model is kinda wonky, and some things will probably not work as intended.
- Play/Pause
- Previous/ Next Track
- Increment/ Set Volume (to percent)

<details><summary>Commands</summary>
<p>
<br>
    to <code>AudioCommand</code> <br>
    turn my <code>AudioCommand</code> <br>
    to turn my <code>AudioCommand</code> <br>
    to <code>AudioCommand</code> my music <br>
    turn my music <code>AudioCommand</code> <br>
    tell my music to <code>AudioCommand</code>
</p>
</details>

<details><summary>Slots</summary>
<details><summary>&nbsp;&nbsp;&nbsp;&nbsp;AudioCommand</summary>
<pre>
    *Sets volume as percentage*
    Set Volume
        Synonyms - change volume, change my volume
    Pause
        Synonyms - pause my music, play my music, pause music, play music, play
    *Volume Up/ Down increments and decrements volume by 5 percent.*
    Volume Down
        Synonyms - turn the volume down, turn volume down
    Volume Up
        Synonyms - turn the volume up, turn volume up
    Previous Song
        Synonyms - last song
    Next Song
    Mute / Unmute (toggles mute regardless of which one is given)
</pre>
</details>
</details>

## RGB Lighting Control *
RGB Control is based on [DarthAffe's RGB.NET][1] , `RGB.NET` is still in active development, as are the proprietary SDKs it is based on. This means that AlexaComp's RGB system may stop working for any manufacturer or device, at any time.

Most of the effects will default to white, but some effects aren't working properly.

<details><summary>Commands</summary>
<p>
<br>
    to set my lighting to <code>Color</code> <br>
    to set my lighting to an <code>Effect</code> <br>
    to set my lighting to a <code>Effect</code> <br>
    to set my RGB to an <code>Effect</code> <br>
    to set my RGB to a <code>Effect</code> <br>
    to set my lights to an <code>Effect</code> <br>
    to set my lights to a <code>Effect</code> <br>
    to set my lighting to an <code>Effect</code> <code>Color</code><br>
    to set my lighting to a <code>Effect</code> <code>Color</code><br>
    to set my RGB to an <code>Effect</code> <code>Color</code><br>
    to set my RGB to a <code>Effect</code> <code>Color</code><br>
    to set my lights to an <code>Effect</code> <code>Color</code><br>
    to set my lights to a <code>Effect</code> <code>Color</code><br>
</p>
</details>

<details><summary>Slots</summary>
<details><summary>&nbsp;&nbsp;&nbsp;&nbsp;Effect</summary>
<pre>
    Off
        Synonyms - LED off, all off
    Error
        Synonyms - error effect
    Alternating
        Synonyms - alternating effect
    Pulsing
        Synonyms - pulsing effect, pulsating, pulsating effect
    Breathing
        Synonyms - breathing effect
    Static Color
        Synonyms - steady, static, static color effect
    Rainbow Fade
        Synonyms - rainbow fading effect, rainbow fade effect, rainbow fading, rainbow
</pre>
</details>
<details><summary>&nbsp;&nbsp;&nbsp;&nbsp;Color</summary>
    See the <a href="https://github.com/AlexaComp/AlexaComp/blob/docs/Docs/Colors.md">Color Documentation</a> for all supported colors, their names, and a small swatch.
</details>
</details>

[0]: https://github.com/openhardwaremonitor/openhardwaremonitor
[1]: https://github.com/DarthAffe/RGB.NET
[2]: https://github.com/AlexaComp/AlexaComp/labels/skill-model
[3]: https://github.com/AlexaComp/AlexaComp/blob/docs/Docs/Colors.md
[4]: https://github.com/AlexaComp/AlexaComp
[5]: https://github.com/AlexaComp/AlexaComp/issues/new
