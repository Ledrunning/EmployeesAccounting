using System.Diagnostics;
using EA.Repository;
using EA.Repository.Contracts;
using EA.Repository.Repository;
using EA.ServerGateway.Authentication;
using EA.ServerGateway.Extensions;
using EA.Services.Configuration;
using EA.Services.Contracts;
using EA.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

const string loggerConfig = "NLog.config";
var logger = NLogBuilder.ConfigureNLog(loggerConfig).GetCurrentClassLogger();
const string ConnectionString = "DbConnection";

try
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString(ConnectionString);

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGenWithOptions();

    builder.Services.AddDbContext<DatabaseContext>(options => { options.UseSqlServer(connectionString); });
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddTransient<ServiceKeysConfig>();
    builder.Services.AddTransient<ConfigurationService>();
    builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddTransient<IAdministratorRepository, AdministratorRepository>();
    builder.Services.AddTransient<IEmployeeService, EmployeeService>();
    builder.Services.AddTransient<IAdministratorService, AdministratorService>();

    builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

    var app = builder.Build();

    // Get the service and call the initialization method
    var cts = new CancellationTokenSource();
    await app.InitializeAdmin(cts.Token);

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.AddSwaggerUi();
    }

    app.MapControllers();
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