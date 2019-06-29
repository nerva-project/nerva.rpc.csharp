using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class SplitIntegratedAddress : Request<SplitIntegratedAddressRequestData, SplitIntegratedAddressResponseData>
    {
        public SplitIntegratedAddress(SplitIntegratedAddressRequestData rpcData, Action<SplitIntegratedAddressResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out SplitIntegratedAddressResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("split_integrated_address", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<SplitIntegratedAddressResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class SplitIntegratedAddressRequestData
    {
        [JsonProperty("integrated_address")]
        public string IntegratedAddress { get; set; } = string.Empty;
    }

    [JsonObject]
    public class SplitIntegratedAddressResponseData
    {
        [JsonProperty("standard_address")]
        public string StandardAddress { get; set; } = string.Empty;

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;

        [JsonProperty("is_subaddress")]
        public bool IsSubAddress { get; set; } = false;
    }
}