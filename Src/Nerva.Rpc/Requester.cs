using System;
using System.IO;
using System.Net;
using System.Text;
using AngryWasp.Logger;

namespace Nerva.Rpc
{
    public class RequestData
    {
        public uint Port { get; set; } = 0;
    }

    public class Requester
    {
        private RequestData rd;

        public Requester(RequestData rd)
        {
            this.rd = rd;
        }

        public bool MakeJsonRpcRequest(JsonRequest request, out string jsonString)
        {
            try
            {
                string url = $"http://127.0.0.1:{rd.Port}/json_rpc";

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
                Log.Instance.WriteNonFatalException(ex);

                jsonString = null;
                return false;
            }
        }

        public bool MakeRpcRequest(string methodName, string postDataString, out string jsonString)
        {
            try
            {
                string url = $"http://127.0.0.1:{rd.Port}/{methodName}";
   
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
                Log.Instance.WriteNonFatalException(ex);
                    
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
                Log.Instance.WriteNonFatalException(ex);
                
                returnString = null;
                return false;
            }
        }
    }
}