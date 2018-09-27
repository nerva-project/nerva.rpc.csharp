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
                return false;

            var status = JObject.Parse(result)["status"].Value<string>();
            var ok = status.ToLower() == "ok";

            if (!ok)
            {
                error.Code = 0; //no error code provided for rpc methods
                error.Message = status;

                switch (Configuration.ErrorLogVerbosity)
                {
                    case Error_Log_Verbosity.Normal:
                        Log.Instance.Write(Log_Severity.Error, "RPC call returned error {0}: {1}", error.Code, error.Message);
                        break;
                    case Error_Log_Verbosity.Detailed:
                        Log.Instance.Write(Log_Severity.Error, "RPC call returned error {0}: {1}", error.Code, error.Message);
                        Log.Instance.Write(Log_Severity.None, "RPC Params: {0}", postData);
                        break;
                    case Error_Log_Verbosity.Full:
                        Log.Instance.Write(Log_Severity.Error, "RPC call returned error {0}: {1}", error.Code, error.Message);
                        Log.Instance.Write(Log_Severity.None, "RPC Params: {0}", postData);
                        Log.Instance.Write(Log_Severity.None, "RPC Response: {0}", string.IsNullOrEmpty(result) ? "None" : result);
                        break;
                }

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
                return false;

            var e = JObject.Parse(result)["error"];

            if (e != null)
            {
                error.Code = e["code"].Value<int>();
                error.Message = e["message"].Value<string>();

                string paramData = param != null ? jr.GetParamsJson() : "None";

                switch (Configuration.ErrorLogVerbosity)
                {
                    case Error_Log_Verbosity.Normal:
                        Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", error.Code, error.Message);
                        break;
                    case Error_Log_Verbosity.Detailed:
                        Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", error.Code, error.Message);
                        Log.Instance.Write(Log_Severity.None, "JSON RPC Params: {0}", paramData);
                        break;
                    case Error_Log_Verbosity.Full:
                        Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", error.Code, error.Message);
                        Log.Instance.Write(Log_Severity.None, "JSON RPC Params: {0}", paramData);
                        Log.Instance.Write(Log_Severity.None, "JSON RPC Response:\r\n{0}", string.IsNullOrEmpty(result) ? "None" : result);
                        break;
                }
                
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