using Microsoft.EntityFrameworkCore;
using MvcTest.Models;

namespace MvcTest.Data
{
    public class MvcTestContext : DbContext
    {
        public MvcTestContext(DbContextOptions<MvcTestContext> options)
            : base(options)
        {
        }

        public DbSet<FileModel> FileModel { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
    }
}