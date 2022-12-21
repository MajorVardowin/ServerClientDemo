using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Client.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly TcpClient _client;
        private string _message = "Das wird die View";

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Properties

        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        #endregion

        public ClientViewModel()
        {
            // Erstelle eine Verbindung zum Server auf dem localhost und dem Port 13000
            _client = new TcpClient("127.0.0.1", 13000);

            Console.WriteLine("Verbindung zum Server hergestellt.");
        }

        public void WriteToServer()
        {
            // Erstelle einen NetworkStream, um Daten zu senden und zu empfangen
            NetworkStream stream = _client.GetStream();

            // Sende eine Nachricht an den Client
            byte[] messageBytes = Encoding.ASCII.GetBytes(Message);
            stream.Write(messageBytes, 0, messageBytes.Length);

            Console.WriteLine("Nachricht an Client gesendet.");
        }

        ~ClientViewModel()
        {
            // Schlie�e die Verbindung zum Server
            _client.Close();
            Console.WriteLine("Verbindung zum Server beendet.");
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
}
