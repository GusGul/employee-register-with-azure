using Microsoft.EntityFrameworkCore;
using employee_register_with_azure.Models;

namespace employee_register_with_azure.Context
{
    public class HRContext : DbContext
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}