using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class Transfer : Request<TransferRequestData, TransferResponseData>
    {
        public Transfer (TransferRequestData rpcData, Action<TransferResponseData> completeAction, Action<RequestError> failedAction,
            string host = "http://127.0.0.1", uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out TransferResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("transfer", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<TransferResponseData>>(json).Result : null;
            return r;
        }
    }

    public enum Send_Priority : uint
    {
        Default = 0,
        Low,
        Medium,
        High
    }

    [JsonObject]
    public class TransferRequestData
    {
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;

        [JsonProperty("destinations")]
        public List<TransferDestination> Destinations { get; set; } = new List<TransferDestination>();

        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; } = 0;

        [JsonProperty("mixin")]
        public uint Mixin => 5;
        
        [JsonProperty("get_tx_key")]
        public bool GetTxKey => true;

        [JsonProperty("priority")]
        public uint Priority { get; set; } = (uint)Send_Priority.Default;
    }

    [JsonObject]
    public class TransferResponseData
    {
        [JsonProperty("fee")]
        public ulong Fee { get; set; } = 0;

        [JsonProperty("tx_hash")]
        public string TxHash { get; set; } = string.Empty;

        [JsonProperty("tx_key")]
        public string TxKey { get; set; } = string.Empty;

        [JsonProperty("amount")]
        public ulong Amount { get; set; } = 0;
    }
}
