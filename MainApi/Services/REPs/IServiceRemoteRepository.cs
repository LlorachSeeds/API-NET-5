using System.Threading.Tasks;

namespace Services.REPs
{
    public interface IServiceRemoteRepository<TEntity>
    {
        public Task<string> RemoteRequest(string endpoint, TEntity remoteObject);
    }
}