using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetAddress : Request<GetAddressRequestData, GetAddressResponseData>
    {
        public GetAddress(GetAddressRequestData rpcData, Action<GetAddressResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetAddressResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_address", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetAddressResponseData>>(json).Result : null;
            return r;
        }
    }

    public class GetAddressRequestData
    {
        [JsonProperty("account_index")]
        public uint AccountIndex { get; set; }

        [JsonProperty("address_index")]
        public List<uint> AddressIndices { get; set; }
    }

    public class GetAddressResponseData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("addresses")]
        public List<AddressesItem> Addresses { get; set; }
    }

    public class AddressesItem
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("address_index")]
        public uint AddressIndex { get; set; }
    }
}