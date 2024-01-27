using CRUD_Operations_Assignment.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operations_Assignment.Contexts
{
    public class ITIDbContext : DbContext
    {
        public ITIDbContext() { }
        public ITIDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
