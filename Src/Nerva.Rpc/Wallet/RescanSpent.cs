using System;

namespace Nerva.Rpc.Wallet
{
    public class RescanSpent : Request<object, string>
    {
        public RescanSpent(Action<string> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("rescan_spent", null, out result);
    }
}