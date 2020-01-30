using System;
using System.IO;
using System.Net;
using System.Text;
using AngryWasp.Logger;

namespace Nerva.Rpc
{
    public class Requester
    {
        private string host;
        private uint port;

        public Requester(string host, uint port)
        {
            this.host = host;
            this.port = port;
        }

        public bool MakeJsonRpcRequest(RequestData request, Log log, ref RequestError error, out string jsonString)
        {
            try
            {
                string url = $"http://{host}:{port}/json_rpc";

                string reqData = request.Encode();
                byte[] reqDataBytes = Encoding.ASCII.GetBytes(reqData);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";

                using (Stream stream = req.GetRequestStream())
                    stream.Write(reqDataBytes, 0, reqDataBytes.Length);

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (Stream stream = resp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }

                return true;
            }
            catch (WebException)
            {
                jsonString = null;
                error.Code = int.MaxValue;
                error.Message = "Error making RPC connection";
                return false;
            }
            catch (Exception ex)
            {
                if (log.LogNetworkErrors)
                {
                    AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {request.MethodName}");
                    string paramData = request.GetParamsJson();
                    if (log.LogRpcRequest && paramData != null)
                        AngryWasp.Logger.Log.Instance.Write(Log_Severity.None, $"{request.MethodName} params: {paramData}");

                    AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, $".NET Exception, {ex.Message}");
                }

                jsonString = null;
                return false;
            }
        }

        public bool MakeRpcRequest(string methodName, string postDataString, Log log, ref RequestError error, out string jsonString)
        {
            try
            {
                string url = $"http://{host}:{port}/{methodName}";
   
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";

                if (!string.IsNullOrEmpty(postDataString))
                {
                    byte[] postData = Encoding.ASCII.GetBytes(postDataString);

                    using (Stream stream = req.GetRequestStream())
                        stream.Write(postData, 0, postData.Length);
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (Stream stream = resp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }

                return true;
            }
            catch (WebException)
            {
                jsonString = null;
                error.Code = int.MaxValue;
                error.Message = "Error making RPC connection";
                return false;
            }
            catch (Exception ex)
            {
                if (log.LogNetworkErrors)
                {
                    AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {methodName}");
                    if (log.LogRpcRequest && !string.IsNullOrEmpty(postDataString))
                        AngryWasp.Logger.Log.Instance.Write(Log_Severity.None, $"{methodName} params: {postDataString}");

                    AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, $".NET Exception, {ex.Message}");
                }

                jsonString = null;
                return false;
            }
        }
    }
}