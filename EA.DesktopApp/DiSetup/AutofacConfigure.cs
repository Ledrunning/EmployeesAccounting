using System;
using System.Configuration;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;

namespace EA.DesktopApp.DiSetup
{
    public static class AutofacConfigure
    {
        public static IContainer Container { get; private set; }

        public static bool ConfigureContainer()
        {
            try
            {
                var builder = new ContainerBuilder();

                var _urlAddress = ConfigurationManager.AppSettings["serverUriString"];
                var _dataStorage = new EmployeeApi(_urlAddress);

                builder.RegisterType<FaceDetectionService>().As<IFaceDetectionService>().SingleInstance();
                builder.RegisterType<PhotoShootService>().As<IPhotoShootService>().SingleInstance();
                builder.RegisterType<SoundPlayerService>().As<ISoundPlayerService>().InstancePerLifetimeScope();

                //var factorySettings = new FactorySettings(projectSettings);
                //factorySettings.LoadSettings();
                //builder.RegisterInstance(factorySettings).As<IFactorySettings>().SingleInstance();

                //builder.RegisterInstance(CancellationTokenSource).AsSelf().SingleInstance();

                //builder.RegisterType<NetworkConnectionService>()
                //    .As<INetworkConnectionService>()
                //    .SingleInstance();

                //builder.RegisterType<SystemTimeConfigurator>().AsSelf().SingleInstance();
                //builder.RegisterType<PackageFormatter>().AsSelf().SingleInstance();

                //builder.RegisterType<SqLiteManager>().As<ISqLiteManager>().SingleInstance();
                //builder.RegisterType<BreezeService>().AsSelf().SingleInstance();
                // Add the MainWindowclass and later resolve

                builder.RegisterType<MainWindow>().AsSelf();
                Container = builder.Build();
                return true;
            }
            catch (Exception e)
            {
                //LoggerManager.FatalMessage("Ошибка инициализации службы! ", e);
                return false;
            }
        }
    }
}