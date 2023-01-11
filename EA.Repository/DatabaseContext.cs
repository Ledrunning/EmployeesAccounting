using EA.Repository.Contracts;
using EA.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace EA.Repository;

/// <summary>
///     Enable-Migrations
///     Add-Migration Initial
///     Update-Database
/// </summary>
public class DatabaseContext : DbContext, IUnitOfWork
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public System.Data.Entity.DbSet<Employee>? Employee { get; set; }

    public System.Data.Entity.DbSet<Administrator>? Administrator { get; set; }
}