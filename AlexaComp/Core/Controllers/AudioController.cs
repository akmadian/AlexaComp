using System;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AlexaComp {
    class AudioController {

        private static CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

        // Play/ Pause, Next and previous track.
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private const int KEYEVENTF_EXTENTEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 0;
        private const int NEXT_TRACK = 0xB0;
        private const int PLAY_PAUSE = 0xB3;
        private const int PREV_TRACK = 0xB1;

        public static void togglePlayPause() {
            keybd_event(PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void prevTrack() {
            keybd_event(PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void nextTrack() {
            keybd_event(NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void setVolume(int percent) {
            defaultPlaybackDevice.Volume = percent;
        }

        public static void volDown() {
            defaultPlaybackDevice.Volume -= 5;
        }

        public static void volUp() {
            defaultPlaybackDevice.Volume += 5;
        }

        public static void toggleMute() {
            // To Implement
        }
    }
}
