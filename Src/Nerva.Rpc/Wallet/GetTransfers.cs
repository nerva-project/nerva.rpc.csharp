using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetTransfers : Request<GetTransfersRequestData, GetTransfersResponseData>
    {
        public GetTransfers(GetTransfersRequestData rpcData, Action<GetTransfersResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetTransfersResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_transfers", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetTransfersResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetTransfersRequestData
    {
        [JsonProperty("in")]
        public bool In { get; set; } = true;

        [JsonProperty("out")]
        public bool Out { get; set; } = true;

        [JsonProperty("pending")]
        public bool Pending { get; set; } = true;

        [JsonProperty("failed")]
        public bool Failed { get; set; } = false;

        [JsonProperty("pool")]
        public bool Pool { get; set; } = false;

        [JsonProperty("filter_by_height")]
        public bool FilterByHeight { get; set; } = false;

        [JsonProperty("min_height")]
        public ulong ScanFromHeight { get; set; } = 0;

        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; } = 0;

        [JsonProperty("subaddr_indices")]
        public List<uint> SubaddrIndices { get; set; } = new List<uint>();

        [JsonProperty("all_accounts")]
        public bool AllAccounts { get; set; } = false;
    }

    [JsonObject]
    public class GetTransfersResponseData
    {
        [JsonProperty("in")]
        public List<TransferItem> Incoming { get; set; } = new List<TransferItem>();

        [JsonProperty("out")]
        public List<TransferItem> Outgoing { get; set; } = new List<TransferItem>();

        [JsonProperty("pending")]
        public List<TransferItem> Pending { get; set; } = new List<TransferItem>();

        [JsonProperty("failed")]
        public List<TransferItem> Failed { get; set; } = new List<TransferItem>();

        [JsonProperty("pool")]
        public List<TransferItem> Pool { get; set; } = new List<TransferItem>();
    }

    [JsonObject]
    public class TransferItem
    {
        [JsonProperty("txid")]
        public string TxId { get; set; } = string.Empty;

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;

        [JsonProperty("height")]
        public uint Height { get; set; } = 0;

        [JsonProperty("timestamp")]
        public ulong Timestamp { get; set; } = 0;

        [JsonProperty("amount")]
        public ulong Amount { get; set; } = 0;

        [JsonProperty("fee")]
        public ulong Fee { get; set; } = 0;

        [JsonProperty("note")]
        public string Note { get; set; } = string.Empty;

        [JsonProperty("unlock_time")]
        public ulong UnlockTime { get; set; } = 100;

        [JsonProperty("locked")]
        public bool Locked { get; set; } = true;

        [JsonProperty("confirmations")]
        public ulong Confirmations { get; set; } = 0;

        [JsonProperty("suggested_confirmations_threshold")]
        public ulong SuggestedConfirmationThreshold { get; set; } = 0;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("destinations")]
        public List<TransferDestination> Destinations { get; set; } = new List<TransferDestination>();

        [JsonProperty("subaddr_index")]
        public SubaddressIndex SubaddrIndex { get; set; } = new SubaddressIndex();
    }

    [JsonObject]
    public class TransferDestination
    {
        [JsonProperty("amount")]
        public ulong Amount { get; set; } = 0;

        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;
    }

    [JsonObject]
    public class SubaddressIndex
    {
        [JsonProperty("major")]
        public uint Major { get; set; } = 0;

        [JsonProperty("minor")]
        public uint Minor { get; set; } = 0;
    }
}