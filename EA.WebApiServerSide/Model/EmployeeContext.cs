using Microsoft.EntityFrameworkCore;

namespace EA.WebApiServerSide.Model
{
    public class EmployeeContext : DbContext
    {
        /// <summary>
        /// Database table name
        /// </summary>
        public DbSet<Person> Person { get; set; }

        public DbSet<AdminModel> Admin { get; set; }

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        { }
    }
}