using Microsoft.AspNetCore.Authentication;

namespace D.Application.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.User;

            var usernameClaim = user.Claims.FirstOrDefault(c => c.Type == "name");
            if (usernameClaim != null)
            {
                var username = usernameClaim.Value;
                context.Items["Username"] = username;
            }

            await _next(context);
        }
    }
}
