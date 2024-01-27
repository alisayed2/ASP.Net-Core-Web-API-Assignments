using Assignment_2.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Assignment_2.Contexts
{
    public class ITIDbContext : DbContext

    {
        public ITIDbContext() { }
        public ITIDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
