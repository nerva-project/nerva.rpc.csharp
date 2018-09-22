using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class CreateAccount : Request<CreateAccountRequestData, CreateAccountResponseData>
    {
        public CreateAccount(CreateAccountRequestData rpcData, Action<CreateAccountResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (rpcData, completeAction, failedAction, port) { }
            
        protected override bool DoRequest(out CreateAccountResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("create_account", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<JsonResponse<CreateAccountResponseData>>(json).Result : null;
            return r;
        }
    }

    [JsonObject]
    public class CreateAccountRequestData
    {
        [JsonProperty("label")]
        public string Label { get; set; } = string.Empty;
    }

    [JsonObject]
    public class CreateAccountResponseData
    {
        [JsonProperty("account_index")]
        public uint Index { get; set; } = uint.MaxValue;
        
        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;
    }
}