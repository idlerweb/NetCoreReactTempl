using System.Linq;
using System.Threading.Tasks;

namespace NetCoreReactTempl.DAL.Interfaces
{
    public interface IDataManager<T>
    {
        IQueryable<T> GetCollection();
        Task<T> GetAsync(long id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(long id);
    }
}
