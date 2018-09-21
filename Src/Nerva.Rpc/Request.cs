using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AngryWasp.Logger;

namespace Nerva.Rpc
{
    public abstract class RpcRequest<T_Req, T_Resp>
    {
        protected RequestError e;
        protected RequestData d;
        protected T_Req rpcData;
        protected Action<T_Resp> completeAction;
        protected Action failedAction;

        public RequestError Error => e;
        public RequestData Data => d;

        public RpcRequest(T_Req rpcData, Action<T_Resp> completeAction, Action failedAction, uint port = 17566)
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
            await Task.Run( () =>
            {
                T_Resp result;
                if (DoRequest(out result))
                    completeAction(result);
                else
                    failedAction();
            });
        }

        protected bool BasicRequest(string rpc, T_Req param, out string result)
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

            if (HasError(result))
            {
                Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", e.Code, e.Message);
                return false;
            }

            return true;
        }

        protected bool HasError(string result)
        {
            var error = JObject.Parse(result)["error"];

            if (error != null)
            {
                e.Code = error["code"].Value<int>();
                e.Message = error["message"].Value<string>();
                
                return true;
            }

            return false;
        }
    }

    /*public abstract class Request<T> : Request
    {
        protected T rpcData;

        public Request(T rpcData, Action<string> completeAction, Action failedAction)
            : base (completeAction, failedAction)
            {
                this.rpcData = rpcData;
            }

        protected bool BasicRequest(string rpc, T param, out string result)
        {
            result = null;

            JsonRequest<T> jr = new JsonRequest<T>
            {
                MethodName = rpc,
                Params = param
            };

            if (!new Requester(d).MakeJsonRpcRequest(jr, out result))
            {
                Log.Instance.Write(Log_Severity.Error, "Could not complete JSON RPC call: {0}", jr.MethodName);
                return false;
            }

            if (HasError(result))
            {
                Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", e.Code, e.Message);
                return false;
            }

            return true;
        }

    }

    public abstract class Request
    {
        protected RequestError e;
        protected RequestData d;
        protected Action<string> completeAction;
        protected Action failedAction;

        public RequestError Error => e;

        public Request(Action<string> completeAction, Action failedAction)
        {
            e = new RequestError();
            d = new RequestData();

            this.completeAction = completeAction;
            this.failedAction = failedAction;
        }

        protected abstract bool SendAction(out string result);

        public void Run()
        {
            Task.Run( () =>
            {
                string result;
                if (SendAction(out result))
                    completeAction(result);
                else
                    failedAction();
            });
        }

        protected bool BasicRequest(string rpc)
        {
            string result = null;
            return BasicRequest(rpc, out result);
        }

        protected bool BasicRequest(string rpc, out string result)
        {
            result = null;

            JsonRequest jr = new JsonRequest
            {
                MethodName = rpc
            };

            if (!new Requester(d).MakeJsonRpcRequest(jr, out result))
            {
                Log.Instance.Write(Log_Severity.Error, "Could not complete JSON RPC call: {0}", jr.MethodName);
                return false;
            }

            if (HasError(result))
            {
                Log.Instance.Write(Log_Severity.Error, "JSON RPC call returned error {0}: {1}", e.Code, e.Message);
                return false;
            }

            return true;
        }

        protected bool HasError(string result)
        {
            var error = JObject.Parse(result)["error"];

            if (error != null)
            {
                e.Code = error["code"].Value<int>();
                e.Message = error["message"].Value<string>();
                
                return true;
            }

            return false;
        }
    }*/
    public class RequestError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}