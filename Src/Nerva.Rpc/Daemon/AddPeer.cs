using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class AddPeer : Request<AddPeerRequestData, string>
    {
        public AddPeer(AddPeerRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("add_peer", rpcData, out result);
    }

    [JsonObject]
    public class AddPeerRequestData
    {
        [JsonProperty("host")]
        public string Host { get; set; }
    }
}