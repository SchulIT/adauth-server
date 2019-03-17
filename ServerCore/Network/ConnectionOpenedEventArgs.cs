using System;
using System.IO;
using System.Net.Sockets;

namespace ServerCore.Network
{
    public class ConnectionOpenedEventArgs : EventArgs
    {
        public Socket Connection { get; }

        public Stream Stream { get; }

        public ConnectionOpenedEventArgs(Socket connection, Stream stream)
        {
            Connection = connection;
            Stream = stream;
        }
    }
}
