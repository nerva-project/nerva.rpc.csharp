using System;

namespace Nerva.Rpc.Wallet
{
    public class CloseWallet : Request<object, string>
    {
        public CloseWallet(Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (null, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("close_wallet", null, out result);
    }
}