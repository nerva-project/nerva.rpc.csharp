using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetLastBlockHeader : Request<object, BlockHeaderResponseData>
    {
        public GetLastBlockHeader(Action<BlockHeaderResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

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
        public uint Depth { get; set; }

        [JsonProperty("difficulty")]
        public uint Difficulty { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("height")]
        public uint Height { get; set; }

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
        public uint PreviousHash { get; set; }

        [JsonProperty("reward")]
        public uint Reward { get; set; }

        [JsonProperty("timestamp")]
        public uint Timestamp { get; set; }
    }
}