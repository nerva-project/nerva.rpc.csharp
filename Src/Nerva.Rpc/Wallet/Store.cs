using System;

namespace Nerva.Rpc.Wallet
{
    public class Store : RpcRequest<object, string>
    {
        public Store (Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }

        protected override bool DoRequest(out string result)
        {
            return BasicRequest("store", null, out result);
        }
    }
}