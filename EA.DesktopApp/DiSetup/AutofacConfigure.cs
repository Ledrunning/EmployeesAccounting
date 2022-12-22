using System;
using System.Configuration;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels;
using NLog;

namespace EA.DesktopApp.DiSetup
{
    public static class AutofacConfigure
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static IContainer Container { get; private set; }


        public static void ConfigureContainer()
        {
            try
            {
                var builder = new ContainerBuilder();

                var urlAddress = ConfigurationManager.AppSettings["serverUriString"];
                var employeeApi = new EmployeeApi(urlAddress);
                builder.RegisterInstance(employeeApi).As<IEmployeeApi>().SingleInstance();

                builder.RegisterType<FaceDetectionService>().As<IFaceDetectionService>().InstancePerLifetimeScope();
                builder.RegisterType<PhotoShootService>().As<IPhotoShootService>().SingleInstance();
                builder.RegisterType<SoundPlayerService>().As<ISoundPlayerService>().InstancePerLifetimeScope();

                builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
                builder.RegisterType<AdminViewModel>().InstancePerLifetimeScope();
                builder.RegisterType<LoginViewModel>().InstancePerLifetimeScope();
                builder.RegisterType<RedactorViewModel>().InstancePerLifetimeScope();
                builder.RegisterType<RegistrationViewModel>().InstancePerLifetimeScope();

                Container = builder.Build();
            }
            catch (Exception e)
            {
                Logger.Error("Ошибка инициализации службы! {e}", e);
                throw new ApplicationException($"{e.Message}. Hui");
            }
        }
    }
}