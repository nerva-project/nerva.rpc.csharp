using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class SubmitTransfer : Request<SubmitTransferRequestData, SubmitTransferResponseData>
    {
        public SubmitTransfer (SubmitTransferRequestData rpcData, Action<SubmitTransferResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out SubmitTransferResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("submit_transfer", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<SubmitTransferResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class SubmitTransferRequestData
    {
        [JsonProperty("tx_data_hex")]
        public string TxDataHex { get; set; } = string.Empty;
    }

    [JsonObject]
    public class SubmitTransferResponseData
    {
        [JsonProperty("tx_hash_list")]
        public List<string> TxHashList { get; set; } = new List<string>();
    }
}
