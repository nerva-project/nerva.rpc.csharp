using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class GetConnections : Request<object, List<GetConnectionsResponseData>>
    {
        public GetConnections(Action<List<GetConnectionsResponseData>> completeAction, Action<RequestError> failedAction, 
            string host = Config.DEFAULT_HOST, uint port = Config.DEFAULT_PORT, Log log = null)
            : base (null, completeAction, failedAction, host, port, log) { }
            
        protected override bool DoRequest(out List<GetConnectionsResponseData> result)
        {
            string json = null;
            bool r = JsonRpcRequest("get_connections", rpcData, out json);
            result = r ? JsonConvert.DeserializeObject<ResponseData<GetConnectionsResponseDataTemp>>(json).Result.Connections : null;
            return r;
        }
    }

    [JsonObject]
    class GetConnectionsResponseDataTemp
    {
        [JsonProperty("connections")]
        public List<GetConnectionsResponseData> Connections { get; set; }
    }

    [JsonObject]
    public class GetConnectionsResponseData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("avg_download")]
        public uint AvgDownload { get; set; }

        [JsonProperty("avg_upload")]
        public uint AvgUpload { get; set; }

        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("current_download")]
        public uint CurrentDownload { get; set; }

        [JsonProperty("current_upload")]
        public uint CurrentUpload { get; set; }

        [JsonProperty("height")]
        public uint Height { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("incoming")]
        public bool Incoming { get; set; }

        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("live_time")]
        public ulong LiveTime { get; set; }

        [JsonProperty("local_ip")]
        public bool LocalIP { get; set; }

        [JsonProperty("localhost")]
        public bool Localhost { get; set; }

        [JsonProperty("peer_id")]
        public string PeerId { get; set; }

        [JsonProperty("port")]
        public uint Port { get; set; }

        [JsonProperty("recv_count")]
        public uint RecvCount { get; set; }

        [JsonProperty("recv_idle_time")]
        public uint RecvIdleTime { get; set; }

        [JsonProperty("send_count")]
        public uint SendCount { get; set; }

        [JsonProperty("send_idle_time")]
        public uint SendIdleTime { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("support_flags")]
        public uint SupportFlags { get; set; }
    }
}