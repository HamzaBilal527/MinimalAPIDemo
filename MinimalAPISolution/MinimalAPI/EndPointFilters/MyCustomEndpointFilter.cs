namespace MinimalAPI.EndPointFilters
{
    public class MyCustomEndpointFilter : IEndpointFilter
    {
        private readonly ILogger<MyCustomEndpointFilter> _logger;

        public MyCustomEndpointFilter( ILogger<MyCustomEndpointFilter> logger)
        {
            _logger = logger;
            
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            _logger.LogInformation("EndPointFilter - before logic");

            var result = await next(context);

            _logger.LogInformation("EndPointFilter - after logic");


            return result;
        }
    }
}
