using AuthServer.Core.Security;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;

namespace AuthServer.Core.Network
{
    /// <summary>
    /// Implements TLS layer for incoming connections
    /// </summary>
    public class TlsTransport : INetworkTransport
    {
        #region Services
        
        private ICertificateProvider certificateProvider;
        private ILogger<TlsTransport> logger;

        #endregion

        #region Events

        public event ConnectionOpenedEventHandler ConnectionOpened;

        #endregion

        public TlsTransport(ICertificateProvider certificateProvider, ILogger<TlsTransport> logger)
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
            try
            {
                var sslStream = new SslStream(new NetworkStream(connection, false));
                var state = new StateObject
                {
                    Stream = sslStream,
                    Connection = connection
                };

                sslStream.BeginAuthenticateAsServer(certificateProvider.GetCertificate(), false, SslProtocols.Tls12, false, new AsyncCallback(OnAuthenticated), state);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error accepting TLS connection");
            }
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
                    logger.LogDebug("TLS: authentication successful");
                    OnConnectionOpened(new ConnectionOpenedEventArgs(state.Connection, sslStream));
                }
                else
                {
                    logger.LogDebug("TLS: authentication not successful");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to authenticate as TLS server");
                sslStream.Close();
                connection.Close();
                logger.LogDebug("Socket: connection closed");
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
