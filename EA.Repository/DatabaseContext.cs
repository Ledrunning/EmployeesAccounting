using EA.Repository.Contracts;
using EA.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace EA.Repository;

public class DatabaseContext : DbContext, IUnitOfWork
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///     Database table name
    /// </summary>
    public System.Data.Entity.DbSet<Employee>? Employee { get; set; }

    public System.Data.Entity.DbSet<Administrator>? Administrator { get; set; }
}