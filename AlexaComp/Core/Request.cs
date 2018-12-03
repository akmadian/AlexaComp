using System.Diagnostics;

using AlexaComp.Core.Requests;

namespace AlexaComp.Core {

    [DebuggerDisplay("[AlexaComp Request Object -- {{COMMAND: {COMMAND}, PRIMARY: {PRIMARY}, SECONDARY: {SECONDARY}, TERTIARY: {TERTIARY}}}]")]
    public class Request : AlexaCompCore {
        #region Properties
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
        public string TERTIARY { get; set; }
        public string SESSID { get; set; }
        public string COLOR1 { get; set; }
        public string COLOR2 { get; set; }

        public Stopwatch sw = new Stopwatch();
        #endregion

        #region Constructors
        /*
        public Request() {
            sw.Start();
            processRequest(this);
        }*/

        public Request(string AUTH_, string COMMAND_, string PRIMARY_ = "", string SECONDARY_ = "", string TERTIARY_ = "") {
            AUTH = AUTH_;
            COMMAND = COMMAND_;
            PRIMARY = PRIMARY_;
            SECONDARY = SECONDARY_;
            TERTIARY = TERTIARY_;
            ProcessRequest(this);
        }
        #endregion

        #region Methods
        public void LogTimeElapsed() {
            sw.Stop();
            Clog("Request Completed, Time Elapsed(ms) - " + sw.ElapsedMilliseconds.ToString());
        }

        public void ProcessRequest(Request req) {
            switch (req.COMMAND) {
                case "LAUNCH":
                    Launch unused = new Launch(req.PRIMARY);
                    break;
                case "COMPUTERCOMMAND":
                    SystemCommand unused2 = new SystemCommand(req.PRIMARY);
                    break;
                case "GETCOMPSTAT":
                    CompStat unused3 = new CompStat(req.PRIMARY, req.SECONDARY, req.TERTIARY);
                    break;
                case "AUDIOCOMMAND":
                    AudioCommand unused4 = new AudioCommand(req.PRIMARY, req.SECONDARY);
                    break;
                case "RGBCOMMAND":
                    RGBCommand unused5 = new RGBCommand(req.PRIMARY, req.COLOR1, req.COLOR2);
                    break;
                case "OPENDEVTOOLS":
                    OpenDevTools unused6 = new OpenDevTools();
                    break;
            }
            req.LogTimeElapsed();
        }

        /*
        public void printRequest() {
            string str = string.Format("New Request - {{Command: {0}, Primary: {1}, Secondary: {2}, Tertiary: {3}}}", COMMAND, PRIMARY, SECONDARY, TERTIARY);
            clog(str);
        }*/
        #endregion
    }
}
