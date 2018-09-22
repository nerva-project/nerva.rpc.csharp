using System;

namespace Nerva.Rpc.Daemon
{
    public class StopDaemon : Request<object, string>
    {
        public StopDaemon(Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

        protected override bool DoRequest(out string result) => RpcRequest("stop_daemon", null, out result);
    }
}