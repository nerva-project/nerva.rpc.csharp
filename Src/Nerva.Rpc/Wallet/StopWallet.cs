using System;

namespace Nerva.Rpc.Wallet
{
    public class StopWallet : RpcRequest<object, string>
    {
        public StopWallet (Action<string> completeAction, Action failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

        protected override bool DoRequest(out string result)
        {
            return BasicRequest("stop_wallet", null, out result);
        }
    }
}