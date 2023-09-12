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

                // Register the CancellationTokenSource as a single instance so the same source is used everywhere.
                builder.RegisterInstance(new CancellationTokenSource()).AsSelf();
                // Register a factory to get the CancellationToken from the source.
                builder.Register(c => c.Resolve<CancellationTokenSource>().Token).As<CancellationToken>();

                builder.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();
                builder.RegisterType<WindowManager>().As<IWindowManager>().InstancePerLifetimeScope();

                builder.RegisterType<FaceDetectionService>().As<IFaceDetectionService>().InstancePerLifetimeScope();
                builder.RegisterType<PhotoShootService>().As<IPhotoShootService>().InstancePerLifetimeScope();
                builder.RegisterType<SoundPlayerService>().As<ISoundPlayerService>().InstancePerLifetimeScope();
                builder.RegisterInstance(employeeGatewayService).As<IEmployeeGatewayService>().SingleInstance();

                builder.RegisterType<MainWindow>().InstancePerLifetimeScope();
                builder.RegisterType<MainViewModel>().InstancePerLifetimeScope();

                builder.RegisterType<AdminForm>().InstancePerLifetimeScope();
                builder.RegisterType<AdminViewModel>().InstancePerLifetimeScope();

                builder.RegisterType<ModalWindow>().InstancePerDependency(); 
                builder.RegisterType<ModalViewModel>().InstancePerDependency(); 

                builder.RegisterType<LoginWindow>().InstancePerDependency(); 
                builder.RegisterType<LoginViewModel>().InstancePerDependency();

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