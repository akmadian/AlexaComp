using AlexaComp.Controllers;

namespace AlexaComp.Core.Requests {

    public class RGBCommand : AlexaCompCore {

        public string effect;

        public string color1;

        public string color2;

        public RGBCommand(string effect_, string color1_, string color2_ = null) {
            Clog("RGBCommand Constructed");
            effect = effect_;
            color1 = color1_;
            color2 = color2_;
            process();
        }
        
        public void process() {
            Clog("Processing...");
            RGBColor priColor = new RGBColor(255, 255, 255);
            RGBColor secColor = new RGBColor(0, 0, 255);
            // if (req.SECONDARY != null) { priColor = new RGBColor(req.SECONDARY); } else { priColor = null; };
            // if (req.TERTIARY != null) { secColor = new RGBColor(req.TERTIARY); } else { secColor = null; };
            LightingController.startLightingThread();
            LightingController.lightingProcess(priColor, secColor, effect);
        }
    }
}