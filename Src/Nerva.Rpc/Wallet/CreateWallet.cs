using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class CreateWallet : Request
    {
        public CreateWallet (Action<string> completeAction, Action failedAction)
            : base (completeAction, failedAction) { }
            
        protected override bool SendAction(out string result)
        {
            return BasicRequest("create_wallet", new CreateWalletData {
                FileName = walletName,
                Password = password
            }, out result);
        }
    }

    public class CreateWalletData : OpenWalletData
    {
        private const string LANGUAGE = "English";

        [JsonProperty("language")]
        public string Language => LANGUAGE;
    }
}