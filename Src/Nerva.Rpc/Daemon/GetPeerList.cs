using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class GetPeerList : Request<object, GetPeerListResponseData>
    {
        public GetPeerList(StartMiningRequestData rpcData, Action<GetPeerListResponseData> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
            : base (rpcData, completeAction, failedAction, port, log) { }

        protected override bool DoRequest(out GetPeerListResponseData result)
        {
            string json = null;
            bool r = RpcRequest("get_peer_list", null, out json);
            result = r ? JsonConvert.DeserializeObject<GetPeerListResponseData>(json) : null;
            return r;
        }
    }

    [JsonObject]
    public class GetPeerListResponseData
    {
        [JsonProperty("white_list")]
        public List<GetPeerListResponseDataItem> WhiteList { get; set; }

        [JsonProperty("gray_list")]
        public List<GetPeerListResponseDataItem> GrayList { get; set; }
    }

    [JsonObject]
    public class GetPeerListResponseDataItem
    {
        [JsonProperty("host")]
        public ulong Host { get; set; } = 0;

        [JsonProperty("id")]
        public ulong ID { get; set; } = 0;

        [JsonProperty("ip")]
        public uint IP { get; set; } = 0;

        [JsonProperty("last_seen")]
        public ulong LastSeen { get; set; } = 1;

        [JsonProperty("port")]
        public uint Port { get; set; } = 0;
    }
}