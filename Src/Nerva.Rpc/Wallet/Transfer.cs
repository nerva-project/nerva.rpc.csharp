using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class Transfer : Request<TransferRequestData, TransferResponseData>
    {
        public Transfer (TransferRequestData rpcData, Action<TransferResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
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
        [JsonProperty("destinations")]
        public List<TransferDestination> Destinations { get; set; } = new List<TransferDestination>();

        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; } = 0;

        [JsonProperty("subaddr_indices")]
        public List<uint> SubAddressIndices { get; set; } = new List<uint>();

        [JsonProperty("priority")]
        public uint Priority { get; set; } = (uint)Send_Priority.Default;

        [JsonProperty("unlock_time")]
        public uint UnlockTime { get; set; } = 0;

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;
        
        [JsonProperty("get_tx_key")]
        public bool GetTxKey { get; set; } = true;

        [JsonProperty("do_not_relay")]
        public bool DoNotRelay { get; set; } = false;

        [JsonProperty("get_tx_hex")]
        public bool GetTxHex { get; set; } = false;

        [JsonProperty("get_tx_metadata")]
        public bool GetTxMetadata { get; set; } = false;
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
