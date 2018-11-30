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
                clog(e.ToString());
                Response res = new Response(false, "Oops! Something went wrong...");
            }
        }
        #endregion

        #region Methods
        public void process() {
            switch (this.command) {
                case "PLAYPAUSE":  
                    AudioController.togglePlayPause();                         
                    break;

                case "NEXTTRACK":  
                    AudioController.nextTrack();                               
                    break;

                case "PREVTRACK":  
                    AudioController.prevTrack();                               
                    break;

                case "SETVOLUME":  
                    AudioController.setVolume(this.setVolAmt); 
                    break;

                case "VOLUMEUP":   
                    AudioController.volUp();                                   
                    break;

                case "VOLUMEDOWN": 
                    AudioController.volDown();                                 
                    break;

                case "TOGGLEMUTE": 
                    break; // TODO: Implement Mute Functionality
            }
            Response res = new Response(true, "Done!");
        }
        #endregion
    }
}