using System;
using System.Threading;

namespace AlexaComp {
    class TestFunctions {

        /** testAudioFunctions
         * Tests all audio related functions.
         */
        public static void testAudioFunctions() {
            Console.WriteLine("Testing Audio Functions...");

            Console.WriteLine("Testing togglePlayPause");
            Console.WriteLine("    Pausing");
            AudioController.togglePlayPause();
            Thread.Sleep(1300);
            Console.WriteLine("    Playing");
            AudioController.togglePlayPause(); // Restart Music
            
            Console.WriteLine("Testing prevTrack");
            AudioController.prevTrack();
            Thread.Sleep(1300);

            Console.WriteLine("Testing nextTrack");
            AudioController.nextTrack();
            Thread.Sleep(1300);

            Console.WriteLine("Testing volDown");
            AudioController.volDown();
            Thread.Sleep(1300);

            Console.WriteLine("Testing volUp");
            AudioController.volUp();
            Thread.Sleep(1300);

            Console.WriteLine("Testing setVolume");
            Console.WriteLine("    Stepping Up");
            for (int vol = 0; vol <= 100; vol += 10) { // Step Up
                Console.WriteLine("        Step Up - " + vol.ToString());
                AudioController.setVolume(vol);
                Thread.Sleep(500);
            }

            Console.WriteLine("    Stepping Down");
            for (int vol = 100; vol >= 50; vol -= 10) { // Step Down
                Console.WriteLine("        Step Down - " + vol.ToString());
                AudioController.setVolume(vol);
                Thread.Sleep(500);
            }

            Console.WriteLine("Testing toggleMute");
            AudioController.toggleMute(); // Mute
            Thread.Sleep(1300);
            AudioController.toggleMute(); // Unmute
        }

        public static void testCommandFunctions() {
            System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }
    }
}
