using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server.ViewModel;

public class ServerViewModel : INotifyPropertyChanged
{
    

    #region Properties

    public string ReceivedMessage 
    { 
        get => _receivedMessage;
        set => SetField(ref _receivedMessage, value);
    }

    #endregion

    #region Fields

    private readonly TcpListener _server;
    private readonly TcpClient? _client;
    private string _receivedMessage = "Das wird der Server";

    #endregion

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public ServerViewModel()
    { 
        // Erstelle einen TCP-Server auf dem localhost und dem Port 13000
        
        _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
        _server.Start();

        Console.WriteLine("Server gestartet. Warten auf Verbindungen...");

        // Warte auf eine Verbindung vom Client
        _server.BeginAcceptTcpClient(new AsyncCallback(WaitForMessage), _server);

        Console.WriteLine("Verbindung von einem Client hergestellt.");
    }

    public void WaitForMessage(IAsyncResult ar)
    {
        if (ar.AsyncState == null) return;

        // Get the listener that handles the client request.
        TcpListener listener = (TcpListener)ar.AsyncState;
        
        TcpClient client = listener.AcceptTcpClient();
        // Erstelle einen NetworkStream, um Daten zu senden und zu empfangen
        NetworkStream stream = _client.GetStream();

        // Empfange die Nachricht vom Server
        byte[] messageBytes = new byte[1024];
        int bytesRead = stream.Read(messageBytes, 0, messageBytes.Length);
        ReceivedMessage = Encoding.ASCII.GetString(messageBytes, 0, bytesRead);

        Console.WriteLine("Nachricht vom Server empfangen: " + ReceivedMessage);
    }

    ~ServerViewModel()
    {
        // Schlieﬂe die Verbindung zum Client
        _client.Close();
        // Beende den Server
        _server.Stop();
    }

    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}