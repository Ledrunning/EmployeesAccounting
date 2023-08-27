using System.Data;
using EA.Repository.Configurations;
using EA.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EA.Repository;

/// <summary>
///     dotnet tool install --global dotnet-ef
///     dotnet ef migrations add InitialCreate
///     dotnet ef database update
/// </summary>
public sealed class DatabaseContext : DbContext, IUnitOfWork
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new AdministratorConfiguration());
    }
}