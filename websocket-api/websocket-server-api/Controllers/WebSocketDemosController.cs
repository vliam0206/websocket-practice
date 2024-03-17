using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;
using System.Text;

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
                // generate values x,y random every 2 seconds
                var random = new Random();
                while (ws.State == WebSocketState.Open)
                {
                    int x = random.Next(1, 100);
                    int y = random.Next(1, 100);
                    var stringValue = $"{{ \"x\": {x}, \"y\": {y} }}";
                    var buffer = Encoding.UTF8.GetBytes(stringValue);
                    await ws.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text, true, CancellationToken.None);
                    // waiting for 2 seconds for the next pair of values
                    await Task.Delay(2000);                    
                }
                // close ws connection
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "WebSocket connection closed by Server.", CancellationToken.None);
            }
        } else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
        }
    }
}
