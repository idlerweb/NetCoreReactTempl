using NetCoreReactTempl.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Domain.Repositories
{
    public interface IDataManager<TData> where TData : BaseData
    {
        Task<IEnumerable<TData>> GetUserData(long userId);
        Task<IEnumerable<TData>> GetUserData(long userId, string fieldFilter);
        Task<TData> GetData(long id);
        Task<TData> Create(TData data);
        Task<TData> Update(TData entity);
        Task Delete(long id);
    }
}
