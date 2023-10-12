using System;
using System.Configuration;
using System.Threading;
using System.Windows;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.Services.ViewServices;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels;
using EA.RecognizerEngine.Contracts;
using EA.RecognizerEngine.Engines;
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
                builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();

                builder.RegisterType<ModalViewModelFactory>().As<IModalViewModelFactory>().SingleInstance();
                builder.RegisterType<LbphFaceRecognition>().As<ILbphFaceRecognition>().SingleInstance();
                builder.RegisterType<FaceDetectionService>().As<IFaceDetectionService>().InstancePerLifetimeScope();
                builder.RegisterType<PhotoShootService>().As<IPhotoShootService>().InstancePerLifetimeScope();
                builder.RegisterType<SoundPlayerService>().As<ISoundPlayerService>().InstancePerLifetimeScope();
                builder.RegisterInstance(employeeGatewayService).As<IEmployeeGatewayService>().SingleInstance();

                builder.RegisterType<MainWindow>().InstancePerDependency();
                builder.RegisterType<MainViewModel>().InstancePerDependency();

                builder.RegisterType<AdminForm>().InstancePerDependency();
                builder.RegisterType<AdminViewModel>().InstancePerDependency();

                builder.RegisterType<Window>().InstancePerDependency(); 
                builder.RegisterType<ModalWindow>().InstancePerDependency(); 
                builder.RegisterType<ModalViewModel>().InstancePerDependency(); 

                builder.RegisterType<LoginWindow>().InstancePerDependency(); 
                builder.RegisterType<LoginViewModel>().InstancePerDependency();

                builder.RegisterType<RedactorForm>().InstancePerDependency();
                builder.RegisterType<RedactorViewModel>().InstancePerDependency();
                
                builder.RegisterType<RegistrationForm>().InstancePerDependency();
                builder.RegisterType<RegistrationViewModel>().InstancePerDependency();

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