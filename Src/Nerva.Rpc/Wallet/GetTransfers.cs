using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetTransfers : RpcRequest<GetTransfersRequestData, GetTransfersResponseData>
    {
        public GetTransfers (GetTransfersRequestData rpcData, Action<GetTransfersResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out GetTransfersResponseData result)
        {
            string json = null;
            bool r = BasicRequest("get_transfers", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<JsonResponse<GetTransfersResponseData>>(json).Result : null;

            return r;
        }
    }

    [JsonObject]
    public class GetTransfersRequestData
    {
        [JsonProperty("in")]
        public bool In => true;

        [JsonProperty("out")]
        public bool Out => true;

        [JsonProperty("pending")]
        public bool Pending => true;

        [JsonProperty("failed")]
        public bool Failed => false;

        [JsonProperty("pool")]
        public bool Pool => false;

        [JsonProperty("filter_by_height")]
        public bool FilterByHeight => false;

        [JsonProperty("min_height")]
        public uint ScanFromHeight { get; set; } = 0;

        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; } = 0;
    }

    [JsonObject]
    public class GetTransfersResponseData
    {
        [JsonProperty("in")]
        public List<Transfer> Incoming { get; set; } = new List<Transfer>();

        [JsonProperty("out")]
        public List<Transfer> Outgoing { get; set; } = new List<Transfer>();

        [JsonProperty("pending")]
        public List<Transfer> Pending { get; set; } = new List<Transfer>();
    }

    [JsonObject]
    public class Transfer
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

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("destinations")]
        public List<Destination> Destinations { get; set; } = new List<Destination>();
    }

    [JsonObject]
    public class Destination
    {
        [JsonProperty("amount")]
        public ulong Amount { get; set; } = 0;

        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;
    }
}