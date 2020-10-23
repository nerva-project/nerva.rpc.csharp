using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class GetAccountTags : Request<object, GetAccountTagsResponseData>
    {
        public GetAccountTags(Action<GetAccountTagsResponseData> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_WALLET_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out GetAccountTagsResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_account_tags", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetAccountTagsResponseData>>(json).Result : null;
            return r;
        }
    }

    public class GetAccountTagsResponseData
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("accounts")]
        public List<uint> AccountIndices { get; set; }
    }
}