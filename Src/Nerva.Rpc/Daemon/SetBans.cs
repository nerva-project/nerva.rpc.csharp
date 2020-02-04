using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class SetBans : Request<SetBansRequestData, string>
    {
        public SetBans(SetBansRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out string result) => JsonRpcRequest("set_bans", rpcData, out result);
    }

    [JsonObject]
    public class SetBansRequestData
    {
        [JsonProperty("bans")]
        public List<SetBansItem> Bans { get; set; }
    }

    [JsonObject]
    public class SetBansItem
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("ban")]
        public bool Banned { get; set; } = true;

        [JsonProperty("seconds")]
        public uint Seconds { get; set; } = 6000;

    }
}