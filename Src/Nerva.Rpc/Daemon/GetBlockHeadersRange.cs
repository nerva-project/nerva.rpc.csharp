using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockHeadersRange : Request<GetBlockHeadersRangeRequestData, List<BlockHeaderResponseData>>
    {
        public GetBlockHeadersRange(GetBlockHeadersRangeRequestData rpcData, Action<List<BlockHeaderResponseData>> completeAction, 
            Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out List<BlockHeaderResponseData> result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block_headers_range", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetBlockHeadersRangeResponseData>>(json).Result.Headers : null;
            return r;
        }
    }

    [JsonObject]
    public class GetBlockHeadersRangeRequestData
    {
        [JsonProperty("start_height")]
        public uint StartHeight { get; set; }

        [JsonProperty("end_height")]
        public uint EndHeight { get; set; }
    }

    [JsonObject]
    public class GetBlockHeadersRangeResponseData
    {
        [JsonProperty("headers")]
        public List<BlockHeaderResponseData> Headers { get; set; }
    }
}