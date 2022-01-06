using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nerva.Rpc.Wallet.Helpers;

namespace Nerva.Rpc.Wallet
{
    public class SweepAll : Request<SweepAllRequestData, SweepAllResponseData>
    {
        public SweepAll(SweepAllRequestData rpcData, Action<SweepAllResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out SweepAllResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("sweep_all", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<SweepAllResponseData>>(json).Result : null;
            return r;
        }
    }

    public class SweepAllRequestData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; } = 0;

        [JsonProperty("subaddr_indices")]
        public List<uint> SubaddressIndices { get; set; } = new List<uint>();

        [JsonProperty("priority")]
        public uint Priority { get; set; } = (uint)SendPriority.Default;

        [JsonProperty("unlock_time")]
        public uint UnlockTime { get; set; } = 0;

        [JsonProperty("get_tx_keys")]
        public bool GetTxKeys { get; set; } = true;

        [JsonProperty("do_not_relay")]
        public bool DoNotRelay { get; set; } = false;

        [JsonProperty("get_tx_hex")]
        public bool GetTxHex { get; set; } = false;

        [JsonProperty("get_tx_metadata")]
        public bool GetTxMetadata { get; set; } = false;
    }

    public class SweepAllResponseData
    {
        [JsonProperty("tx_hash_list")]
        public List<string> TxHashList { get; set; }

        [JsonProperty("tx_key_list")]
        public List<string> TxKeyList { get; set; }

        [JsonProperty("amount_list")]
        public List<ulong> AmountList { get; set; }

        [JsonProperty("fee_list")]
        public List<ulong> FeeList { get; set; }

        [JsonProperty("tx_blob_list")]
        public List<string> TxBlobList { get; set; }

        [JsonProperty("tx_metadata_list")]
        public List<string> TxMetadataList { get; set; }

        [JsonProperty("multisig_txset")]
        public string MultisigTxset { get; set; }

        [JsonProperty("unsigned_txset")]
        public string UnsignedTxset { get; set; }
    }
}