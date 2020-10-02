using System.Threading.Tasks;
using XMemes.Models.Domain;

namespace XMemes.Data.Repositories
{
    public interface IMemerRepository : IRepository<Memer>
    {
        Task<bool> IsUsernameAvailable(string username);
    }
}