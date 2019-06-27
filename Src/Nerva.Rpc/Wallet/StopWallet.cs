using System;

namespace Nerva.Rpc.Wallet
{
    public class StopWallet : Request<object, string>
    {
        public StopWallet(Action<string> completeAction, Action<RequestError> failedAction,
            string host = "http://127.0.0.1", uint port = 17566, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("stop_wallet", null, out result);
    }
}