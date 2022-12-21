using System;
using System.Net.Sockets;
using System.Text;

namespace Client.ViewModel
{
    public class ClientViewModel
    {
        static void Initialize()
        {
            // Erstelle eine Verbindung zum Server auf dem localhost und dem Port 13000
            TcpClient client = new TcpClient("127.0.0.1", 13000);

            Console.WriteLine("Verbindung zum Server hergestellt.");

            // Erstelle einen NetworkStream, um Daten zu senden und zu empfangen
            NetworkStream stream = client.GetStream();

            // Empfange die Nachricht vom Server
            byte[] messageBytes = new byte[1024];
            int bytesRead = stream.Read(messageBytes, 0, messageBytes.Length);
            string message = Encoding.ASCII.GetString(messageBytes, 0, bytesRead);

            Console.WriteLine("Nachricht vom Server empfangen: " + message);

            // Schlieﬂe die Verbindung zum Server
            client.Close();
            Console.WriteLine("Verbindung zum Server beendet.");
        }
    }
}
