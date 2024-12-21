using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Template.CrossCutting.ExceptionHandler.ViewModels
{
    internal class ExceptionViewModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
        }
    }
}
