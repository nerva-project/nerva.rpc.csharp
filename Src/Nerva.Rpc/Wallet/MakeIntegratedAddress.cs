using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class MakeIntegratedAddress : Request<MakeIntegratedAddressRequestData, MakeIntegratedAddressResponseData>
    {
        public MakeIntegratedAddress(MakeIntegratedAddressRequestData rpcData, Action<MakeIntegratedAddressResponseData> completeAction, Action<RequestError> failedAction,
            string host = "http://127.0.0.1", uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out MakeIntegratedAddressResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("make_integrated_address", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<MakeIntegratedAddressResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class MakeIntegratedAddressRequestData
    {
        [JsonProperty("standard_address")]
        public string StandardAddress { get; set; } = string.Empty;

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;
    }

    [JsonObject]
    public class MakeIntegratedAddressResponseData
    {
        [JsonProperty("integrated_address")]
        public string IntegratedAddress { get; set; } = string.Empty;

        [JsonProperty("payment_id")]
        public string PaymentId { get; set; } = string.Empty;
    }
}