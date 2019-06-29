using System;

namespace Nerva.Rpc.Daemon
{
    public class StopDaemon : Request<object, string>
    {
        public StopDaemon(Action<string> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out string result) => RpcRequest("stop_daemon", null, out result);
    }
}