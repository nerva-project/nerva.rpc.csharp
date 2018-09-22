using Newtonsoft.Json;

namespace Nerva.Rpc
{
    [JsonObject]
    public class JsonResponse<T>
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}