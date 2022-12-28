using System.Diagnostics;
using EA.Repository.Model;
using EA.ServerGateway.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

const string loggerConfig = "NLog.config";
var logger = NLogBuilder.ConfigureNLog(loggerConfig).GetCurrentClassLogger();
const string ConnectionString = "DbConnection";


try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //TODO con str needed
    var connStr = builder.Configuration.GetConnectionString(ConnectionString);

    builder.Services.AddDbContext<DatabaseContext>(
        options => options.UseSqlServer(connStr));

    //builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseRouting();

    app.Lifetime.RegisterApplicationLifetimeDelegates(logger);

    app.Run();
}
catch (Exception ex)
{
    var name = typeof(Program).Assembly.GetName().Name;
    Trace.Write(
        $"[{DateTime.Now:HH:mm:ss.fff}] Application startup error [{name}]! Details {ex.Message}");
    logger.Fatal(ex, $"Application startup error [{name}]");
}
finally
{
    LogManager.Shutdown();
}