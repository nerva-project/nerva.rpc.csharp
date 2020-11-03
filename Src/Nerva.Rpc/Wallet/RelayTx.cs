using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class RelayTx : Request<RelayTxRequestData, RelayTxResponseData>
    {
        public RelayTx (RelayTxRequestData rpcData, Action<RelayTxResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out RelayTxResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("relay_tx", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<RelayTxResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class RelayTxRequestData
    {
        [JsonProperty("hex")]
        public string TxHex { get; set; } = string.Empty;
    }

    [JsonObject]
    public class RelayTxResponseData
    {
        [JsonProperty("tx_hash")]
        public string TxHash { get; set; } = string.Empty;
    }
}
