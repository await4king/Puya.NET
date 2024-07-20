using Newtonsoft.Json;
using Puya.Service;

namespace Puya.Api
{
    public class NewtonsoftJsonResponseSerializer : IApiResponseSerializer
    {
        public string Serialize(ServiceResponse response)
        {
            var result = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return result;
        }
    }
}
