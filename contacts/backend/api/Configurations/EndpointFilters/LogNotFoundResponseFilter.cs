using System.Net;

namespace Contacts.Api.Configurations.EndpointFilters
{
    public class LogNotFoundResponseFilter : IEndpointFilter
    {
        private readonly ILogger<LogNotFoundResponseFilter> _logger;

        public LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next.Invoke(context);

            var actualResult = (result is INestedHttpResult) ? ((INestedHttpResult)result).Result : (IResult)result!;

            if ((actualResult as IStatusCodeHttpResult)?.StatusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found.");
            }

            return result;
        }
    }
}
