using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockHeaderByHeight : Request<GetBlockHeaderByHeightRequestData, BlockHeaderResponseData>
    {
        public GetBlockHeaderByHeight(GetBlockHeaderByHeightRequestData rpcData, Action<BlockHeaderResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_DAEMON_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out BlockHeaderResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block_header_by_height", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<BlockHeaderResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetBlockHeaderByHeightRequestData
    {
        [JsonProperty("height")]
        public uint Height { get; set; }
    }
}