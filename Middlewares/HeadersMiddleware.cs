namespace GorBilet_example.Middlewares
{
    internal class HeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public HeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["Header1"] = $"123";
            context.Response.OnStarting(() => {
                context.Response.Headers["Header1"] = $"123";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
