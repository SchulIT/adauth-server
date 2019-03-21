using ServerCore.Log;
using ServerCore.Security;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;

namespace ServerCore.Network
{
    /// <summary>
    /// Implements TLS layer for incoming connections
    /// </summary>
    public class TlsTransport : INetworkTransport
    {
        #region Services
        
        private ICertificateProvider certificateProvider;
        private ILogger logger;

        #endregion

        #region Events

        public event ConnectionOpenedEventHandler ConnectionOpened;

        #endregion

        public TlsTransport(ICertificateProvider certificateProvider, ILogger logger)
        {
            this.certificateProvider = certificateProvider;
            this.logger = logger;
        }

        protected virtual void OnConnectionOpened(ConnectionOpenedEventArgs args)
        {
            ConnectionOpened?.Invoke(args);
        }

        public void OnAcceptConnection(Socket connection)
        {
            var sslStream = new SslStream(new NetworkStream(connection, false));
            var state = new StateObject
            {
                Stream = sslStream,
                Connection = connection
            };

            sslStream.BeginAuthenticateAsServer(certificateProvider.GetCertificate(), false, SslProtocols.Tls12, false, new AsyncCallback(OnAuthenticated), state);
        }

        private void OnAuthenticated(IAsyncResult ar)
        {
            var state = ar.AsyncState as StateObject;
            var sslStream = state.Stream as SslStream;
            var connection = state.Connection as Socket;

            try
            {
                sslStream.EndAuthenticateAsServer(ar);

                if (sslStream.IsAuthenticated)
                {
                    logger.WriteDebug("TLS: authentication successful");
                    OnConnectionOpened(new ConnectionOpenedEventArgs(state.Connection, sslStream));
                }
                else
                {
                    logger.WriteDebug("TLS: authentication not successful");
                }
            }
            catch (Exception e)
            {
                logger.WriteException(e);
                sslStream.Close();
                connection.Close();
                logger.WriteDebug("Socket: connection closed");
            }
        }

        public void OnCloseConnection(Stream stream, Socket connection)
        {
            
        }

        private class StateObject
        {
            public Socket Connection { get; set; }

            public Stream Stream { get; set; }
        }
    }
}
