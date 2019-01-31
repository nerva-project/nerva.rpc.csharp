using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class StopMining : Request<object, string>
    {
        public StopMining(Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (null, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out string result) => RpcRequest("stop_mining", null, out result);
    }
}