using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreReactTempl.DAL.Managers
{
    public class DataManager<T> : IDataManager<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly IQueryable<T> _collection;

        public DataManager(DataContext context)
        {
            _context = context;            
            var propertyInfo = _context.GetType().GetProperty($"{typeof(T).Name}s");
            _collection = propertyInfo.GetValue(_context, null) as IQueryable<T>;
        }

        public IQueryable<T> GetCollection() => _collection;
        public async Task<T> GetAsync(long id) => await _collection.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<T> CreateAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var entityDest = await _collection.FirstOrDefaultAsync(e => e.Id == entity.Id);
            var entityDestProperties = entityDest.GetType().GetProperties().Where(p => !string.Equals(p.Name, "Id")
                                                                                    || !string.Equals(p.Name, "User")
                                                                                    || !string.Equals(p.Name, "UserId"));
            foreach (var property in entityDestProperties)
            {
                var value = property.GetValue(entity, null);
                if (value == null)
                    continue;

                property.SetValue(entityDest, value, null);
            }
            await _context.SaveChangesAsync();
            return entityDest;
        }

        public async Task DeleteAsync(long id)
        {
            _context.Remove(await _collection.FirstAsync(e => e.Id == id));
            await _context.SaveChangesAsync();
        }
    }
}
