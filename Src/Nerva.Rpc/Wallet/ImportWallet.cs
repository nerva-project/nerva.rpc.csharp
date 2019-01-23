using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class ImportWallet : Request<ImportWalletRequestData, ImportWalletResponseData>
    {
        public ImportWallet(ImportWalletRequestData rpcData, Action<ImportWalletResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out ImportWalletResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("restore_deterministic_wallet", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<ImportWalletResponseData>>(json).Result : null;
            return r;
        }
    }

    public class ImportWalletRequestData : CreateWalletRequestData
    {
        [JsonProperty("restore_height")]
        public ulong RestoreHeight { get; set; } = 0;

        [JsonProperty("seed")]
        public string Seed { get; set; } = "";

        [JsonProperty("seed_offset")]
        public string SeedOffset { get; set; } = "";
    }

    public class ImportWalletResponseData
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