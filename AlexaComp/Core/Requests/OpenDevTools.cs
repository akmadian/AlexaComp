using System.Threading;

using AlexaComp.Forms;

namespace AlexaComp.Core.Requests {

    public class OpenDevTools {
        
        public OpenDevTools(){
            process();
        }

        public void process() {
            Thread AdvancedSettingsThread = new Thread(AdvancedSettingsForm.startAdvToolsThread) {
                Name = "AdvancedSettingsThread"
            };
            AdvancedSettingsThread.Start();
        }
    }
}