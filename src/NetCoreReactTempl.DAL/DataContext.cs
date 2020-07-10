using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;

namespace NetCoreReactTempl.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Data> Datas { get; set; }
        public DbSet<Field> Fields { get; set; }
    }
}
