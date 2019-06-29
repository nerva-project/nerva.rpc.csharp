using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nerva.Rpc.Wallet
{
    public class QueryKey : Request<QueryKeyRequestData, QueryKeyResponseData>
    {
        public QueryKey(QueryKeyRequestData rpcData, Action<QueryKeyResponseData> completeAction, Action<RequestError> failedAction,
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (rpcData, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out QueryKeyResponseData result)
        {
            string json = null;
            bool r = JsonRpcRequest("query_key", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<QueryKeyResponseData>>(json).Result : null;
            return r;
        }
    }

    public enum Key_Type
    {
        Secret_View_Key,
        Secret_Spend_Key,
        Public_View_Key,
        Public_Spend_Key,
        All_Keys,
        Mnemonic
    }

    [JsonObject]
    public class QueryKeyRequestData
    {
        [JsonProperty("key_type")]
        public string KeyType { get; set; } = Key_Type.All_Keys.ToString().ToLower();
    }

    [JsonObject]
    public class QueryKeyResponseData
    {
        [JsonProperty("public_view_key")]
        public string PublicViewKey { get; set; } = string.Empty;

        [JsonProperty("public_spend_key")]
        public string PublicSpendKey { get; set; } = string.Empty;

        [JsonProperty("private_view_key")]
        public string PrivateViewKey { get; set; } = string.Empty;

        [JsonProperty("private_spend_key")]
        public string PrivateSpendKey { get; set; } = string.Empty;

        [JsonProperty("mnemonic")]
        public string MnemonicSeed { get; set; } = string.Empty;
    }
}