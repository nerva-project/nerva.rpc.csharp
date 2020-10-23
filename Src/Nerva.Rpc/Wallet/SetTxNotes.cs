using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class SetTxNotes : Request<SetTxNotesRequestData, string>
    {
        public SetTxNotes(SetTxNotesRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out string result) => JsonRpcRequest("set_tx_notes", rpcData, out result);
    }

    [JsonObject]
    public class SetTxNotesRequestData
    {
        [JsonProperty("txids")]
        public List<string> TxIds { get; set; } = new List<string>();

        [JsonProperty("notes")]
        public List<string> Notes { get; set; } = new List<string>();
    }
}