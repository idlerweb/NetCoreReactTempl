using NetCoreReactTempl.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Domain.Repositories
{
    public interface IAuthManager
    {
        Task<bool> EmailExist(string email);
        Task<IEnumerable<User>> Get();
        Task<User> Get(long id);
        Task<User> Get(string email);
        Task<User> Add(User authInfo);
        Task Delete(long id);
    }
}
