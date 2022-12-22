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
    private TcpClient? _client;
    private string _receivedMessage = "Das wird der Server";
    private NetworkStream? _stream;
    private readonly byte[] _messageBytes = new byte[1024];
    private readonly int _offset = 0;

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
        _server.BeginAcceptTcpClient(new AsyncCallback(ConnectWithClient), _server);

        Console.WriteLine("Verbindung von einem Client hergestellt.");
    }

    public void ConnectWithClient(IAsyncResult ar)
    {
        if (ar.AsyncState == null) return;

        // Get the listener that handles the client request.
        TcpListener listener = (TcpListener)ar.AsyncState;

        _client = listener.AcceptTcpClient();
        // Erstelle einen NetworkStream, um Daten zu senden und zu empfangen
        _stream = _client.GetStream();


        _stream.BeginRead(_messageBytes, _offset, _messageBytes.Length, new AsyncCallback(WaitForMessage), _stream);
        _server.EndAcceptSocket(ar);
    }

    public void WaitForMessage(IAsyncResult ar)
    {
        if (ar.AsyncState == null) return;
        NetworkStream streamObject = (NetworkStream) ar.AsyncState;
        
        // Empfange die Nachricht vom Server

        int bytesRead = _stream.Read(_messageBytes, 0, _messageBytes.Length);
        ReceivedMessage = Encoding.ASCII.GetString(_messageBytes, 0, bytesRead);

        Console.WriteLine("Nachricht vom Server empfangen: " + ReceivedMessage);
        _client?.Close();
        _stream?.Close();
    }

    ~ServerViewModel()
    {
        // Schlieﬂe die Verbindung zum Client
        _client?.Close();
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