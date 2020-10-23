using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class RestoreWalletFromSeed : Request<RestoreWalletFromSeedRequestData, RestoreWalletFromSeedResponseData>
    {
        public RestoreWalletFromSeed(RestoreWalletFromSeedRequestData rpcData, Action<RestoreWalletFromSeedResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out RestoreWalletFromSeedResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("restore_wallet_from_seed", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<RestoreWalletFromSeedResponseData>>(json).Result : null;
            return r;
        }
    }

    public class RestoreWalletFromSeedRequestData : CreateWalletRequestData
    {
        [JsonProperty("restore_height")]
        public ulong RestoreHeight { get; set; } = 0;

        [JsonProperty("seed")]
        public string Seed { get; set; } = "";

        [JsonProperty("seed_offset")]
        public string SeedOffset { get; set; } = "";
    }

    public class RestoreWalletFromSeedResponseData
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