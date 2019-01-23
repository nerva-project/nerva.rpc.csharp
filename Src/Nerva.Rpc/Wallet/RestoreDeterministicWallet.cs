using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class RestoreDeterministicWallet : Request<RestoreDeterministicWalletRequestData, RestoreDeterministicWalletResponseData>
    {
        public RestoreDeterministicWallet(RestoreDeterministicWalletRequestData rpcData, Action<RestoreDeterministicWalletResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out RestoreDeterministicWalletResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("restore_deterministic_wallet", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<RestoreDeterministicWalletResponseData>>(json).Result : null;
            return r;
        }
    }

    public class RestoreDeterministicWalletRequestData : CreateWalletRequestData
    {
        [JsonProperty("restore_height")]
        public ulong RestoreHeight { get; set; } = 0;

        [JsonProperty("seed")]
        public string Seed { get; set; } = "";

        [JsonProperty("seed_offset")]
        public string SeedOffset { get; set; } = "";
    }

    public class RestoreDeterministicWalletResponseData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("seed")]
        public string Seed{ get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("was_deprecated")]
        public string WasDeprecated { get; set; }
    }
}