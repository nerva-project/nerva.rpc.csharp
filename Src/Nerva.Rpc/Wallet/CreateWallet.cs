using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class CreateWallet : RpcRequest<CreateWalletRequestData, string>
    {
        public CreateWallet (CreateWalletRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out string result)
        {
            return BasicRequest("create_wallet", rpcData, out result);
        }
    }

    public class CreateWalletRequestData : OpenWalletRequestData
    {
        private const string LANGUAGE = "English";

        [JsonProperty("language")]
        public string Language => LANGUAGE;
    }
}