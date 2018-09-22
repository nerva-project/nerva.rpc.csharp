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
        protected RequestError e;
        protected RequestData d;
        protected T_Req rpcData;
        protected Action<T_Resp> completeAction;
        protected Action<RequestError> failedAction;

        public RequestError Error => e;
        public RequestData Data => d;

        public Request(T_Req rpcData, Action<T_Resp> completeAction, Action<RequestError> failedAction, uint port = 17566)
        {
            e = new RequestError();
            d = new RequestData(port);

            this.rpcData = rpcData;
            this.completeAction = completeAction;
            this.failedAction = failedAction;
        }

        protected abstract bool DoRequest(out T_Resp result);

        public async Task Run()
        {
            await Task.Run( () => {
                T_Resp result;
                if (DoRequest(out result))
                    completeAction?.Invoke(result);
                else
                    failedAction?.Invoke(e);
            });
        }

        protected bool RpcRequest(string methodName, string postData, out string result)
        {
            result = null;

            if (!new Requester(d).MakeRpcRequest(methodName, postData, out result))
            {
                Log.Instance.Write(Log_Severity.Error, "Could not complete JSON RPC call: {0}", methodName);
                return false;
            }

            var status = JObject.Parse(result)["status"].Value<string>();
            var ok = status.ToLower() == "ok";

            if (!ok)
            {
                e.Code = 0; //no error code provided for rpc methods
                e.Message = status;
                Log.Instance.Write(Log_Severity.Error, "RPC call returned error {0}: {1}", e.Code, e.Message);
                return false;
            }

            return true;
        }

        protected bool JsonRpcRequest(string rpc, T_Req param, out string result)
        {
            result = null;

            var jr = param == null ?
                new JsonRequest {
                    MethodName = rpc
                } :
                new JsonRequest<T_Req> {
                    MethodName = rpc,
                    Params = param
                };

            if (!new Requester(d).MakeJsonRpcRequest(jr, out result))
            {
                Log.Instance.Write(Log_Severity.Error, "Could not complete JSON RPC call: {0}", jr.MethodName);
                return false;
            }

            var error = JObject.Parse(result)["error"];

            if (error != null)
            {
                e.Code = error["code"].Value<int>();
                e.Message = error["message"].Value<string>();

                Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", e.Code, e.Message);
                
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
}