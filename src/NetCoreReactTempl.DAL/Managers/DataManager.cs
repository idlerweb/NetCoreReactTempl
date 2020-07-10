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
    public class DataManager<TEntity, TData> : IDataManager<TData> where TEntity : BaseDataEntity
                                                                   where TData : BaseData
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

        public async Task<IEnumerable<TData>> GetUserData(long userId) =>
            _mapper.Map<IEnumerable<TData>>(
                await _collection.Include(f => f.Fields).Where(d => d.UserId == userId).ToArrayAsync()
            );

        public async Task<IEnumerable<TData>> GetUserData(long userId, string fieldFilter) =>
            _mapper.Map<IEnumerable<TData>>(
                await _collection.Include(f => f.Fields)
                    .Where(d => d.UserId == userId 
                        && (fieldFilter == null || d.Fields.Any(df => df.Value.Contains(fieldFilter)))
                    ).ToArrayAsync()
            );

        public async Task<TData> GetData(long id) =>
            _mapper.Map<TData>(
                await _collection.Include(f => f.Fields).Where(d => d.Id == id).ToArrayAsync()
            );

        public async Task<TData> Create(TData data)
        {
            var entity = _mapper.Map<TEntity>(data);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TData>(entity);
        }

        public async Task<TData> Update(TData data)
        {
            var entity = _mapper.Map<TEntity>(data);
            var entityDest = await _collection.Include(d => d.Fields).FirstOrDefaultAsync(e => e.Id == entity.Id);
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

            foreach (var field in entityDest.Fields)
            {
                field.Value = data.Fields[field.Name].ToString();
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<TData>(entityDest);
        }

        public async Task Delete(long id)
        {
            _context.Remove(await _collection.FirstAsync(e => e.Id == id));
            await _context.SaveChangesAsync();
        }
    }
}
