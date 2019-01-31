using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockHeaderByHash : Request<GetBlockHeaderByHashRequestData, BlockHeaderResponseData>
    {
        public GetBlockHeaderByHash(GetBlockHeaderByHashRequestData rpcData, Action<BlockHeaderResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out BlockHeaderResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block_header_by_hash", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<BlockHeaderResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetBlockHeaderByHashRequestData
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}