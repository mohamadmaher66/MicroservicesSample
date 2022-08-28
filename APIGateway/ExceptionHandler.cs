using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace APIGateway
{
    public class ExceptionHandler
    {
        public static async void HandleException(HttpContext context)
        {
            var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = feature.Error;

            var result = JsonConvert.SerializeObject(exception.Message, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("access-control-allow-origin", "*");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(result);
        }
    }
}
