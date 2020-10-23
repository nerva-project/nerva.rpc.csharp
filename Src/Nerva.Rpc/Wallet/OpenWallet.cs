using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class OpenWallet : Request<OpenWalletRequestData, string>
    {
        public OpenWallet(OpenWalletRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out string result) => JsonRpcRequest("open_wallet", rpcData, out result);
    }

    public class OpenWalletRequestData
    {
        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}