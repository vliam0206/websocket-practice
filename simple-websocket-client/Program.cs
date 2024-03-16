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
        
        // show console client
        var input = "";        
        while (input != "G9")
        {
            Console.WriteLine("Enter your message: ");
            input = Console.ReadLine();
            ws.Send(input);
        }             

        ws.Close();
    }

    private static void Ws_OnMessage(object? sender, MessageEventArgs e)
    {
        Console.WriteLine($"Received from Server: {e.Data}");
    }
}
