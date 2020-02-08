using System.Threading.Tasks;

namespace AuthServer.Client
{
    public interface IClient
    {
        Task<ClientResponse> RequestAsync(string hostname, int port, string request);
    }
}
