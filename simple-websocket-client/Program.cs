using WebSocketSharp;

namespace WebsocketClient;

public class Program
{
    static void Main(string[] args)
    {
        // create an instance of a ws client
        WebSocket ws = new WebSocket("ws://localhost:7149/Echo");

        ws.OnMessage += Ws_OnMessage;

        ws.Connect();
        ws.Send("Hello Server from a cutie client!");

        Console.ReadKey();

        ws.Close();
    }

    private static void Ws_OnMessage(object? sender, MessageEventArgs e)
    {
        Console.WriteLine($"Received from Server: {e.Data}");
    }
}
