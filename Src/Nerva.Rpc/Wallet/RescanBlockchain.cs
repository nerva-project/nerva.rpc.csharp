using System;

namespace Nerva.Rpc.Wallet
{
    public class RescanBlockchain : Request<object, string>
    {
        public RescanBlockchain(Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (null, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("rescan_blockchain", null, out result);

    }
}