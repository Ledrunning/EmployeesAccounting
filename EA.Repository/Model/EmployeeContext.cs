using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EA.Repository.Model
{
    public class EmployeeContext : DbContext
    {
        /// <summary>
        /// Database table name
        /// </summary>
        public DbSet<Employee>? Employee { get; set; }

        public DbSet<AdminModel>? Admin { get; set; }

        public EmployeeContext() : base("Employees")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}