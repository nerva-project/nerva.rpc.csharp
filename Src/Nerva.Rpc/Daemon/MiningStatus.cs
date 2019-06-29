using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class MiningStatus : Request<object, MiningStatusResponseData>
    {
        public MiningStatus(Action<MiningStatusResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out MiningStatusResponseData result)
        {
            string json = null;
            bool r = RpcRequest("mining_status", null, out json);
            result = r ? JsonConvert.DeserializeObject<MiningStatusResponseData>(json) : null;
            return r;
        }
    }

    [JsonObject]
    public class MiningStatusResponseData
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("is_background_mining_enabled")]
        public bool IsBackground { get; set; }

        [JsonProperty("speed")]
        public uint Speed { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("threads_count")]
        public uint ThreadCount { get; set; }
    }
}