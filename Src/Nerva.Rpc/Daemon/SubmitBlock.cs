using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class SubmitBlock : Request<List<string>, string>
    {
        public SubmitBlock(List<string> rpcData, Action<string> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_DAEMON_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out string result) => JsonRpcRequest("submit_block", rpcData, out result);
    }
}