using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc.Daemon
{
    public class GetConnections : Request<object, List<GetConnectionsResponseData>>
    {
        public GetConnections(Action<List<GetConnectionsResponseData>> completeAction, Action<RequestError> failedAction, uint port = 17566)
            : base (null, completeAction, failedAction, port) { }
            
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
        public int AvgDownload { get; set; }

        [JsonProperty("avg_upload")]
        public int AvgUpload { get; set; }

        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("current_download")]
        public int CurrentDownload { get; set; }

        [JsonProperty("current_upload")]
        public int CurrentUpload { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("incoming")]
        public bool Incoming { get; set; }

        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("live_time")]
        public long LiveTime { get; set; }

        [JsonProperty("local_ip")]
        public bool LocalIP { get; set; }

        [JsonProperty("localhost")]
        public bool Localhost { get; set; }

        [JsonProperty("peer_id")]
        public string PeerId { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("recv_count")]
        public int RecvCount { get; set; }

        [JsonProperty("recv_idle_time")]
        public int RecvIdleTime { get; set; }

        [JsonProperty("send_count")]
        public int SendCount { get; set; }

        [JsonProperty("send_idle_time")]
        public int SendIdleTime { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("support_flags")]
        public int SupportFlags { get; set; }
    }
}