using System;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class CreateWallet : Request<CreateWalletRequestData, CreateWalletResponseData>
    {
        public CreateWallet(CreateWalletRequestData rpcData, Action<CreateWalletResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out CreateWalletResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("create_wallet", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<CreateWalletResponseData>>(json).Result : null;
            return r;
        }
    }

    public class CreateWalletRequestData : OpenWalletRequestData
    {
        private const string LANGUAGE = "English";

        [JsonProperty("language")]
        public string Language => LANGUAGE;
    }

    public class CreateWalletResponseData
    {
        [JsonProperty("seed")]
        public string Seed { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}