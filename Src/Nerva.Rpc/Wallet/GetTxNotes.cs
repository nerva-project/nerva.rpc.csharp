using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetTxNotes : Request<GetTxNotesRequestData, GetTxNotesResponseData>
    {
        public GetTxNotes(GetTxNotesRequestData rpcData, Action<GetTxNotesResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetTxNotesResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_tx_notes", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetTxNotesResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class GetTxNotesRequestData
    {
        [JsonProperty("txids")]
        public List<string> TxIds { get; set; } = new List<string>();
    }

    [JsonObject]
    public class GetTxNotesResponseData
    {
        [JsonProperty("notes")]
        public List<string> Notes { get; set; } = new List<string>();
    }
}