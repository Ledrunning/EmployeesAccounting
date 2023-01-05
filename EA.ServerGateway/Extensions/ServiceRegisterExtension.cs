using EA.ServerGateway.Helpers;
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
}