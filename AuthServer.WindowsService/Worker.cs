using AuthServer.Core.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServer.WindowsService
{
    public class Worker : IHostedService
    {
        private readonly IServer server;
        private readonly ILogger<Worker> logger;

        public Worker(IServer server, ILogger<Worker> logger)
        {
            this.server = server;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting server...");
            server.Start();
            logger.LogInformation("Server started.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping server...");
            server.Stop();
            logger.LogInformation("Server stopped.");

            return Task.CompletedTask;
        }
    }
}
