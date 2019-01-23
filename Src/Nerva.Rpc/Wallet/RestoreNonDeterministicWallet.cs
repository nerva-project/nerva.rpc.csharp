using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class RestoreNonDeterministicWallet : Request<RestoreNonDeterministicWalletRequestData, RestoreNonDeterministicWalletResponseData>
    {
        public RestoreNonDeterministicWallet(RestoreNonDeterministicWalletRequestData rpcData, Action<RestoreNonDeterministicWalletResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out RestoreNonDeterministicWalletResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("restore_nondeterministic_wallet", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<RestoreNonDeterministicWalletResponseData>>(json).Result : null;
            return r;
        }
    }

    public class RestoreNonDeterministicWalletRequestData : CreateWalletRequestData
    {
        [JsonProperty("restore_height")]
        public ulong RestoreHeight { get; set; } = 0;

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("viewkey")]
        public string ViewKey { get; set; } = "";

        [JsonProperty("spendkey")]
        public string SpendKey { get; set; } = "";
    }

    public class RestoreNonDeterministicWalletResponseData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }
    }
}