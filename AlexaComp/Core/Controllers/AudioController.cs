using System;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AlexaComp {
    class AudioController : AlexaCompCore {

        private static CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

        // Play/ Pause, Next and previous track.
        [DllImport("user32.dll")]
        public static extern void Keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private const int KEYEVENTF_EXTENTEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 0;
        private const int NEXT_TRACK = 0xB0;
        private const int PLAY_PAUSE = 0xB3;
        private const int PREV_TRACK = 0xB1;

        public static void initController(){
            defaultPlaybackDevice.Mute(true);
        }

        public static void TogglePlayPause() {
            Keybd_event(PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void PrevTrack() {
            Keybd_event(PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void NextTrack() {
            Keybd_event(NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void SetVolume(int percent) {
            defaultPlaybackDevice.Volume = percent;
        }

        public static void VolDown() {
            defaultPlaybackDevice.Volume -= 5;
        }

        public static void VolUp() {
            defaultPlaybackDevice.Volume += 5;
        }

        public static void ToggleMute() {
           if (defaultPlaybackDevice.IsMuted) {
               defaultPlaybackDevice.Mute(false); // Unmute
           } else {
               defaultPlaybackDevice.Mute(true); // Mute
           }
        }
    }
}
