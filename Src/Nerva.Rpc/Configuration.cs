using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nerva.Rpc
{
    public enum Error_Log_Verbosity
    {
        //no messages
        None,
        //show a generic error message
        Normal, 
        //show normal + request data
        Detailed, 
        //show detailed + exceptions
        Full
    }

    public static class Configuration
    {
        public static bool TraceRpcData { get; set; } = false;

        public static Error_Log_Verbosity ErrorLogVerbosity { get; set; } = Error_Log_Verbosity.None;

        public static List<int> SuppressRpcCodes { get; set; } = new List<int>();
    }
}