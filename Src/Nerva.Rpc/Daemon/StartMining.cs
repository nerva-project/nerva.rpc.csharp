using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class StartMining : Request<StartMiningRequestData, string>
    {
        public StartMining(StartMiningRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }

        protected override bool DoRequest(out string result) => RpcRequest("start_mining", JsonConvert.SerializeObject(rpcData), out result);
    }

    [JsonObject]
    public class StartMiningRequestData
    {
        [JsonProperty("do_background_mining")]
        public bool BackgroundMining { get; set; } = false;

        [JsonProperty("ignore_battery")]
        public bool IgnoreBattery { get; set; } = true;

        [JsonProperty("miner_address")]
        public string MinerAddress { get; set; }

        [JsonProperty("threads_count")]
        public int MiningThreads { get; set; } = 1;
    }
}