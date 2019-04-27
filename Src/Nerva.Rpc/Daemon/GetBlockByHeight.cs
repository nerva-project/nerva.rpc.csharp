using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockByHeight : Request<GetBlockByHeightRequestData, BlockResponseData>
    {
        public GetBlockByHeight(GetBlockByHeightRequestData rpcData, Action<BlockResponseData> completeAction, 
            Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out BlockResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<BlockResponseData>>(json).Result : null;
            if (result != null)
                result.JsonData = JsonConvert.DeserializeObject<BlockJsonData>(result.JsonString);
            return r;
        }
    }

    [JsonObject]
    public class GetBlockByHeightRequestData
    {
        [JsonProperty("height")]
        public uint Height { get; set; }
    }

    [JsonObject]
    public class BlockResponseData
    {
        [JsonProperty("blob")]
        public string Blob { get; set; }

        [JsonProperty("block_header")]
        public BlockHeaderResponseData BlockHeader { get; set; }

        [JsonProperty("json")]
        public string JsonString { get; set; }

        public BlockJsonData JsonData { get; set; }
    }

    [JsonObject]
    public class BlockJsonData
    {
        [JsonProperty("major_version")]
        public uint MajorVersion { get; set; }

        [JsonProperty("minor_version")]
        public uint MinorVersion { get; set; }

        [JsonProperty("timestamp")]
        public ulong Timestamp { get; set; }

        [JsonProperty("prev_id")]
        public string PreviousHash { get; set; }

        [JsonProperty("nonce")]
        public uint Nonce { get; set; }

        [JsonProperty("miner_tx")]
        public MinerTx MinerTx { get; set; }

        [JsonProperty("tx_hashes")]
        public string[] TxHashes { get; set; }
    }

    public class MinerTx
    {
        [JsonProperty("version")]
        public uint Version { get; set; }

        [JsonProperty("unlock_time")]
        public ulong UnlockTime { get; set; }

        [JsonProperty("extra")]
        public byte[] Extra{ get; set; }

        //todo: need to parse inputs/outputs of miner tx

    }
}