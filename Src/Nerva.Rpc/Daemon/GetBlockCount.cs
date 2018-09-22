using System;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetBlockCount : Request<object, uint>
    {
        public GetBlockCount(Action<uint> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

        protected override bool DoRequest(out uint result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_block_count", null, out json);
            result = r ? JObject.Parse(json)["result"]["count"].Value<uint>() : 0;
            return r;
        }
    }
}