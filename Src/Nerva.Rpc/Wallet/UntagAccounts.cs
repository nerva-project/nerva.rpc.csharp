using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class UntagAccounts : Request<UntagAccountsRequestData, string>
    {
        public UntagAccounts(UntagAccountsRequestData rpcData, Action<string> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out string result)
        {
            bool r = JsonRpcRequest("untag_accounts", rpcData, out result);
            return r;
        }
    }

    public class UntagAccountsRequestData
    {
        [JsonProperty("accounts")]
        public List<uint> AccountIndices { get; set; }
    }
}