using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace EA.Repository.Model;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        
    }

    /// <summary>
    ///     Database table name
    /// </summary>
    public System.Data.Entity.DbSet<Employee>? Employee { get; set; }

    public System.Data.Entity.DbSet<AdminModel>? Admin { get; set; }
}