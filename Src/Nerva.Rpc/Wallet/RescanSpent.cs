using System;

namespace Nerva.Rpc.Wallet
{
    public class RescanSpent : Request<object, string>
    {
        public RescanSpent(Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

        protected override bool DoRequest(out string result) => JsonRpcRequest("rescan_spent", null, out result);
    }
}