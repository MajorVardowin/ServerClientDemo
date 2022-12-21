using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.ViewModel;

public class ServerViewModel
{
    static void Initialize()
    {
        // Erstelle einen TCP-Server auf dem localhost und dem Port 13000
        TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
        server.Start();

        Console.WriteLine("Server gestartet. Warten auf Verbindungen...");

        // Warte auf eine Verbindung vom Client
        TcpClient client = server.AcceptTcpClient();

        Console.WriteLine("Verbindung von einem Client hergestellt.");

        // Erstelle einen NetworkStream, um Daten zu senden und zu empfangen
        NetworkStream stream = client.GetStream();

        // Sende eine Nachricht an den Client
        string message = "Hallo Client!";
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        stream.Write(messageBytes, 0, messageBytes.Length);

        Console.WriteLine("Nachricht an Client gesendet.");

        // Schlieﬂe die Verbindung zum Client
        client.Close();
        Console.WriteLine("Verbindung zum Client beendet.");

        // Beende den Server
        server.Stop();
    }
}