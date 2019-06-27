using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class SubmitBlock : Request<List<string>, string>
    {
        public SubmitBlock(List<string> rpcData, Action<string> completeAction, Action<RequestError> failedAction,
            string host = "http://127.0.0.1", uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out string result)
        {
            bool r = JsonRpcRequest("submit_block", rpcData, out result);
            return r;
        }
    }
}