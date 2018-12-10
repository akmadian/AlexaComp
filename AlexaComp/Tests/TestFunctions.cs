using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AlexaComp.Controllers;

namespace AlexaComp {
    class TestFunctions : AlexaCompCore{

        public static List<string> failedTests = new List<string>();
        public static int failedTestsCount = 0;

        public static void addFailedTest(string testName) {
            failedTestsCount++;
            failedTests.Add(testName);
        }

        public static void testAllFunctions() {

        }

        public static void testRGBEffects() {
            Clog("Testing Error Effect");
            LightingController.LightingEffects.ErrorEffect();
        }

        public static void testColorConversions() {
            Clog("Testing Color Value Conversion Methods...");
            // RGB to Hex
            if (ColorMethods.RGBToHex(new RGBColor(255, 0, 255)) == "FF00FF") {
                Clog("    RGB To Hex Conversion Passed");
            } else {
                addFailedTest("RGB To Hex Conversion");
            }

            // Integer Array to RGB Object
            int[] intArr = new int[] {255, 0, 255};
            if (ColorMethods.ArrToRGB(intArr).Equals(new RGBColor(255, 0, 255))) {
                Clog("    Integer Array to RGB Object Conversion Passed");
            } else {
                addFailedTest("Int Array to RGB");
            }

            // Hex String to RGB Object without #
            if (ColorMethods.HexToRGB("FF00FF").Equals(new RGBColor(255, 0, 255))) {
                Clog("    Hex to RGB Object ( w/o # ) Conversion Passed");
            } else {
                addFailedTest("Hex to RGB Object ( w/o # )");
            }

            // Hex String to RGB Object with #
            if (ColorMethods.HexToRGB("#FF00FF").Equals(new RGBColor(255, 0, 255))) {
                Clog("    Hex to RGB Object ( w/ # ) Conversion Passed");
            }
            else {
                addFailedTest("Hex to RGB Object ( w/ # )");
            }

            // TODO : Implement HSLToRGB test.
        }

        /** testAudioFunctions
         * Tests all audio related functions.
         */
        public static void testAudioFunctions() {
            Console.WriteLine("Testing Audio Functions...");

            Console.WriteLine("Testing togglePlayPause");
            Console.WriteLine("    Pausing");
            AudioController.TogglePlayPause();
            Thread.Sleep(1300);
            Console.WriteLine("    Playing");
            AudioController.TogglePlayPause(); // Restart Music
            
            Console.WriteLine("Testing prevTrack");
            AudioController.PrevTrack();
            Thread.Sleep(1300);

            Console.WriteLine("Testing nextTrack");
            AudioController.NextTrack();
            Thread.Sleep(1300);

            Console.WriteLine("Testing volDown");
            AudioController.VolDown();
            Thread.Sleep(1300);

            Console.WriteLine("Testing volUp");
            AudioController.VolUp();
            Thread.Sleep(1300);

            Console.WriteLine("Testing setVolume");
            Console.WriteLine("    Stepping Up");
            for (int vol = 0; vol <= 100; vol += 10) { // Step Up
                Console.WriteLine("        Step Up - " + vol.ToString());
                AudioController.SetVolume(vol);
                Thread.Sleep(500);
            }

            Console.WriteLine("    Stepping Down");
            for (int vol = 100; vol >= 50; vol -= 10) { // Step Down
                Console.WriteLine("        Step Down - " + vol.ToString());
                AudioController.SetVolume(vol);
                Thread.Sleep(500);
            }

            Console.WriteLine("Testing toggleMute");
            AudioController.ToggleMute(); // Mute
            Thread.Sleep(1300);
            AudioController.ToggleMute(); // Unmute
        }

        public static void testCommandFunctions() {
            System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }
    }
}
