using System;

namespace AlexaComp.Core.Requests {
    public class CompStat : AlexaCompCore {
        
        private string part;
        private string stat;
        private string TERTIARY;

        public CompStat(string part_, string stat_, string TERTIARY_) {
            part = part_;
            stat = stat_;
            TERTIARY = TERTIARY_;
            process();
        }

        public void process() {
            // TODO : Fix Compstat System
            // TODO : Implement switch case
            if (this.part == "RAM") {
                Response res = new Response(true, "5.3 Gigabites");
            }
            else if (this.part == "GPU") {
                Response res = new Response(true, "38 degrees celcius");
            } else {
                Devices[part].RefreshSensors();
                string result = Devices[part].Sensors[stat].Value.ToString();
                Console.WriteLine(result);
                Response res = new Response(true, result);
            }
        }
    }
}