using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responder;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway
{
    public class CustomResponseMiddleware : OcelotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpResponder _responder;
        private readonly IErrorsToHttpStatusCodeMapper _codeMapper;

        public CustomResponseMiddleware(
            RequestDelegate next,
            IHttpResponder responder,
            IErrorsToHttpStatusCodeMapper codeMapper,
            IOcelotLoggerFactory loggerFactory)
        : base(loggerFactory.CreateLogger<CustomResponseMiddleware>())
        {
            _next = next;
            _responder = responder;
            _codeMapper = codeMapper;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next.Invoke(httpContext);
            if (httpContext.Response.HasStarted)
                return;
            
            var errors = httpContext.Items.Errors();
            if (errors.Count > 0)
            {
                var statusCode = _codeMapper.Map(errors);
                var error = string.Join(",", errors.Select(x => x.Message));
                httpContext.Response.StatusCode = statusCode;
                // output error
                await httpContext.Response.WriteAsync(error);
            }
            else
            {
                var downstreamResponse = httpContext.Items.DownstreamResponse();
                await _responder.SetResponseOnHttpContext(httpContext, downstreamResponse);
            }
        }
    }
}