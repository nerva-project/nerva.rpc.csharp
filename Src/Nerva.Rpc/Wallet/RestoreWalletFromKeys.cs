using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class RestoreWalletFromKeys : Request<RestoreWalletFromKeysRequestData, RestoreWalletFromKeysResponseData>
    {
        public RestoreWalletFromKeys(RestoreWalletFromKeysRequestData rpcData, Action<RestoreWalletFromKeysResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out RestoreWalletFromKeysResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("restore_wallet_from_keys", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<RestoreWalletFromKeysResponseData>>(json).Result : null;
            return r;
        }
    }

    public class RestoreWalletFromKeysRequestData : CreateWalletRequestData
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

    public class RestoreWalletFromKeysResponseData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }
    }
}