using System.Threading.Tasks;

namespace ServerCore
{
    public interface IServerHandler
    {
        Task<bool> CanHandleAsync(string json);

        Task<string> HandleAsync(string json);
    }
}
