using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class OpenWallet : Request<T>
    {
        public OpenWallet(Action completeAction, Action failedAction)
            : base (completeAction, failedAction) { }
            
        protected override bool SendAction(out string result)
        {
            return BasicRequest("open_wallet");
        }
    }

    public class OpenWalletData
    {
        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}