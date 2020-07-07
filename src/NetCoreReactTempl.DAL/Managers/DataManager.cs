using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreReactTempl.DAL.Managers
{
    public class DataManager<TEntity, TModel> : IDataManager<TModel> where TEntity : BaseEntity
                                                                     where TModel : BaseModel
    {
        private readonly DataContext _context;
        private readonly IQueryable<TEntity> _collection;
        private readonly IMapper _mapper;

        public DataManager(DataContext context, IMapper mapper)
        {
            _context = context;
            var propertyInfo = _context.GetType().GetProperty($"{typeof(TEntity).Name}s");
            _collection = propertyInfo.GetValue(_context, null) as IQueryable<TEntity>;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TModel>> GetCollection() => (await _collection.ToArrayAsync()).Select(c => _mapper.Map<TModel>(c));

        public async Task<TModel> GetAsync(long id) => _mapper.Map<TModel>(await _collection.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<TModel> CreateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public async Task<TModel> UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
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
            return _mapper.Map<TModel>(entityDest);
        }

        public async Task DeleteAsync(long id)
        {
            _context.Remove(await _collection.FirstAsync(e => e.Id == id));
            await _context.SaveChangesAsync();
        }
    }
}
