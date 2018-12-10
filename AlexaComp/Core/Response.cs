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
        public Response(bool passorfail, string message, string primary = "", string secondary = "") {
            this.passorfail = passorfail;
            this.message = message;
            this.primary = primary;
            this.secondary = secondary;
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
                AlexaCompSERVER.SendToLambda(json);
            }
        }
        #endregion
    }
}
