using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebsocketServer;

public class Echo : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        // write the message to server console
        Console.WriteLine($"Received message from client: {e.Data}");
        // send the same message to the client console
        Send(e.Data);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // init new web socker server with with self-specified url
        WebSocketServer wsServer = new WebSocketServer("ws://localhost:7149");

        // add service to web socker server
        wsServer.AddWebSocketService<Echo>("/Echo");

        wsServer.Start();
        
        Console.WriteLine("Websocket Server started on \"ws://localhost:7149/Echo\"");
        Console.ReadKey();
        
        wsServer.Stop();
    }
}