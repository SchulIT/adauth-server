using System.Threading.Tasks;

namespace ServerGUI.Services.Client
{
    public interface IClient
    {
        Task<ClientResponse> RequestAsync(string hostname, int port, bool useTls, string request);
    }
}
