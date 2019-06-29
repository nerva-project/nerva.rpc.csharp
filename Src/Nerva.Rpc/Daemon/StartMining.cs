using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class StartMining : Request<StartMiningRequestData, string>
    {
        public StartMining(StartMiningRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }

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
        public uint MiningThreads { get; set; } = 1;
    }
}