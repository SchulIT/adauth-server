using System.IO;
using System.Net.Sockets;

namespace ServerCore.Network
{
    /// <summary>
    /// Implements an unencrypted transport layer
    /// </summary>
    public class UnencryptedTransport : INetworkTransport
    {
        public event ConnectionOpenedEventHandler ConnectionOpened;

        protected virtual void OnConnectionOpened(ConnectionOpenedEventArgs args)
        {
            ConnectionOpened?.Invoke(args);
        }

        public void OnAcceptConnection(Socket connection)
        {
            var stream = new NetworkStream(connection, true);
            OnConnectionOpened(new ConnectionOpenedEventArgs(connection, stream));
        }

        public void OnCloseConnection(Stream stream, Socket connection)
        {
            
        }
    }
}
