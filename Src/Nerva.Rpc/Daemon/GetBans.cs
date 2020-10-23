using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class GetBans : Request<object, GetBansResponseData>
    {
        public GetBans(Action<GetBansResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_DAEMON_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetBansResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_bans", null, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetBansResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetBansResponseData
    {
        [JsonProperty("bans")]
        public List<GetBansItem> Bans { get; set; }
    }

    [JsonObject]
    public class GetBansItem
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("ban")]
        public bool Banned { get; set; } = true;

        [JsonProperty("seconds")]
        public uint Seconds => 6000;

    }
}