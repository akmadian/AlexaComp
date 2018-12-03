using System;
using System.Diagnostics;

namespace AlexaComp.Core {
    [DebuggerDisplay("[AlexaComp Response Object -- {{passorfail: {passorfail}, message: {message}, primary: {primary}, secondary: {secondary}}}]")]
    class Response : AlexaCompCore {
        #region Properties
        public bool passorfail;
        public string message;
        public string primary;
        public string secondary;

        public string json;
        #endregion

        #region Constructors
        public Response(bool passorfail_, string message_, string primary_ = "", string secondary_ = "") {
            passorfail = passorfail_;
            message = message_;
            primary = primary_;
            secondary = secondary_;

            json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            Console.WriteLine(json);
        }
        #endregion

        #region Methods
        public void SendResponse(System.Net.Sockets.NetworkStream customStream) {
            if (customStream != null) {
                AlexaCompSERVER.SendToLambda(json, customStream);
            }
            else {
                AlexaCompSERVER.SendToLambda(json, null);
            }
        }
        #endregion
    }
}
