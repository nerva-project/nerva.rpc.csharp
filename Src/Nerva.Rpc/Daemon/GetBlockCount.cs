using System;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockCount : Request<object, uint>
    {
        public GetBlockCount(Action<uint> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out uint result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block_count", null, out json);
            result = r ? JObject.Parse(json)["result"]["count"].Value<uint>() : 0;
            return r;
        }
    }
}