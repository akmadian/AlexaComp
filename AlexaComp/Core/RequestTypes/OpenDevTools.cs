using System.Threading;

using AlexaComp.Forms;

namespace AlexaComp.Core.Requests {

    public class OpenDevTools : AlexaCompCore {
        
        public OpenDevTools(){
            Clog("OpenDevTools Constructed");
            process();
        }

        public void process() {
            Thread AdvancedSettingsThread = new Thread(AdvancedSettingsForm.startAdvToolsThread) {
                Name = "AdvancedSettingsThread"
            };
            Clog("Starting Window...");
            AdvancedSettingsThread.Start();
            Clog("Window Started");
        }
    }
}