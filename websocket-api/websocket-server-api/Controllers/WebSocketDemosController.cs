using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace websocket_server_api.Controllers;

[Route("api/ws")]
[ApiController]
public class WebSocketDemosController : ControllerBase
{
    [HttpGet]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
            {
                Console.WriteLine("WebSocket Connection started.");

                // Receive loop
                var buffer = new byte[1024 * 4]; // Set an appropriate buffer size
                WebSocketReceiveResult result;
                do
                {
                    result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // Assuming received message is UTF-8 encoded JSON
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        if (message != null)
                        {
                            Console.WriteLine($"Received message from client: {message}");

                            // Send back the all received messages to the client
                            var jsonString = JsonSerializer.Serialize(new MessageObject { Message = message });
                            var stringBuffer = Encoding.UTF8.GetBytes(jsonString);
                            await ws.SendAsync(
                                new ArraySegment<byte>(stringBuffer),
                                WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                } while (!result.CloseStatus.HasValue);

                Console.WriteLine($"WebSocket Connection closed by the client. Status: {result.CloseStatus}.");
            }
        } else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
        }
    }
}

public class MessageObject
{
    public string Message { get; set; } = default!;
}
