using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetInfo : Request<object, GetInfoResponseData>
    {
        public GetInfo(Action<GetInfoResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_DAEMON_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out GetInfoResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_info", null, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetInfoResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetInfoResponseData
    {
        [JsonProperty("alt_block_count")]
        public uint AltBlockCount { get; set; }

        [JsonProperty("block_size_limit")]
        public uint BlockSizeLimit { get; set; }

        [JsonProperty("block_size_median")]
        public uint BlockSizeMedian { get; set; }

        [JsonProperty("bootstrap_daemon_address")]
        public string BootstrapDaemonAddress { get; set; }

        [JsonProperty("cumulative_difficulty")]
        public ulong CumulativeDifficulty { get; set; }

        [JsonProperty("difficulty")]
        public ulong Difficulty { get; set; }

        [JsonProperty("free_space")]
        public ulong FreeSpace { get; set; }

        [JsonProperty("grey_peerlist_size")]
        public uint GreyPeerListSize { get; set; }

        [JsonProperty("height")]
        public ulong Height { get; set; }

        [JsonProperty("height_without_bootstrap")]
        public ulong HeightWithoutBootstrap { get; set; }

        [JsonProperty("incoming_connections_count")]
        public uint IncomingConnectionsCount { get; set; }

        [JsonProperty("mainnet")]
        public bool Mainnet { get; set; }

        [JsonProperty("offline")]
        public bool Offline { get; set; }

        [JsonProperty("outgoing_connections_count")]
        public uint OutgoingConnectionsCount { get; set; }

        [JsonProperty("rpc_connections_count")]
        public uint RpcConnectionsCount { get; set; }

        [JsonProperty("stagenet")]
        public bool Stagenet { get; set; }

        [JsonProperty("start_time")]
        public ulong StartTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("target")]
        public uint Target { get; set; }

        [JsonProperty("target_height")]
        public ulong TargetHeight { get; set; }

        [JsonProperty("testnet")]
        public bool Testnet { get; set; }

        [JsonProperty("top_block_hash")]
        public string TopBlockHash { get; set; }

        [JsonProperty("tx_count")]
        public uint TxCount { get; set; }

        [JsonProperty("tx_pool_size")]
        public uint TxPoolSize { get; set; }

        [JsonProperty("untrusted")]
        public bool Untrusted { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("was_bootstrap_ever_used")]
        public bool WasBootstrapEverUsed { get; set; }

        [JsonProperty("white_peerlist_size")]
        public uint WhitePeerListSize { get; set; }
    }
}