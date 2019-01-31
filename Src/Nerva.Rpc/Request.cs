using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AngryWasp.Logger;
using System.Net;
using System.Collections.Generic;

namespace Nerva.Rpc
{
    public abstract class Request<T_Req, T_Resp>
    {
        protected RequestError error;
        protected Log log;
        protected uint port;
        protected T_Req rpcData;
        protected Action<T_Resp> completeAction;
        protected Action<RequestError> failedAction;

        public Request(T_Req rpcData, Action<T_Resp> completeAction, Action<RequestError> failedAction, uint port = 17566, Log log = null)
        {
            this.error = new RequestError();
            this.log = (log == null) ? Log.Presets.Normal : log;
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

            if (!new Requester(port).MakeRpcRequest(methodName, postData, log, out result))
                return false;

            var status = JObject.Parse(result)["status"].Value<string>();
            var ok = status.ToLower() == "ok";

            string paramData = !string.IsNullOrEmpty(postData) ? postData : "None";

            

            bool hasError = false;

            if (!ok)
            {
                error.Code = 0; //no error code provided for rpc methods
                error.Message = status;
                hasError = true;
            }

            if (hasError)
            {
                if (log.LogRpcErrors)
                    AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, $"{methodName} returned error {error.Code}: {error.Message}");

                if (log.OnlyLogRpcOnError)
                {
                    if (log.LogRpcRequest)
                        AngryWasp.Logger.Log.Instance.Write($"{methodName} request: {paramData}");

                    if (log.LogRpcResponse)
                        AngryWasp.Logger.Log.Instance.Write($"{methodName} response: {(string.IsNullOrEmpty(result) ? "None" : result)}");
                }

                return false;
            }
            else
            {
                if (!log.OnlyLogRpcOnError)
                {
                    if (log.LogRpcRequest)
                        AngryWasp.Logger.Log.Instance.Write($"{methodName} request: {paramData}");

                    if (log.LogRpcResponse)
                        AngryWasp.Logger.Log.Instance.Write($"{methodName} response: {(string.IsNullOrEmpty(result) ? "None" : result)}");
                }

                return true;
            }
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

            string paramData = param != null ? jr.Encode() : "none";

            if (!new Requester(port).MakeJsonRpcRequest(jr, log, out result))
                return false;

            var e = JObject.Parse(result)["error"];

            if (e != null)
            {
                error.Code = e["code"].Value<int>();
                error.Message = e["message"].Value<string>();

                if (log.SuppressRpcCodes.Contains(error.Code))
                    return false;

                if (log.LogRpcErrors)
                    AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, $"{rpc} returned error {error.Code}: {error.Message}");

                if (log.OnlyLogRpcOnError)
                {
                    if (log.LogRpcRequest)
                        AngryWasp.Logger.Log.Instance.Write($"{rpc} request: {paramData}");

                    if (log.LogRpcResponse)
                        AngryWasp.Logger.Log.Instance.Write($"{rpc} response: {(string.IsNullOrEmpty(result) ? "None" : result)}");
                }

                return false;
            }
            else
            {
                if (!log.OnlyLogRpcOnError)
                {
                    if (log.LogRpcRequest)
                        AngryWasp.Logger.Log.Instance.Write($"{rpc} request: {paramData}");

                    if (log.LogRpcResponse)
                        AngryWasp.Logger.Log.Instance.Write($"{rpc} response: {(string.IsNullOrEmpty(result) ? "None" : result)}");
                }

                return true;
            }
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

        [JsonIgnore]
        public virtual bool HasParams => false;

        [JsonProperty("jsonrpc")]
        public string RpcVersion => RPC_VERSION;

        [JsonProperty("id")]
        public string RpcId => ID;
        
        [JsonProperty("method")]
        public string MethodName { get; set; }

        public string Encode() => JsonConvert.SerializeObject(this);

        public virtual string GetParamsJson() => null;
    }

    [JsonObject]
    public class RequestData<T> : RequestData
    {  
        [JsonProperty("params")]
        public T Params { get; set; }

        public override bool HasParams => true;

        public override string GetParamsJson() => JsonConvert.SerializeObject(Params);
    }

    [JsonObject]
    public class ResponseData<T>
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}