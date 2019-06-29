using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class StopMining : Request<object, string>
    {
        public StopMining(Action<string> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out string result) => RpcRequest("stop_mining", null, out result);
    }
}