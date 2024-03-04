using D.Application.WebSocket;
using Microsoft.AspNetCore.Authorization;

namespace D.Application.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketHandler _webSocketHandler;

        public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            await _webSocketHandler.HandleWebSocket(context);
        }
    }
}
