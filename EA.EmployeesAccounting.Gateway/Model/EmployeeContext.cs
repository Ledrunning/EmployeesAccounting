namespace EA.ServerGateway.Model
{
    public class EmployeeContext : DbContext
    {
        /// <summary>
        /// Database table name
        /// </summary>
        public DbSet<Employee> Person { get; set; }

        public DbSet<AdminModel> Admin { get; set; }

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        { }
    }
}