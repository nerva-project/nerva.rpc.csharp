using System;
using System.IO;
using System.Net;
using System.Text;
using AngryWasp.Logger;

namespace Nerva.Rpc
{
    public class Requester
    {
        private uint port;

        public Requester(uint port)
        {
            this.port = port;
        }

        public bool MakeJsonRpcRequest(RequestData request, out string jsonString)
        {
            try
            {
                string url = $"http://127.0.0.1:{port}/json_rpc";

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
            catch (Exception ex)
            {
                string paramData = request.GetParamsJson();
                if (paramData == null) 
                    paramData = "None";

                switch (Configuration.ErrorLogVerbosity)
                {
                    case Error_Log_Verbosity.Normal:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {request.MethodName}");
                        break;
                    case Error_Log_Verbosity.Detailed:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {request.MethodName}");
                        Log.Instance.Write(Log_Severity.None, $"JSON RPC Params: {paramData}");
                        break;
                    case Error_Log_Verbosity.Full:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {request.MethodName}");
                        Log.Instance.Write(Log_Severity.None, $"JSON RPC Params: {paramData}");
                        Log.Instance.WriteNonFatalException(ex);
                        break;
                }

                jsonString = null;
                return false;
            }
        }

        public bool MakeRpcRequest(string methodName, string postDataString, out string jsonString)
        {
            try
            {
                string url = $"http://127.0.0.1:{port}/{methodName}";
   
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
            catch (Exception ex)
            {
                switch (Configuration.ErrorLogVerbosity)
                {
                    case Error_Log_Verbosity.Normal:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {methodName}");
                        break;
                    case Error_Log_Verbosity.Detailed:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {methodName}");
                        Log.Instance.Write(Log_Severity.None, $"RPC Params: {postDataString}");
                        break;
                    case Error_Log_Verbosity.Full:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete JSON RPC call: {methodName}");
                        Log.Instance.Write(Log_Severity.None, $"RPC Params: {postDataString}");
                        Log.Instance.WriteNonFatalException(ex);
                        break;
                }

                jsonString = null;
                return false;
            }
        }

        public static bool MakeHttpRequest(string url, out string returnString)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (Stream stream = resp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    returnString = reader.ReadToEnd();
                }

                return true;
            }
            catch (Exception ex)
            {
               switch (Configuration.ErrorLogVerbosity)
                {
                    case Error_Log_Verbosity.Normal:
                    case Error_Log_Verbosity.Detailed:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete HTTP call: {url}");
                        break;
                    case Error_Log_Verbosity.Full:
                        Log.Instance.Write(Log_Severity.Error, $"Could not complete HTTP call: {url}");
                        Log.Instance.WriteNonFatalException(ex);
                        break;
                }
                
                returnString = null;
                return false;
            }
        }
    }
}