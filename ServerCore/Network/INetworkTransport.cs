using System.IO;
using System.Net.Sockets;

namespace ServerCore.Network
{
    /// <summary>
    /// Interface for network transports (such as plain, ssl, tls)
    /// </summary>
    public interface INetworkTransport 
    {
        event ConnectionOpenedEventHandler ConnectionOpened;

        void OnAcceptConnection(Socket connection);

        void OnCloseConnection(Stream stream, Socket connection);
    }
}
