using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockTemplate : Request<GetBlockTemplateRequestData, GetBlockTemplateResponseData>
    {
        public GetBlockTemplate(GetBlockTemplateRequestData rpcData, Action<GetBlockTemplateResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out GetBlockTemplateResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block_template", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetBlockTemplateResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetBlockTemplateRequestData
    {
        [JsonProperty("reserve_size")]
        public int ReserveSize { get; set; }

        [JsonProperty("wallet_address")]
        public string Address { get; set; }
    }

    [JsonObject]
    public class GetBlockTemplateResponseData
    {
        [JsonProperty("blockhashing_blob")]
        public string HashingBlob { get; set; }

        [JsonProperty("blocktemplate_blob")]
        public string TemplateBlob { get; set; }

        [JsonProperty("difficulty")]
        public uint Difficulty { get; set; }

        [JsonProperty("expected_reward")]
        public ulong ExpectedReward { get; set; }

        [JsonProperty("height")]
        public uint Height { get; set; }

        [JsonProperty("prev_hash")]
        public string PrevHash { get; set; }

        [JsonProperty("reserved_offset")]
        public uint ReservedOffset { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("untrusted")]
        public bool Untrusted { get; set; }
    }
}