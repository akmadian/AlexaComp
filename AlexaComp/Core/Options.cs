namespace AlexaComp.Core{
    class Options {
        public Request req;
        public System.Net.Sockets.NetworkStream nwStream;

        public Options(Request req_, System.Net.Sockets.NetworkStream nwStream_) {
            req = req_;
            nwStream = nwStream_;
        }
    }
}