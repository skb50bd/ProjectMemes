using System.Threading.Tasks;
using XMemes.Models.Domain;
using XMemes.Models.Operations;

namespace XMemes.Data.Repositories
{
    public interface IMemerRepository : IRepository<Memer>
    {
        Task<Outcome<object>> IsUsernameAvailable(string username);
    }
}