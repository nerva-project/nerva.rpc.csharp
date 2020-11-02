using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class SignTransfer : Request<SignTransferRequestData, SignTransferResponseData>
    {
        public SignTransfer (SignTransferRequestData rpcData, Action<SignTransferResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out SignTransferResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("sign_transfer", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<SignTransferResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class SignTransferRequestData
    {
        [JsonProperty("unsigned_txset")]
        public string UnsignedTxSet { get; set; } = string.Empty;

        [JsonProperty("export_raw")]
        public bool ExportRaw { get; set; } = false;

        [JsonProperty("get_tx_keys")]
        public bool GetTxKeys { get; set; } = false;
    }

    [JsonObject]
    public class SignTransferResponseData
    {
        [JsonProperty("signed_txset")]
        public string SignedTxSet { get; set; } = string.Empty;

        [JsonProperty("tx_hash_list")]
        public List<string> TxHashList { get; set; } = new List<string>();

        [JsonProperty("tx_raw_list")]
        public List<string> TxRawList { get; set; } = new List<string>();

        [JsonProperty("tx_key_list")]
        public List<string> TxKeyList { get; set; } = new List<string>();
    }
}
