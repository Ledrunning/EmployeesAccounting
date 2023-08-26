using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EA.Repository.Context;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private const string ConnectionString = "DbConnection";

    public DatabaseContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        var connectionString = config.GetConnectionString(ConnectionString);
        builder.UseSqlServer(connectionString);
        return new DatabaseContext(builder.Options);
    }
}