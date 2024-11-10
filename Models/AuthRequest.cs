using Newtonsoft.Json;

namespace child13.Models
{
    public class AuthRequest
    {
        [JsonProperty("openai_key")]
        public string Openai_key { get; set; }
    }
}
