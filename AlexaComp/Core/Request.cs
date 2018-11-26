using System.Diagnostics;

namespace AlexaComp.Core {

    [DebuggerDisplay("[AlexaComp Request Object -- {{COMMAND: {COMMAND}, PRIMARY: {PRIMARY}, SECONDARY: {SECONDARY}, TERTIARY: {TERTIARY}}}]")]
    class Request : AlexaCompCore {
        #region Properties
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
        public string TERTIARY { get; set; }
        public string SESSID { get; set; }

        public Stopwatch sw = new Stopwatch();
        #endregion

        #region Constructors
        public Request() {
            sw.Start();
        }

        public Request(string AUTH_, string COMMAND_, string PRIMARY_ = "", string SECONDARY_ = "", string TERTIARY_ = "") {
            sw.Start();
            AUTH = AUTH_;
            COMMAND = COMMAND_;
            PRIMARY = PRIMARY_;
            SECONDARY = SECONDARY_;
            TERTIARY = TERTIARY_;
        }
        #endregion

        #region Methods
        public void logTimeElapsed() {
            sw.Stop();
            clog("Request Completed, Time Elapsed(ms) - " + sw.ElapsedMilliseconds.ToString());
        }

        /*
        public void printRequest() {
            string str = string.Format("New Request - {{Command: {0}, Primary: {1}, Secondary: {2}, Tertiary: {3}}}", COMMAND, PRIMARY, SECONDARY, TERTIARY);
            clog(str);
        }*/
        #endregion
    }
}
