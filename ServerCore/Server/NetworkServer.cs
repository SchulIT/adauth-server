using ServerCore.Handler;
using ServerCore.Log;
using ServerCore.Network;
using ServerCore.Performance;
using ServerCore.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore.Server
{
    /// <summary>
    /// The actual AD Auth Server
    /// </summary>
    public class NetworkServer : IServer
    {
        /// <summary>
        /// Maxmimum length of the pending connections queue
        /// </summary>
        private const int Backlog = 10;

        /// <summary>
        /// Buffer size for the read buffer
        /// </summary>
        private const int BufferSize = 1024;

        /// <summary>
        /// Maximum length of the request until the server
        /// closes the connection to prevent clients from 
        /// spamming.
        /// 
        /// 10 KB sould be sufficient for now
        /// </summary>
        private const int MaxRequestLength = 10240;

        /// <summary>
        /// Maximum lifetime for a connection in order to submit
        /// a request.
        /// </summary>
        private const int MaxConnectionLifetimeInSeconds = 5;

        /// <summary>
        /// Network socket for incoming connections
        /// </summary>
        private Socket socketListener;

        /// <summary>
        /// Holds all active connections
        /// </summary>
        private List<StateObject> openConnections = new List<StateObject>();

        /// <summary>
        /// Thread which closes dead connections automatically.
        /// </summary>
        private Thread deadConnectionsThread;

        #region Services

        private readonly IRequestHandler requestHandler;
        private readonly ISettings settings;
        private readonly INetworkTransport transport;
        private readonly IPerformanceMonitor performance;
        private readonly ILogger logger;

        #endregion

        public NetworkServer(IRequestHandler requestHandler, ISettings settings, INetworkTransport transport, IPerformanceMonitor performance, ILogger logger)
        {
            this.requestHandler = requestHandler;
            this.settings = settings;
            this.transport = transport;
            this.performance = performance;
            this.logger = logger;

            deadConnectionsThread = new Thread(CloseDeadConnections);
        }

        /// <summary>
        /// Starts the network server.
        /// </summary>
        public void Start()
        {
            if (socketListener != null)
            {
                return;
            }

            transport.ConnectionOpened += OnConnectionOpened;

            var ipAddress = settings.Server.IPv6 ? IPAddress.IPv6Any : IPAddress.Any;
            var endPoint = new IPEndPoint(ipAddress, settings.Server.Port);

            var addressFamily = settings.Server.IPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
            socketListener = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);

            socketListener.Bind(endPoint);
            socketListener.Listen(Backlog);
            deadConnectionsThread.Start();

            logger.WriteLog("Listen for incoming connections.");

            socketListener.BeginAccept(new AsyncCallback(OnAcceptConnection), socketListener);
        }

        /// <summary>
        /// Stops the network server
        /// </summary>
        public void Stop()
        {
            transport.ConnectionOpened -= OnConnectionOpened;

            socketListener.Close();
            socketListener.Dispose();

            deadConnectionsThread.Abort();

            CloseAllConnections();

            socketListener = null;
            logger.WriteLog("AD Auth Server stopped.");
        }

        private void OnAcceptConnection(IAsyncResult ar)
        {
            var listener = ar.AsyncState as Socket;
            var connection = listener.EndAccept(ar);
            transport.OnAcceptConnection(connection);
            socketListener.BeginAccept(new AsyncCallback(OnAcceptConnection), socketListener);
        }

        private void OnConnectionOpened(ConnectionOpenedEventArgs args)
        {
            var stream = args.Stream;
            var state = new StateObject
            {
                Connection = args.Connection,
                Stream = stream,
                InputBuffer = new byte[BufferSize],
                LastActivity = DateTime.Now
            };

            openConnections.Add(state);

            try
            {
                stream.BeginRead(state.InputBuffer, 0, BufferSize, new AsyncCallback(OnReceive), state);
            }
            catch
            {
                CloseConnection(state);
            }
        }

        private async void OnReceive(IAsyncResult ar)
        {
            var state = ar.AsyncState as StateObject;
            var stream = state.Stream;
            var connection = state.Connection;

            try
            {
                var bytesReceived = stream.EndRead(ar);
                logger.WriteDebug($"Request: {bytesReceived} bytes received");

                if (bytesReceived > 0)
                {
                    state.LastActivity = DateTime.Now;

                    state.Received += Encoding.UTF8.GetString(state.InputBuffer, 0, bytesReceived);

                    if (state.Received.Length > MaxRequestLength)
                    {
                        logger.WriteLog("Request: maximum request size reached, close connection");
                        CloseConnection(state);
                        return;
                    }

                    logger.WriteDebug($"Request: " + state.Received);
                    var canHandle = await requestHandler.CanHandleAsync(state.Received);

                    if (!canHandle)
                    {
                        logger.WriteDebug("Request: did not read JSON object, proceed reading");
                        stream.BeginRead(state.InputBuffer, 0, BufferSize, new AsyncCallback(OnReceive), state);
                    }
                    else
                    {
                        state.IsRequestCompleted = true;

                        logger.WriteDebug("Request: handle request data");
                        var response = await requestHandler.HandleAsync(state.Received);

                        logger.WriteDebug("Response: send response");
                        var responseByteArray = Encoding.UTF8.GetBytes(response);

                        try
                        {
                            stream.BeginWrite(responseByteArray, 0, responseByteArray.Length, new AsyncCallback(OnSent), state);
                        }
                        catch (Exception)
                        {
                            CloseConnection(state);
                        }
                    }
                }
            }
            catch (Exception)
            {
                CloseConnection(state);
            }
        }

        private void OnSent(IAsyncResult ar)
        {
            var state = ar.AsyncState as StateObject;
            var stream = state.Stream;

            try
            {
                stream.EndWrite(ar);
                stream.Flush();
                logger.WriteDebug("Response: sent successfully");
            }
            catch (Exception e)
            {
                logger.WriteException(e);
            }

            CloseConnection(state);
        }

        /// <summary>
        /// Closes a connection
        /// </summary>
        /// <param name="state"></param>
        private void CloseConnection(StateObject state)
        {
            if (state == null)
            {
                return;
            }

            var stream = state.Stream;
            var connection = state.Connection;

            try
            {
                transport.OnCloseConnection(stream, connection);

                stream.Close();
            }
            catch (Exception) { }
            finally
            {
                logger.WriteDebug("Stream: closed");
            }

            try
            {
                connection.Close();
            }
            catch (Exception) { }
            finally
            {
                logger.WriteDebug("Socket: connection closed");
            }

            try
            {
                openConnections.Remove(state);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Closes a list of connections
        /// </summary>
        /// <param name="connections"></param>
        private void CloseConnections(IEnumerable<StateObject> connections)
        {
            foreach(var state in connections)
            {
                CloseConnection(state);
            }
        }

        /// <summary>
        /// Closes all open connections
        /// </summary>
        private void CloseAllConnections()
        {
            CloseConnections(openConnections.ToList());
        }

        /// <summary>
        /// Closes all dead connections. This method runs an infinite loop and is executed in a different thread.
        /// 
        /// A connection is concidered "dead" in the following cases:
        /// * the request is incomplete (no valid JSON received so far) and the last data was received "MaxConnectionLifetimeInSeconds" ago
        /// OR
        /// * the underlying connection is already closed
        /// </summary>
        private void CloseDeadConnections()
        {
            try
            {
                while (true)
                {
                    performance.WriteOpenConnections(openConnections.Count);
                    logger.WriteDebug($"Active connections: {openConnections.Count}");

                    var threshold = DateTime.Now.AddSeconds(-MaxConnectionLifetimeInSeconds);
                    List<StateObject> deadConnections;

                    /*
                     * Locking is required here as the list of connections may change during execution and List<T> is not thread-safe.
                     */
                    lock (openConnections)
                    {
                        deadConnections = openConnections.ToList().Where(x => x != null).Where(x => (x.LastActivity < threshold && x.IsRequestCompleted == false) || x.Connection.Connected == false).ToList();
                    }

                    var count = deadConnections.Count;
                    performance.WriteDeadConnections(count);

                    if (count > 0)
                    {
                        logger.WriteDebug($"Closing {deadConnections.Count} dead connections");
                        CloseConnections(deadConnections);
                    }

                    Thread.Sleep(500);
                }
            }
            catch (ThreadAbortException)
            {
                // Nothing to do here, just make the infinite loop stop
            }
        }

        /// <summary>
        /// Defines the state of an incoming connection (including its stream and underlying connection)
        /// </summary>
        private class StateObject
        {
            /// <summary>
            /// Input buffer which is internally used by .NET to write incoming data into
            /// </summary>
            public byte[] InputBuffer;

            /// <summary>
            /// Underlying stream
            /// </summary>
            public Stream Stream;

            /// <summary>
            /// Actual connection
            /// </summary>
            public Socket Connection;

            /// <summary>
            /// Received data
            /// </summary>
            public string Received;

            /// <summary>
            /// Flag whether the request is completed (response may be pending)
            /// </summary>
            public bool IsRequestCompleted = false;

            /// <summary>
            /// Indicates the last activity of the sender
            /// </summary>
            public DateTime LastActivity;
        }
    }
}

