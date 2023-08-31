using System;
using System.Configuration;
using System.Threading;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.Services.ViewServices;
using EA.DesktopApp.View;
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
                var timeOutConfig = ConfigurationManager.AppSettings["timeOut"];
                int.TryParse(timeOutConfig, out var timeOut);

                var employeeGatewayService = new EmployeeGatewayService(urlAddress, timeOut);

                builder.RegisterType<FaceDetectionService>().As<IFaceDetectionService>().InstancePerLifetimeScope();
                builder.RegisterType<PhotoShootService>().As<IPhotoShootService>().InstancePerLifetimeScope();
                builder.RegisterType<SoundPlayerService>().As<ISoundPlayerService>().InstancePerLifetimeScope();
                builder.RegisterInstance(employeeGatewayService).As<IEmployeeGatewayService>().SingleInstance();

                builder.RegisterType<MainWindow>().InstancePerLifetimeScope();
                builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();
                builder.RegisterType<AdminViewModel>().InstancePerLifetimeScope();

                builder.RegisterType<ModalWindow>().InstancePerDependency(); // Or per your desired lifetime.
                builder.RegisterType<ModalViewModel>().InstancePerDependency(); // Or per your desired lifetime.

                builder.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();
                builder.RegisterType<LoginWindow>().InstancePerDependency(); // Or per your desired lifetime.
                builder.RegisterType<LoginViewModel>().InstancePerDependency(); // Or per your desired lifetime.

                builder.RegisterType<RedactorViewModel>().InstancePerLifetimeScope();


                builder.RegisterType<RegistrationForm>().InstancePerLifetimeScope();
                builder.RegisterType<RegistrationViewModel>().InstancePerLifetimeScope();
                
                Container = builder.Build();
            }
            catch (Exception e)
            {
                Logger.Error("Application run error! {e}", e);
                throw new ApplicationException($"{e.Message}. Application run error!");
            }
        }
    }
}