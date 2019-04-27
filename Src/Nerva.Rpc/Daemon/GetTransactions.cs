using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nerva.Rpc.Daemon
{
    public class GetTransactions : Request<GetTransactionsRequestData, GetTransactionsResponseData>
    {
        public GetTransactions(GetTransactionsRequestData rpcData, Action<GetTransactionsResponseData> completeAction, 
            Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out GetTransactionsResponseData result)
        {
            string json = null;
            bool r = RpcRequest("get_transactions", JsonConvert.SerializeObject(rpcData), out json);
            result = r ? JsonConvert.DeserializeObject<GetTransactionsResponseData>(json): null;

            if (r)
                foreach (var t in result.TransactionStrings)
                    result.Transactions.Add(JsonConvert.DeserializeObject<TransactionData>(t["as_json"].Value<string>()));

            return r;
        }
    }

    public enum Tx_Type
    {
      RCTTypeNull = 0,
      RCTTypeFull = 1,
      RCTTypeSimple = 2,
      RCTTypeFullBulletproof = 3,
      RCTTypeSimpleBulletproof = 4,
      RCTTypeBulletproof_v2 = 5,
    };

    [JsonObject]
    public class GetTransactionsRequestData
    {
        [JsonProperty("txs_hashes")]
        public List<string> Hashes { get; set; }

        [JsonProperty("decode_as_json")]
        public bool DecodeAsJson => true;
    }

    [JsonObject]
    public class GetTransactionsResponseData
    {
        [JsonProperty("txs")]
        public List<JContainer> TransactionStrings { get; set; }

        public List<TransactionData> Transactions { get; set; } = new List<TransactionData>();
    }

    public class TransactionData : MinerTx
    {
        [JsonProperty("rct_signatures")]
        public RingCtSignature RingCtSignatures  { get; set; }
    }

    public class RingCtSignature
    {
        [JsonProperty("type")]
        public Tx_Type Type { get; set; }

        [JsonProperty("txnFee")]
        public ulong Fee { get; set; }
    }
}