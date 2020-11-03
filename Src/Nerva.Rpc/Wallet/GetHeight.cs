using System;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Wallet
{
    public class GetHeight : Request<object, ulong>
    {
        public GetHeight(Action<ulong> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_DAEMON_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out ulong result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_height", null, out json);
            result = r ? JObject.Parse(json)["result"]["height"].Value<ulong>() : 0;
            return r;
        }
    }
}