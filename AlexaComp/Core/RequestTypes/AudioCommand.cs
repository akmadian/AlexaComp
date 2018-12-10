using System;

namespace AlexaComp.Core.Requests {

    public class AudioCommand : AlexaCompCore {
        
        #region Properties
        private string command;
        private int setVolAmt;

        #endregion

        #region Constructors
        public AudioCommand(string command, string setVolAmt = "0") {
            this.command = command;
            this.setVolAmt = Convert.ToInt32(setVolAmt);

            try{
                process();
            } catch (Exception e) {
                Clog(e.ToString());
            }
        }
        #endregion

        #region Methods
        public void process() {
            switch (command) {
                case "PLAYPAUSE":  
                    AudioController.TogglePlayPause();                         
                    break;

                case "NEXTTRACK":  
                    AudioController.NextTrack();                               
                    break;

                case "PREVTRACK":  
                    AudioController.PrevTrack();                               
                    break;

                case "SETVOLUME":  
                    AudioController.SetVolume(this.setVolAmt); 
                    break;

                case "VOLUMEUP":   
                    AudioController.VolUp();                                   
                    break;

                case "VOLUMEDOWN": 
                    AudioController.VolDown();                                 
                    break;

                case "TOGGLEMUTE": 
                    AudioController.ToggleMute();
                    break;
            }
        }
        #endregion
    }
}