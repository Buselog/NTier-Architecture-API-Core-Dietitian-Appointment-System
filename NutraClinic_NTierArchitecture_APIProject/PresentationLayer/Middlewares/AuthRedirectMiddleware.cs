namespace PresentationLayer.Middlewares
{
    public class AuthRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (!path.StartsWith("/auth") && !path.StartsWith("/account/login"))
            {
                var token = context.Request.Cookies["JwtToken"];

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.Redirect("/Auth/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}