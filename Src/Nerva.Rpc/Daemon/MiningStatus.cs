using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class MiningStatus : Request<object, MiningStatusResponseData>
    {
        public MiningStatus(Action<MiningStatusResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

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
        public int Speed { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("threads_count")]
        public int ThreadCount { get; set; }
    }
}