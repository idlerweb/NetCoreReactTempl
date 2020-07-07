using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Domain.Repositories
{
    public interface IDataManager<T>
    {
        Task<IEnumerable<T>> GetCollection();
        Task<T> GetAsync(long id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(long id);
    }
}
