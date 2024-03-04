using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using A.Contracts.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using A.Contracts.Entities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace D.Application.WebSocket
{
    [Authorize]
    public class WebSocketHandler
    {
        private readonly ILogger<WebSocketHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, System.Net.WebSockets.WebSocket> connectedUsers = new Dictionary<string, System.Net.WebSockets.WebSocket>();

        public WebSocketHandler(ILogger<WebSocketHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private string ValidateJWT(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["TokenKey"];

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                SecurityToken validatedToken;
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                var nameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "name");
                if (nameClaim != null)
                {
                    return nameClaim.Value;
                }
                else return null;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

        public async Task HandleWebSocket(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var ws = await context.WebSockets.AcceptWebSocketAsync();

                string jwToken = context.Request.Query["access_token"];

                string username = ValidateJWT(jwToken);
                if(string.IsNullOrEmpty(username)) { return;}

                // Add user ID and WebSocket connection to dictionary
                connectedUsers[username] = ws;

                // Handle incoming messages
                await HandleIncomingMessages(ws);
            }
        }


        public async Task HandleIncomingMessages(System.Net.WebSockets.WebSocket ws)
        {
            while (true)
            {
                try
                {
                    // Receive message from client
                    var buffer = new byte[1024];
                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        // Process the received message as needed
                        // For example, extract target user ID and send message to that user
                        await SendMessageToUser(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        // Handle close message
                        break;
                    }
                }
                catch (WebSocketException)
                {
                    // Handle WebSocket exceptions
                    break;
                }
                catch (Exception)
                {
                    // Handle other exceptions
                    break;
                }
            }
        }

        public async Task SendMessageToUser(string messageString)
        {

            MessageDTO message = JsonConvert.DeserializeObject<MessageDTO>(messageString);
            await SendMessageToUser(messageString);
            return;
        }

        public async Task SendMessageToUser(MessageDTO message)
        {
            if (connectedUsers.TryGetValue(message.RecipientUsername, out System.Net.WebSockets.WebSocket userWs))
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var wsMessage = new { type="newMessage", content=message };
                string messageString = JsonConvert.SerializeObject(wsMessage, settings);
                var bytes = Encoding.UTF8.GetBytes(messageString);
                var arraySegment = new ArraySegment<byte>(bytes);
                await userWs.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task RemoveConnection(string username)
        {
            connectedUsers.Remove(username);
        }

    }
}
