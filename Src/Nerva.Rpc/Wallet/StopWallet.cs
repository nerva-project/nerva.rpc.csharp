using System;

namespace Nerva.Rpc.Wallet
{
    public class StopWallet : Request<object, string>
    {
        public StopWallet(Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("stop_wallet", null, out result);
    }
}