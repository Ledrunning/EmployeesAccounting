using System;
using System.Configuration;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels;

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

                var urlAddress = ConfigurationManager.AppSettings["serverUriString"];
                var employeeApi = new EmployeeApi(urlAddress);
                builder.RegisterInstance(employeeApi).As<IEmployeeApi>().SingleInstance();

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

                builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
                Container = builder.Build();

                return true;
            }
            catch (Exception e)
            {
                //LoggerManager.FatalMessage("Ошибка инициализации службы! ", e);
                throw new ApplicationException( $"{e.Message}. Hui");
            }
        }
    }
}