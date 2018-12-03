using System;

namespace AlexaComp.Core.Requests {

    public class AudioCommand : AlexaCompCore {
        
        #region Properties
        private string command;

        private int setVolAmt;
        #endregion

        #region Constructors
        public AudioCommand(string command_, string setVolAmt_ = "0") {
            command = command_;
            setVolAmt = Convert.ToInt32(setVolAmt_);

            try{
                process();
            } catch (Exception e) {
                Clog(e.ToString());
                Response res = new Response(false, "Oops! Something went wrong...");
            }
        }
        #endregion

        #region Methods
        public void process() {
            switch (this.command) {
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
            Response res = new Response(true, "Done!");
        }
        #endregion
    }
}