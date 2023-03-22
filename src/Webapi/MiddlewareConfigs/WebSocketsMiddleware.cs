namespace Webapi.MiddlewareConfigs
{
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate next;

        public WebSocketsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (request.Query.TryGetValue("access_token", out var accessToken))
            {
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            await next(httpContext);
        }
    }
}
