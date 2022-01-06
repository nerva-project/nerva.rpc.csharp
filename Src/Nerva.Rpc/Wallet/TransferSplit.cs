using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nerva.Rpc.Wallet.Helpers;

namespace Nerva.Rpc.Wallet
{
    public class TransferSplit : Request<TransferSplitRequestData, TransferSplitResponseData>
    {
        public TransferSplit (TransferSplitRequestData rpcData, Action<TransferSplitResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out TransferSplitResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("transfer_split", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<TransferSplitResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class TransferSplitRequestData
    {
        [JsonProperty("destinations")]
        public List<TransferDestination> Destinations { get; set; } = new List<TransferDestination>();

        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; } = 0;

        [JsonProperty("subaddr_indices")]
        public List<uint> SubAddressIndices { get; set; } = new List<uint>();

        [JsonProperty("priority")]
        public uint Priority { get; set; } = (uint)SendPriority.Default;

        [JsonProperty("unlock_time")]
        public uint UnlockTime { get; set; } = 0;

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;
        
        [JsonProperty("get_tx_keys")]
        public bool GetTxKeys { get; set; } = true;

        [JsonProperty("do_not_relay")]
        public bool DoNotRelay { get; set; } = false;

        [JsonProperty("get_tx_hex")]
        public bool GetTxHex { get; set; } = false;

        [JsonProperty("get_tx_metadata")]
        public bool GetTxMetadata { get; set; } = false;
    }

    [JsonObject]
    public class TransferSplitResponseData
    {
        [JsonProperty("tx_hash_list")]
        public List<string> TxHashList { get; set; } = new List<string>();

        [JsonProperty("tx_key_list")]
        public List<string> TxKeyList { get; set; } = new List<string>();

        [JsonProperty("amount_list")]
        public List<ulong> AmountList { get; set; } = new List<ulong>();

        [JsonProperty("fee_list")]
        public List<ulong> FeeList { get; set; } = new List<ulong>();

        [JsonProperty("tx_blob_list")]
        public List<string> TxBlobList { get; set; } = new List<string>();

        [JsonProperty("tx_metadata_list")]
        public List<string> TxMetadataList { get; set; } = new List<string>();

        [JsonProperty("multisig_txset")]
        public string MultisigTxSet { get; set; } = string.Empty;

        [JsonProperty("unsigned_txset")]
        public string UnsignedTxSet { get; set; } = string.Empty;
    }
}