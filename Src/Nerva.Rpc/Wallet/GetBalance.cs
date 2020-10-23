using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetBalance : Request<GetBalanceRequestData, GetBalanceResponseData>
    {
        public GetBalance(GetBalanceRequestData rpcData, Action<GetBalanceResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetBalanceResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_balance", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetBalanceResponseData>>(json).Result : null;
            return r;
        }
    }

    public class GetBalanceRequestData
    {
        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; }

        [JsonProperty("address_indices")]
        public List<uint> AddressIndices { get; set; }
    }

    public class GetBalanceResponseData
    {
        [JsonProperty("balance")]
        public ulong Balance { get; set; }

        [JsonProperty("unlocked_balance")]
        public ulong UnlockedBalance { get; set; }

        [JsonProperty("multisig_import_needed")]
        public bool MultisigImportNeeded { get; set; }

        [JsonProperty("per_subaddress")]
        public List<SubAddressBalance> PerSubaddress { get; set; }
    }

    public class SubAddressBalance
    {
        [JsonProperty("address_index")]
        public uint AddressIndex { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("balance")]
        public ulong Balance { get; set; }

        [JsonProperty("unlocked_balance")]
        public ulong UnlockedBalance { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("num_unspent_outputs")]
        public uint NumUnspentOutputs { get; set; }
    }
}