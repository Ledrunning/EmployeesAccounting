using System.Diagnostics;
using EA.Repository;
using EA.Repository.Contracts;
using EA.Repository.Repository;
using EA.ServerGateway.Authentication;
using EA.ServerGateway.Extensions;
using EA.Services.Contracts;
using EA.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

    var connectionString = builder.Configuration.GetConnectionString(ConnectionString);

    builder.Services.AddDbContext<DatabaseContext>(options => { options.UseSqlServer(connectionString); });
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddTransient<IEmployeeService, EmployeeService>();

    builder.Services.AddAuthentication()
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
    });

    var app = builder.Build();

    app.UseAuthorization();

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