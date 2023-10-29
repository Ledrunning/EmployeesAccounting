using EA.ServerGateway.Helpers;
using EA.Services.Contracts;
using NLog;

namespace EA.ServerGateway.Extensions;

internal static class ServiceRegisterExtension
{
    public static void RegisterApplicationLifetimeDelegates(this IHostApplicationLifetime hostApplicationLifetime,
        Logger logger)
    {
        hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            logger.Info(
                $"EA.Gateway has been started : [{ProgramRuntime.ProgramName}] ({ProgramRuntime.ProgramVersion}) Rev({ProgramRuntime.ProgramRevision})");
            logger.Info($"Program Version - {ProgramRuntime.ProgramVersion}");
        });

        hostApplicationLifetime.ApplicationStopped.Register(() =>
        {
            logger.Info(
                $"EA.Gateway has been stopped: [{ProgramRuntime.ProgramName}] ({ProgramRuntime.ProgramVersion}) Rev({ProgramRuntime.ProgramRevision})");
            logger.Info($"Program Version - {ProgramRuntime.ProgramVersion}");
        });
    }

    public static async Task InitializeAdmin(this WebApplication app, CancellationToken token)
    {
        using var scope = app.Services.CreateScope();
        var scopedServiceProvider = scope.ServiceProvider;
        var adminService = scopedServiceProvider.GetRequiredService<IAdministratorService>();
        await adminService.InitializeAdmin(token);
    }
}