using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetAccounts : Request<GetAccountsRequestData, GetAccountsResponseData>
    {
        public GetAccounts(GetAccountsRequestData rpcData, Action<GetAccountsResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetAccountsResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_accounts", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetAccountsResponseData>>(json).Result : null;
            return r;
        }
    }

    public class GetAccountsRequestData
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }

    [JsonObject]
    public class GetAccountsResponseData
    {
        [JsonProperty("subaddress_accounts")]
        public List<SubAddressAccount> Accounts { get; set; }

        [JsonProperty("total_balance")]
        public ulong TotalBalance { get; set; }

        [JsonProperty("total_unlocked_balance")]
        public ulong TotalUnlockedBalance { get; set; }
    }

    [JsonObject]
    public class SubAddressAccount
    {
        [JsonProperty("account_index")]
        public uint Index { get; set; }

        [JsonProperty("balance")]
        public ulong Balance { get; set; }

        [JsonProperty("base_address")]
        public string BaseAddress { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("unlocked_balance")]
        public ulong UnlockedBalance { get; set; }
    }
}