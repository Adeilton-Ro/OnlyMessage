namespace Webapi.MiddlewareConfigs
{
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.Query.TryGetValue("access_token", out var accessToken))
            {
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            await _next(httpContext);
        }
    }
}
