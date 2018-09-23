using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AngryWasp.Logger;
using System.Net;

namespace Nerva.Rpc
{
    public abstract class Request<T_Req, T_Resp>
    {
        protected RequestError error;
        protected uint port;
        protected T_Req rpcData;
        protected Action<T_Resp> completeAction;
        protected Action<RequestError> failedAction;

        public Request(T_Req rpcData, Action<T_Resp> completeAction, Action<RequestError> failedAction, uint port = 17566)
        {
            error = new RequestError();
            this.port = port;

            this.rpcData = rpcData;
            this.completeAction = completeAction;
            this.failedAction = failedAction;
        }

        protected abstract bool DoRequest(out T_Resp result);

        public async Task RunAsync()
        {
            await Task.Run( () => {
                T_Resp result;
                if (DoRequest(out result))
                    completeAction?.Invoke(result);
                else
                    failedAction?.Invoke(error);
            });
        }

        public bool Run()
        {
            T_Resp result;
            bool ok = DoRequest(out result);

            if (ok)
                completeAction?.Invoke(result);
            else
                failedAction?.Invoke(error);

            return ok;
        }

        protected bool RpcRequest(string methodName, string postData, out string result)
        {
            result = null;

            if (!new Requester(port).MakeRpcRequest(methodName, postData, out result))
            {
                if (Configuration.LogErrors)
                    Log.Instance.Write(Log_Severity.Error, "Could not complete JSON RPC call: {0}", methodName);

                return false;
            }

            var status = JObject.Parse(result)["status"].Value<string>();
            var ok = status.ToLower() == "ok";

            if (!ok)
            {
                error.Code = 0; //no error code provided for rpc methods
                error.Message = status;

                if (Configuration.LogErrors)
                    Log.Instance.Write(Log_Severity.Error, "RPC call returned error {0}: {1}", error.Code, error.Message);

                return false;
            }

            return true;
        }

        protected bool JsonRpcRequest(string rpc, T_Req param, out string result)
        {
            result = null;

            var jr = param == null ?
                new RequestData {
                    MethodName = rpc
                } :
                new RequestData<T_Req> {
                    MethodName = rpc,
                    Params = param
                };

            if (!new Requester(port).MakeJsonRpcRequest(jr, out result))
            {
                if (Configuration.LogErrors)
                    Log.Instance.Write(Log_Severity.Error, "Could not complete JSON RPC call: {0}", jr.MethodName);

                return false;
            }

            var e = JObject.Parse(result)["error"];

            if (e != null)
            {
                error.Code = e["code"].Value<int>();
                error.Message = e["message"].Value<string>();

                if (Configuration.LogErrors)
                    Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", error.Code, error.Message);
                
                return false;
            }

            return true;
        }
    }

    public class RequestError
    {
        public int Code { get; set; } = int.MaxValue;
        public string Message { get; set; } = null;
    }

    [JsonObject]
    public class RequestData
    { 
        private const string RPC_VERSION = "2.0";
        private const string ID = "0";

        [JsonProperty("jsonrpc")]
        public string RpcVersion => RPC_VERSION;

        [JsonProperty("id")]
        public string RpcId => ID;
        
        [JsonProperty("method")]
        public string MethodName { get; set; }

        public string Encode() => JsonConvert.SerializeObject(this);
    }

    [JsonObject]
    public class RequestData<T> : RequestData
    {  
        [JsonProperty("params")]
        public T Params { get; set; }
    }

    [JsonObject]
    public class ResponseData<T>
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}