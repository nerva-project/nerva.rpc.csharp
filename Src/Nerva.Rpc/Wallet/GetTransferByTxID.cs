using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetTransferByTxID : RpcRequest<GetTransferByTxIDRequestData,GetTransferByTxIDResponseData>
    {
        public GetTransferByTxID (GetTransferByTxIDRequestData rpcData, Action<GetTransferByTxIDResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out GetTransferByTxIDResponseData result)
        {
            string json = null;
            bool r = BasicRequest("get_transfer_by_txid", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<JsonResponse<GetTransferByTxIDResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetTransferByTxIDRequestData
    {
        [JsonProperty("txid")]
        public string TxID { get; set; }
    }

    [JsonObject]
    public class GetTransferByTxIDResponseData
    {
        [JsonProperty("transfer")]
        public TransferTxID Transfer { get; set; }
    }

    [JsonObject]
    public class TransferTxID : Transfer
    {
        [JsonProperty("unlock_time")]
        public ulong UnlockTime { get; set; } = 0;

        [JsonProperty("subaddr_index")]
        public SubAddressIndex SubAddressIndex { get; set; } = null;

        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;

        [JsonProperty("double_spend_seen")]
        public bool DoubleSpendSeen { get; set; } = false;
    }

    [JsonObject]
    public class SubAddressIndex
    {
        [JsonProperty("major")]
        public uint Major { get; set; }

        [JsonProperty("minor")]
        public uint Minor { get; set; }
    }
}