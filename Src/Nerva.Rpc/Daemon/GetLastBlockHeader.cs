using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetLastBlockHeader : Request<object, BlockHeaderResponseData>
    {
        public GetLastBlockHeader(Action<BlockHeaderResponseData> completeAction, Action<RequestError> failedAction, 
            string host = "http://127.0.0.1", uint port = 17566, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out BlockHeaderResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_last_block_header", null, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<BlockHeaderResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class BlockHeaderResponseData
    {
        [JsonProperty("block_size")]
        public uint BlockSize { get; set; }

        [JsonProperty("depth")]
        public ulong Depth { get; set; }

        [JsonProperty("difficulty")]
        public ulong Difficulty { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("height")]
        public ulong Height { get; set; }

        [JsonProperty("major_version")]
        public uint MajorVersion { get; set; }

        [JsonProperty("minor_version")]
        public uint MinorVersion { get; set; }

        [JsonProperty("nonce")]
        public uint Nonce { get; set; }

        [JsonProperty("num_txes")]
        public uint TxCount { get; set; }

        [JsonProperty("orphan_status")]
        public uint IsOrphaned { get; set; }

        [JsonProperty("prev_hash")]
        public string PreviousHash { get; set; }

        [JsonProperty("reward")]
        public ulong Reward { get; set; }

        [JsonProperty("timestamp")]
        public ulong Timestamp { get; set; }
    }
}
