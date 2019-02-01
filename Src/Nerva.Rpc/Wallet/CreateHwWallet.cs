using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class CreateHwWallet : Request<CreateHwWalletRequestData, CreateHwWalletResponseData>
    {
        public CreateHwWallet(CreateHwWalletRequestData rpcData, Action<CreateHwWalletResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, port, log) { }
            
        protected override bool DoRequest(out CreateHwWalletResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("create_hw_wallet", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<CreateHwWalletResponseData>>(json).Result : null;
            return r;
        }
    }

    public class CreateHwWalletRequestData : OpenWalletRequestData
    {
        private const string DEVICE_NAME = "Ledger";

        [JsonProperty("device_name")]
        public string DeviceName => DEVICE_NAME;
    }

    public class CreateHwWalletResponseData
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}