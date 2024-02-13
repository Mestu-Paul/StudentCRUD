using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using A.Contracts.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using A.Contracts.Entities;

namespace D.Application.WebSocket
{
    [Authorize]
    public class WebSocketHandler
    {
        private readonly ILogger<WebSocketHandler> _logger;
        private readonly Dictionary<string, System.Net.WebSockets.WebSocket> connectedUsers = new Dictionary<string, System.Net.WebSockets.WebSocket>();

        public WebSocketHandler(ILogger<WebSocketHandler> logger)
        {
            _logger = logger;
        }
        public async Task HandleWebSocket(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var ws = await context.WebSockets.AcceptWebSocketAsync();
                
                string username = context.Request.Query["token"];
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

        public async Task SendMessageToUser(MessageDTO message)
        {
            string messageString = JsonConvert.SerializeObject(message);
            await SendMessageToUser(messageString);
            return;
        }

        public async Task SendMessageToUser(string messageString)
        {
            MessageDTO message = JsonConvert.DeserializeObject<MessageDTO>(messageString);
            if (connectedUsers.TryGetValue(message.Recipientname, out System.Net.WebSockets.WebSocket userWs))
            {
                string temp = message.Recipientname;
                message.Recipientname = message.SenderUsername;
                message.Recipientname = temp;

                messageString = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(messageString);
                var arraySegment = new ArraySegment<byte>(bytes);
                await userWs.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

    }
}
