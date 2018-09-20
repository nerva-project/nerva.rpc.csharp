using System;

namespace Nerva.Rpc.Wallet
{
    public class StopWallet : Request
    {
        public StopWallet (Action<string> completeAction, Action failedAction)
            : base (completeAction, failedAction) { }
            
        protected override bool SendAction(out string result)
        {
            result = null;
            return BasicRequest("stop_wallet");
        }
    }
}